using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class PlayerInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera playerCamera;
    public float interactionDistance = 3f;
    public KeyCode interactionKey = KeyCode.F;
    public KeyCode subinteractionKey = KeyCode.E;
    private PlayerInventory playerInventory;
    private DoorTrigger currentDoor;
    private GameObject currentInteractableObject = null;
    private ItemPickup currentItem;
    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(interactionKey))
        {
            if (currentItem != null)
            {
                HandleItemPickup();
            }
            else if (currentDoor != null)
            {
                HandleDoorInteraction();
            }

        }

        if (Input.GetKeyDown(subinteractionKey) && currentDoor != null)
        {
            currentDoor.subInteract(playerInventory);
        }
    }

    private void HandleItemPickup()
    {
        string monologueMessage = "";

        switch (currentItem.itemType)
        {
            case ItemType.Hammer:
                playerInventory.hasHammer = true;
                monologueMessage = "Maybe I can use this to break...";
                break;
            case ItemType.Phone:
                playerInventory.hasPhone = true;
                if(playerInventory.hasPhoneBattery) monologueMessage = "A message on the screen, saying that it's the principal's 65th birthday today.";
                else monologueMessage = "I need a battery for this.";
                break;
            case ItemType.PhoneBattery:
                playerInventory.hasPhoneBattery = true;
                if (playerInventory.hasPhone) monologueMessage = "A message on the screen, saying that it's the principal's 65th birthday today.";
                else monologueMessage = "Looks like a phone battery. How I can make use of this?";
                break;
            case ItemType.KeyChain:
                playerInventory.hasKeyChain = true;
                monologueMessage = "It looks like maintenance keychain.";
                break;
        }

        UIManager.Instance.ShowNotification(currentItem.pickupNotification + currentItem.itemType.ToString());
        if (monologueMessage != "")
        {
            UIManager.Instance.ShowMonologue(monologueMessage);
        }
        currentItem.Pickup();
    }

    private void HandleDoorInteraction()
    {
        if(currentDoor != null) currentDoor.Interact(playerInventory);
    }

    void CheckForInteractable()
    {
        RaycastHit[] hits = Physics.RaycastAll(playerCamera.transform.position, playerCamera.transform.forward, interactionDistance);


        if (hits.Length > 0)
        {
            System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
            GameObject HighPriority = null;
            DoorTrigger LowPriotity = null;
            foreach (var hit in hits)
            {
                ItemPickup item = hit.collider.GetComponent<ItemPickup>();
                DoorTrigger door = hit.collider.GetComponentInParent<DoorTrigger>();
                if (item != null)
                {
                    HighPriority = item.gameObject;
                    break;
                }
                if (door != null)
                {
                    if (door.isOpen && LowPriotity == null)
                    {
                        LowPriotity = door;
                        continue;
                    }
                    else if (!door.isOpen)
                    {
                        HighPriority = door.gameObject;
                        break;
                    }
                }
            }
            GameObject FinalHit = HighPriority != null ? HighPriority : (LowPriotity != null ? LowPriotity.gameObject : null);
            if (FinalHit != currentInteractableObject)
            {
                if (currentInteractableObject != null)
                {
                    HideCurrentPrompt();
                }
                currentInteractableObject = FinalHit;
                if (currentInteractableObject != null)
                {
                    ShowCurrentPrompt();
                }
            }

            if(currentInteractableObject == null)
            {
                HideCurrentPrompt();
            }
        }
        else HideCurrentPrompt();
    }

    void ShowCurrentPrompt()
    {
        if (currentInteractableObject == null) return;
        currentItem = currentInteractableObject.GetComponent<ItemPickup>();
        currentDoor = currentInteractableObject.GetComponentInParent<DoorTrigger>();

        if (currentItem != null)
        {
            UIManager.Instance.ShowInteractionPrompt("Press F to Pick Up " + currentItem.itemType.ToString());
            return;
        }
        if (currentDoor != null)
        {
            currentDoor.ShowInteractionPrompt(playerInventory);
            return;
        }
    }

    void HideCurrentPrompt()
    {
        currentItem = null;
        currentDoor = null;
        UIManager.Instance.HideInteractionPrompt();
    }
}

