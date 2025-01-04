using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private GameObject _currentSelectedButton;
    
    //Colors:
    private readonly Color _selectedColor = new Color32(19, 15, 64, 255);
    private readonly Color _unselectedColor = new Color32(255, 255, 255, 255);
    private readonly Color _hoverColor = new Color32(186, 220, 88, 255);  //6AB04C
    
    public GameObject[] buttonContainer;

    void Start()
    {
        _currentSelectedButton = buttonContainer[0];
        foreach (GameObject button in buttonContainer)
        {
            AddEventTrigger(button);
            button.GetComponent<Button>().onClick.AddListener((() => { OnSelected(button);}));
        }
    }

    void AddEventTrigger(GameObject button)
    {
        // Add an EventTrigger component if it doesn't already exist
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.AddComponent<EventTrigger>();
        }

        // Add PointerEnter event
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        pointerEnterEntry.callback.AddListener((eventData) => { OnHoverStart(button); });
        trigger.triggers.Add(pointerEnterEntry);

        // Add PointerExit event
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        pointerExitEntry.callback.AddListener((eventData) => { OnHoverEnd(button); });
        trigger.triggers.Add(pointerExitEntry);
    }
    
    private void OnHoverStart(GameObject selectedButton)
    {
        if (selectedButton == _currentSelectedButton) return;
        var outline = selectedButton.GetComponent<Outline>();
        outline.enabled = true;
    }

    private void OnHoverEnd(GameObject selectedButton)
    {
        if (selectedButton == _currentSelectedButton) return;
        var outline = selectedButton.GetComponent<Outline>();
        outline.enabled = false;
    }

    private void OnSelected(GameObject selectedButton)
    {
        if (selectedButton == _currentSelectedButton) return;
        var outline = selectedButton.GetComponent<Outline>();
        outline.enabled = false;
        UnhighlightOldButton(_currentSelectedButton);
        _currentSelectedButton = selectedButton;
        HighlightNewButton(selectedButton);
    }

    private void HighlightNewButton(GameObject selectedButton)
    {
        var buttonBgImage = selectedButton.GetComponent<Image>();
        var text = selectedButton.GetComponentInChildren<TMP_Text>(); 
        var icon = selectedButton.GetComponentInChildren<RawImage>();
        
        text.color = _selectedColor;
        icon.color = _selectedColor;
        buttonBgImage.color = _hoverColor;
    }

    private void UnhighlightOldButton(GameObject selectedButton)
    {
        var buttonBgImage = selectedButton.GetComponent<Image>();
        var text = selectedButton.GetComponentInChildren<TMP_Text>(); 
        var icon = selectedButton.GetComponentInChildren<RawImage>();
        
        text.color = _unselectedColor;
        icon.color = _unselectedColor;
        buttonBgImage.color = _selectedColor;
    }
}


