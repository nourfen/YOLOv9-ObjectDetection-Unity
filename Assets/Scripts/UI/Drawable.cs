using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drawable
{
    // Object Pooling
    private ObjectPool<BoundingBox> _boundingBoxPool;
    private List<BoundingBox> _activeBoundingBoxes;
    private GameObject _boundingBoxPrefab;
    // Display
    private readonly RawImage _screen;
    private RectTransform _screenRectTransform;
    
    private float _screenWidth;
    private float _screenHeight;

    public Drawable()
    {
        var display = GameObject.Find("Display");
        _screenRectTransform = display.GetComponent<RectTransform>();
        _screen = display.GetComponent<RawImage>();
    }

    public void SetDisplayResolution(int displayWidth, int displayHeight)
    {
        _screenRectTransform.sizeDelta = new Vector2(displayWidth, displayHeight);
        PrepareBoundingBoxPool();
    }

    public void SetTexture(Texture texture)
    {
        if (!texture)
        {
            Debug.LogError("The given texture is null");
            return;
        }

        _screen.texture = texture;
    }
    
    public void DrawBoundingBoxes(List<YoloPrediction> yoloPredictions)
    {
        // Calculate the offset for center-middle anchoring
        float offsetX = _screenWidth / 2;
        float offsetY = _screenHeight / 2;

        foreach (YoloPrediction prediction in yoloPredictions)
        {
            // Get a bounding box from the pool
            BoundingBox boundingBox = _boundingBoxPool.Get();
            _activeBoundingBoxes.Add(boundingBox);

            // Get the RectTransform of the bounding box
            RectTransform boxRectTransform = boundingBox.GetComponent<RectTransform>();

            var predBoundingBox = prediction.BoundingBox;

            // Calculate the box's position relative to the center of the image (center-middle anchoring)
            float xMin = predBoundingBox.xMin - offsetX;
            float yMin = predBoundingBox.yMin - offsetY;
            float width = predBoundingBox.width;
            float height = predBoundingBox.height;

            // Set the size and position of the bounding box
            boxRectTransform.anchoredPosition = new Vector2(xMin + width / 2, yMin + height / 2); // Center the box
            boxRectTransform.sizeDelta = new Vector2(width, height);

            // Update the bounding box appearance
            boundingBox.SetColor(prediction.ClassColor);
            boundingBox.SetLabel($"{prediction.ClassName} ({prediction.Score:F2})");
        }
    }

    public void ResetBoundingBoxes()
    {
        foreach (BoundingBox box in _activeBoundingBoxes)
        {
            _boundingBoxPool.ReturnToPool(box);
        }
        _activeBoundingBoxes.Clear();
    }

    private void PrepareBoundingBoxPool()
    {
        _activeBoundingBoxes = new List<BoundingBox>();
        _screenRectTransform = _screen.GetComponent<RectTransform>();
        _boundingBoxPrefab = Resources.Load<GameObject>("Prefabs/BBox");

        if (_boundingBoxPrefab != null)
        {
            _screenWidth = _screenRectTransform.rect.width;
            _screenHeight = _screenRectTransform.rect.height;
            _boundingBoxPool = new ObjectPool<BoundingBox>(
                _boundingBoxPrefab.GetComponent<BoundingBox>(),
                initialSize: 10,
                parent: _screenRectTransform
            );
        }
    }
}
