using UnityEngine;

public class CameraSource : Source
{
    WebCamTexture webcamTexture;

    public CameraSource()
    {
        webcamTexture = new WebCamTexture();
    }

    public override Texture GetTexture()
    {
        return webcamTexture;
    }

    public override bool IsFrameReady()
    {
        return webcamTexture.didUpdateThisFrame;
    }

    public override bool IsProcessedOnce()
    {
        return false;
    }

    public override void Play()
    {
        webcamTexture.Play();
    }

    public override void Dispose()
    {
        webcamTexture.Stop();
    }
}
