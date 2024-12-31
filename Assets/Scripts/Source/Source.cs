using UnityEngine;
using UnityEngine.Events;

public abstract class Source
{
    public abstract Texture GetTexture();

    public abstract bool IsFrameReady();

    public abstract bool IsProcessedOnce();

    public abstract void Play();
}

public enum SourceType
{
    ImageSource,
    CameraSource,
    VideoSource
}
