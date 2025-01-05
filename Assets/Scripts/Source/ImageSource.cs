using System.IO;
using UnityEngine;

public class ImageSource : Source
{
    private Texture2D _texture = new(2, 2);
    private bool _isLoaded = false;
    public ImageSource(string path)
    {
        LoadTextureFromFile(path);
    }

    public override Texture GetTexture()
    {
        return _isLoaded ? _texture : null;
    }

    public override bool IsFrameReady()
    {
        return _isLoaded;
    }

    public override bool IsProcessedOnce()
    {
        return true;
    }

    public override void Play()
    {
        throw new System.NotImplementedException();
    }

    public override void Dispose()
    {
        _isLoaded = false;
        _texture = null;
    }

    void LoadTextureFromFile(string path)
    {
        if (File.Exists(path))
        {
            byte[] imageData = File.ReadAllBytes(path);

            _isLoaded = _texture.LoadImage(imageData);
            // Load the image data into the texture
            if (!_isLoaded)
            {
                Debug.LogError("Failed to load image data into texture.");
            }  
        }
        else
        {
            Debug.LogError("File not found at path: " + path);
        }
    }
}
