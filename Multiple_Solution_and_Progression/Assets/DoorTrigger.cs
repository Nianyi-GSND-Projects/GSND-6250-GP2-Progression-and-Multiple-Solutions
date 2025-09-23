using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum LockType { Unlocked, KeyLocked, PasswordLocked }
public enum KeyType { None, KeyChain}
public class DoorTrigger : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90.0f;
    public float animationSpeed = 2.0f;

    [Header("Interaction Settings")]
    public LockType currentLockType = LockType.Unlocked;
    public string correctPassword = "1234";

    [Header("UI Settings")]
    public string openPrompt = "Press F to Close";
    public string closedPrompt = "Press F to Open";
    public string needsKeyPrompt = "Locked"; 
    public string needsPasswordPrompt = "Press F to Enter Password";

    [Header("Events for Linked Doors")]
    public UnityEvent OnDoorToggle;
    public UnityEvent OnUnlock;

    private Quaternion closedRotation;

    private Quaternion openRotation;

    private bool isOpen = false;


    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * animationSpeed);
    }

    public void ToggleDoorState()
    {
        if (currentLockType != LockType.Unlocked) return;
        isOpen = !isOpen;
        UpdateInteractionPromptOnStateChange();
        OnDoorToggle.Invoke();
    }
    
    public void ForceUnlock()
    {
        currentLockType = LockType.Unlocked;
    }
    public void UnlockWithKey()
    {
        if (currentLockType == LockType.KeyLocked)
        {
            ForceUnlock(); 
            OnUnlock.Invoke(); 
            UIManager.Instance.ShowNotification("Unlocked with key");
            ToggleDoorState();
        }
    }

    public bool AttemptPasswordUnlock(string passwordAttempt)
    {
        if (currentLockType == LockType.PasswordLocked && passwordAttempt == correctPassword)
        {
            currentLockType = LockType.Unlocked;
            UIManager.Instance.ShowNotification("Access Granted");
            ToggleDoorState(); 
            return true;
        }
        UIManager.Instance.ShowNotification("Wrong Password");
        return false;
    }

    public void ShowInteractionPrompt()
    {
        string prompt = GetCurrentPromptText();
        UIManager.Instance.ShowInteractionPrompt(prompt);
    }

    public void HideInteractionPrompt()
    {
        UIManager.Instance.HideInteractionPrompt();
    }

    private void UpdateInteractionPromptOnStateChange()
    {
        ShowInteractionPrompt();
    }
    private string GetCurrentPromptText()
    {
        switch (currentLockType)
        {
            case LockType.Unlocked:
                return isOpen ? openPrompt : closedPrompt;
            case LockType.KeyLocked:
                return needsKeyPrompt;
            case LockType.PasswordLocked:
                return needsPasswordPrompt;
            default:
                return ""; 
        }
    }

}
