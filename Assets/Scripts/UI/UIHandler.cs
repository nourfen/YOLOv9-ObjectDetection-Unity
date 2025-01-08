using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private Button startDetectionButton;
    [SerializeField] private Button openFileSelectionButton;
    [SerializeField] private Button resetButton;
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

    private readonly string _format = "0.00";

    private readonly List<SourceType> _sourceTypes = new List<SourceType> { SourceType.ImageSource, SourceType.CameraSource, SourceType.VideoSource };

    #region Unity Events
    void Start()
    {
        RegisterButtonsEvents();

        RegisterSourceSelectorEvents();

        RegisterThresholdControlsEvents();
    }
    
    #endregion
    
    #region Private Methods
    private void OnReset()
    {
        fileLoader.Dispose();
        detector.Dispose();
    }

    #region Event Handlers
    private void RegisterButtonsEvents()
    {
        // Ensure the button is assigned
        if (startDetectionButton != null)
        {
            // Add a listener to the button
            startDetectionButton.onClick.AddListener(OnStartDetectionButtonClick);
        }
        
        // Ensure the button is assigned
        if (resetButton != null)
        {
            // Add a listener to the button
            resetButton.onClick.AddListener(OnReset);
        }
        
        // Ensure the button is assigned
        if (openFileSelectionButton != null)
        {
            // Add a listener to the button
            openFileSelectionButton.onClick.AddListener(OnOpenFileSelectionButtonClick);
        }
    }

    private void RegisterSourceSelectorEvents()
    {
        if (sourceTypeSelector != null)
        {
            sourceTypeSelector.onValueChanged.AddListener(OnSourceTypeChanged);
        }
    }

    private void RegisterThresholdControlsEvents()
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
    #endregion

    #region Event Functions
    private void OnConfidenceValueChanged(string value)
    {
        value = String.Format(value, _format);
        float valueFloat = float.Parse(value);
        confidenceThresholdSlider.value = valueFloat;
    }
    
    private void OnIOUValueChanged(string value)
    {
        value = String.Format(value, _format);
        float valueFloat = float.Parse(value);
        iouThresholdSlider.value = valueFloat;
    }
    
    private void OnConfidenceSliderValueChanged(float value)
    {
        string valueString = value.ToString(_format);
        confidenceThresholdSlider.value = float.Parse(valueString);
        confidenceThreshold.text = valueString;
    }
    
    private void OnIOUSliderValueChanged(float value)
    {
        string valueString = value.ToString(_format);
        iouThresholdSlider.value = float.Parse(valueString);
        iouThreshold.text = valueString;
    }

    private void OnStartDetectionButtonClick()
    {
        float cTh = float.Parse(confidenceThreshold.text);
        float iouTh = float.Parse(iouThreshold.text);
        detector.StartDetection(cTh, iouTh);
    }

    private void OnSourceTypeChanged(int value)
    {
        fileLoader.fileStatus.text = "";
        var sourceType = _sourceTypes[value];
        if ((sourceType == SourceType.CameraSource) && (openFileSelectionButton.gameObject.activeSelf))
        {
            openFileSelectionButton.gameObject.SetActive(false);
        } else if (sourceType is SourceType.VideoSource or SourceType.ImageSource && !openFileSelectionButton.gameObject.activeSelf)
        {
            openFileSelectionButton.gameObject.SetActive(true);
        }
        fileLoader.SetDefaultFilter(sourceType);
    }

    private void OnOpenFileSelectionButtonClick()
    {
        fileSelectorPrefab.SetActive(true);
        fileLoader.OpenFileBrowser();
    }
    #endregion
    
    #endregion
    
    
    
    

   

}
