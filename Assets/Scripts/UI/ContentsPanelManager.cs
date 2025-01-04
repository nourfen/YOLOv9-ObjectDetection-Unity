using TMPro;
using UnityEngine;

public class ContentsPanelManager : MonoBehaviour
{
    public GameObject[] panels;
    public TMP_Text mainTitle;
    private int _currentSelectedPanelIndex = 0;
    private string[] _panelNames = new [] {"Dashboard", "Yolo Models", "About", "Settings"};

    void Start()
    {
        mainTitle.text = _panelNames[_currentSelectedPanelIndex];
    }
    public void OnPanelChanged(int index)
    {
        if (_currentSelectedPanelIndex == index) return;
        panels[_currentSelectedPanelIndex].SetActive(false);
        _currentSelectedPanelIndex = index;
        panels[_currentSelectedPanelIndex].SetActive(true);
        mainTitle.text = _panelNames[_currentSelectedPanelIndex];
    }
}
