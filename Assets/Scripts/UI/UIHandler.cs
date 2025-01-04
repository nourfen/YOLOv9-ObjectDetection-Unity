using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private Button startDetectionButton;
    [SerializeField] private Button openFileSelectionButton;
    [SerializeField] private TMP_InputField confidenceThreshold;
    [SerializeField] private Slider confidenceThresholdSlider;
    [SerializeField] private TMP_InputField iouThreshold;
    [SerializeField] private Slider iouThresholdSlider;
    [SerializeField] private TMP_Dropdown sourceTypeSelector;
    [SerializeField] private Detector detector;
    [SerializeField] private FileLoader fileLoader;
    [SerializeField] private GameObject userInterface;
    [SerializeField] private GameObject display;
    [SerializeField] private GameObject fileSelectorPrefab;

    private string _format = "0.00";

    private List<SourceType> sourceTypes = new List<SourceType> { SourceType.ImageSource, SourceType.CameraSource, SourceType.VideoSource };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensure the button is assigned
        if (startDetectionButton != null)
        {
            // Add a listener to the button
            startDetectionButton.onClick.AddListener(OnStartDetectionButtonClick);
        }

        // Ensure the button is assigned
        if (openFileSelectionButton != null)
        {
            // Add a listener to the button
            openFileSelectionButton.onClick.AddListener(OnOpenFileSelectonButton);
        }

        if (sourceTypeSelector != null)
        {
            sourceTypeSelector.onValueChanged.AddListener(OnSourceTypeChanged);
        }

        RegisterInputEvents();
    }

    private void RegisterInputEvents()
    {
        if (confidenceThresholdSlider != null)
        {
            confidenceThresholdSlider.onValueChanged.AddListener(OnConfidenceSliderValueChanged);
        }
        
        if (iouThresholdSlider != null)
        {
            iouThresholdSlider.onValueChanged.AddListener(OnIOUSliderValueChanged);
        }
        
        if (confidenceThreshold != null)
        {
            confidenceThreshold.onValueChanged.AddListener(OnConfidenceValueChanged);
        }
        
        if (iouThreshold != null)
        {
            iouThreshold.onValueChanged.AddListener(OnIOUValueChanged);
        }
    }

    private void OnConfidenceValueChanged(string value)
    {
        value = String.Format(value, _format);
        float valueFloat = float.Parse(value);
        confidenceThresholdSlider.value = valueFloat;
        Debug.Log("Confidence value changed to: " + value);
    }
    
    private void OnIOUValueChanged(string value)
    {
        value = String.Format(value, _format);
        float valueFloat = float.Parse(value);
        iouThresholdSlider.value = valueFloat;
        Debug.Log("IoU value changed to: " + value);
    }
    
    private void OnConfidenceSliderValueChanged(float value)
    {
        string valueString = value.ToString(_format);
        confidenceThresholdSlider.value = float.Parse(valueString);
        confidenceThreshold.text = valueString;
        Debug.Log("Confidence value changed to: " + valueString);
    }
    
    private void OnIOUSliderValueChanged(float value)
    {
        string valueString = value.ToString(_format);
        iouThresholdSlider.value = float.Parse(valueString);
        iouThreshold.text = valueString;
        Debug.Log("Confidence value changed to: " + valueString);
    }

    private void OnStartDetectionButtonClick()
    {
        float cTh = float.Parse(confidenceThreshold.text);
        float iouTh = float.Parse(iouThreshold.text);
        detector.StartDetection(cTh, iouTh);
        userInterface.SetActive(false);
        display.SetActive(true);
    }

    private void OnSourceTypeChanged(int value)
    {
        fileLoader.fileStatus.text = "";
        var sourceType = sourceTypes[value];
        if ((sourceType == SourceType.CameraSource) && (openFileSelectionButton.gameObject.activeSelf))
        {
            openFileSelectionButton.gameObject.SetActive(false);
        } else if ( ((sourceType == SourceType.VideoSource) || (sourceType == SourceType.ImageSource)) && (!openFileSelectionButton.gameObject.activeSelf) )
        {
            openFileSelectionButton.gameObject.SetActive(true);
        }
        fileLoader.SetDefaultFilter(sourceType);
    }

    private void OnOpenFileSelectonButton()
    {
        fileSelectorPrefab.SetActive(true);
        fileLoader.OpenFileBrowser();
    }

}
