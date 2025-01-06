using UnityEngine;
using Unity.Sentis;
using System.Collections.Generic;

public class Detector : MonoBehaviour
{
    private const int TARGET_WIDTH = 640;
    private const int TARGET_HEIGHT = 640;

    public FileLoader fileLoader; 

    // Object Detection
    private ModelAsset _modelAsset;
    private Drawable _screen;
    private Model _runtimeModel;
    private Worker _worker;
    private Yolo _yolo;
    private Source _source;
    
    //
    private bool _resolutionWasSet = false;

    void Start()
    {
        // Initialise Classes
        _yolo = new Yolo();
        _modelAsset = Resources.Load<ModelAsset>("Models/yolov9-c");
        _runtimeModel = ModelLoader.Load(_modelAsset);
        _worker = new Worker(_runtimeModel, BackendType.GPUCompute);

        fileLoader.OnSourceDetected += OnSourceChanged;
    }

    void Update()
    {
        if (_source == null || _source.IsProcessedOnce())
            return;

        if (_source.IsFrameReady())
        {
            SetResolutionOnce();
            DetectFrame();
        }
    }

    private void OnDisable()
    {
        _worker.Dispose();
    }

    public void Dispose()
    {
        _source.Dispose();
        _screen.Dispose();
        _screen = null;
        _resolutionWasSet = false;
    }

    public void StartDetection(float cTh, float iouTh)
    {
        _yolo.IouThreshold = iouTh;
        _yolo.ConfidenceThreshold = cTh;
        _screen = new Drawable();
        
        if (_source.IsProcessedOnce())
        {
            SetResolutionOnce();
            DetectFrame();
        } else
        {
            _source.Play();
        }
    }
    void OnSourceChanged(SourceType sourceType, string path)
    {
        if (sourceType == SourceType.ImageSource) {
            _source = new ImageSource(path);
        }
        else if (sourceType == SourceType.VideoSource)
        {
            _source = new VideoSource(path);
        } else
        {
            _source = new CameraSource();
        }
    }

    private void SetResolutionOnce()
    {
        if (_resolutionWasSet) return;
        var texture = _source.GetTexture();
        _screen.SetDisplayResolution(texture.width, texture.height);
        _resolutionWasSet = true;
    }
    private void DetectFrame()
    {
        // Get the newly generated texture
        Texture texture = _source.GetTexture();

        // Remove the old bounding boxes
        _screen.ResetBoundingBoxes();

        // Display the texture
        _screen.SetTexture(texture);

        // Prepare the input tensor
        Tensor<float> inputTensor = TextureConverter.ToTensor(texture, TARGET_WIDTH, TARGET_HEIGHT, 3);

        // Run the model on the input
        _worker.Schedule(inputTensor);

        // Get output tensor
        Tensor<float> outputTensor = _worker.PeekOutput() as Tensor<float>;
        // Process Model Output
        List<YoloPrediction> predictions = _yolo.Predict(outputTensor, texture.width, texture.height);

        // Draw the new bounding boxes 
        _screen.DrawBoundingBoxes(predictions);

        // Dispose tensors
        outputTensor?.Dispose();
        inputTensor.Dispose();
    }
}


