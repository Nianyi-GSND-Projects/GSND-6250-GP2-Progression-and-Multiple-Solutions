using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Password : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField passwordInputField;
    public Button Enter;
    public Button Close;
    public TextMeshProUGUI ErrorText;

    public TextMeshProUGUI AcceptedText;

    private DoorTrigger targetDoor;

    void Awake()
    {
        Enter.onClick.AddListener(OnEnterButtonClicked);
        Close.onClick.AddListener(OnCloseButtonClicked);
    }

    public void OpenPasswordUI(DoorTrigger door)
    {
        targetDoor = door;
        AcceptedText.gameObject.SetActive(false);
        ErrorText.gameObject.SetActive(false);
        passwordInputField.text = "";
        passwordInputField.ActivateInputField();
    }

    private void OnEnterButtonClicked()
    {
        if (targetDoor == null) return;

        if (passwordInputField.text == targetDoor.correctPassword)
        {
            AcceptedText.gameObject.SetActive(true);
            ErrorText.gameObject.SetActive(false);
            targetDoor.ForceUnlock();
            targetDoor.ToggleDoorState();
        }
        else
        {
            ShowError("Incorrect Password");
        }
    }

    private void OnCloseButtonClicked()
    {
        UIManager.Instance.HidePasswordUI();
    }

    private void ShowError(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ErrorRoutine(message));
    }

    private IEnumerator ErrorRoutine(string message)
    {
        ErrorText.text = message;
        ErrorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        ErrorText.gameObject.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
