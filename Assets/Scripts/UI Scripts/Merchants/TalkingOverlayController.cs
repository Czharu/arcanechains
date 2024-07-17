using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkingOverlayController : MonoBehaviour
{
    public GameObject talkingOverlay;
    public TMP_Text characterNameText;
    public TMP_Text dialogText;
    public Button actionButton1; // e.g., Shop Button
    public Button actionButton2; // e.g., Close Button

    void Start()
    {
        talkingOverlay.SetActive(false); // Ensure the overlay is hidden initially
    }

    public void ShowOverlay(string characterName, string dialog, string action1Text, UnityEngine.Events.UnityAction action1, string action2Text, UnityEngine.Events.UnityAction action2)
    {
        talkingOverlay.SetActive(true);
        characterNameText.text = characterName;
        dialogText.text = dialog;
        actionButton1.GetComponentInChildren<TMP_Text>().text = action1Text;
        actionButton1.onClick.RemoveAllListeners();
        actionButton1.onClick.AddListener(action1);
        actionButton2.GetComponentInChildren<TMP_Text>().text = action2Text;
        actionButton2.onClick.RemoveAllListeners();
        actionButton2.onClick.AddListener(action2);
    }

    public void HideOverlay()
    {
        talkingOverlay.SetActive(false);
    }
}
