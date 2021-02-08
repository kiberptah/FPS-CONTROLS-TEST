using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UI_ShowHideInteract : MonoBehaviour
{
    [SerializeField] Text text;
    string defaultInteractText;

    private void OnEnable()
    {
        EventDirector.showInteractUI += ShowUI;
        EventDirector.hideInteractUI += HideUI;
    }
    private void OnDisable()
    {
        EventDirector.showInteractUI -= ShowUI;
        EventDirector.hideInteractUI -= HideUI;
    }


    private void Awake()
    {
        defaultInteractText = text.text;
    }

    void ShowUI(string _text)
    {
        text.text += " " + _text;
        text.enabled = true;
    }
    void HideUI()
    {
        text.enabled = false;
        text.text = defaultInteractText;
    }
}
