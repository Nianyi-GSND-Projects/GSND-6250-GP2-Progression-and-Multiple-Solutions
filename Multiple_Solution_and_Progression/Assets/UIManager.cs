using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI temporaryNotificationText;
    public TextMeshProUGUI interactionPromptText;

    [Header("Settings")]
    public float notificationDisplayTime = 2f;

    private Coroutine notificationCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ShowNotification(string message)
    {
        if (notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }
        notificationCoroutine = StartCoroutine(NotificationRoutine(message));
    }

    private IEnumerator NotificationRoutine(string message)
    {
        temporaryNotificationText.text = message;
        temporaryNotificationText.gameObject.SetActive(true);
        yield return new WaitForSeconds(notificationDisplayTime);
        temporaryNotificationText.gameObject.SetActive(false);
    }

    public void ShowInteractionPrompt(string message)
    {
        if (interactionPromptText == null) return;
        interactionPromptText.text = message;
        interactionPromptText.gameObject.SetActive(true);
    }
    
    public void HideInteractionPrompt()
    {
        if (interactionPromptText == null) return;
        interactionPromptText.gameObject.SetActive(false);
    }
}