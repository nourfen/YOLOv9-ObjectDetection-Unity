using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private GameObject _currentSelectedButton;
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
        UnhighlightOldButton(_currentSelectedButton);
        _currentSelectedButton = selectedButton;
        HighlightNewButton(_currentSelectedButton);
    }

    private void HighlightNewButton(GameObject selectedButton)
    {
        var outline = selectedButton.GetComponent<Outline>();
        outline.enabled = true;
    }

    private void UnhighlightOldButton(GameObject selectedButton)
    {
        var outline = selectedButton.GetComponent<Outline>();
        outline.enabled = false;
    }
}


