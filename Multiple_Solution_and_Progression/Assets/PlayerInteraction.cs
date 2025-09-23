using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera playerCamera;
    public float interactionDistance = 3f; // 交互的最大距离
    public KeyCode interactionKey = KeyCode.F;

    private PlayerInventory playerInventory;
    private DoorTrigger currentDoor;
    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(interactionKey) && currentDoor != null)
        {
            switch (currentDoor.currentLockType)
            {
                case LockType.Unlocked:
                    currentDoor.ToggleDoorState();
                    break;

                case LockType.KeyLocked:
                    if (playerInventory.hasKeyChain)
                    {
                        currentDoor.UnlockWithKey();
                    }
                    else
                    {
                        Debug.Log("I don't have the key for this door.");
                    }
                    break;

                case LockType.PasswordLocked:
                    Debug.Log("This should open a password UI keypad!");
                    break;
            }
        }
    }

    void CheckForInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
        {
            DoorTrigger door = hit.collider.GetComponentInParent<DoorTrigger>();

            if (door != null) 
            {
                if (door != currentDoor) 
                {
                    
                    if(currentDoor != null) currentDoor.HideInteractionPrompt();
                    currentDoor = door;
                    currentDoor.ShowInteractionPrompt();
                }
            }
            else 
            {
                if(currentDoor != null)
                {
                    currentDoor.HideInteractionPrompt();
                    currentDoor = null;
                }
            }
        }
        else 
        {
            if (currentDoor != null)
            {
                currentDoor.HideInteractionPrompt();
                currentDoor = null;
            }
        }
    }
}
