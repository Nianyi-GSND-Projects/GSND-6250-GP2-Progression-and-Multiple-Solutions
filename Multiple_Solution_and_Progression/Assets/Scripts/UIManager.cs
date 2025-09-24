using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI temporaryNotificationText;
    public TextMeshProUGUI interactionPromptText;

    public GameObject monologuePanel;

    public TextMeshProUGUI monologueText;

    [Header("Settings")]
    public float notificationDisplayTime = 2f;
    public float displaytime = 10f;
    private Coroutine notificationCoroutine;
    private Coroutine monologueCoroutine;

    [Header("Password UI")]
    public GameObject passwordUI;
    public Password passwordScript;

    private PlayerMovement playerMovement;



    void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        if (UIManager.Instance != null)
        {
            StartCoroutine(OpenSceneMonologue());
        }
    }
    private IEnumerator OpenSceneMonologue()
    {
        UIManager.Instance.ShowMonologue("I must find a way to Principal's Office and falsify my grades, or else I'll be expelled!");
        yield return new WaitForSeconds(UIManager.Instance.displaytime + 1.0f);
        UIManager.Instance.ShowMonologue("It seems that nobody is here now. This my only chance.");
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

    public void ShowMonologue(string message)
    {
        if (monologueCoroutine != null)
        {
            StopCoroutine(monologueCoroutine);
        }
        monologueCoroutine = StartCoroutine(MonologueRoutine(message));
    }

    private IEnumerator MonologueRoutine(string message)
    {
        monologueText.text = message;
        monologuePanel.SetActive(true);
        yield return new WaitForSeconds(displaytime);
        monologuePanel.SetActive(false);
    }

    public void ShowPasswordUI(DoorTrigger door)
    {
        if (passwordUI == null || passwordScript == null) return;
        passwordUI.SetActive(true);
        HideInteractionPrompt();
        passwordScript.OpenPasswordUI(door);
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HidePasswordUI()
    {
        if (passwordUI == null || passwordScript == null) return;
        passwordUI.SetActive(false);
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}