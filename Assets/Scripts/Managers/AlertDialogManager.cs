using System;
using UnityEngine;
using UnityEngine.UI;

public class AlertDialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public Text messageText;
    public Button okButton;
    public Button cancelButton;

    private System.Action<bool> responceCallBack;

    private void Start()
    {
        dialogBox.SetActive(false);

        okButton.onClick.AddListener(() => HandleResponse(true));
        cancelButton.onClick.AddListener(() => HandleResponse(false));
    }

    public void ShowDialog(string message, System.Action<bool> callBack)
    {
        responceCallBack = callBack;
        messageText.text = message;
        dialogBox.SetActive(true);
    }

    private void HandleResponse(bool responce)
    {
        dialogBox.SetActive(false);
        responceCallBack?.Invoke(responce);
    }
}