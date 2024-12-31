using UnityEngine;
using UnityEngine.Video;

public class VideoSource : Source
{
    private readonly VideoPlayer _videoPlayer;
    private long _lastProcessedFrame = -1; // Track the last processed frame
    //private bool _frameReady = false;

    public VideoSource(string path)
    {
        _videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
        _videoPlayer.url = path;
    }

    public override Texture GetTexture()
    {
        return _videoPlayer.texture;
    }

    public override bool IsFrameReady()
    {
        // Check if a new frame is available from the VideoPlayer
        if (!_videoPlayer.isPlaying || _videoPlayer.frame <= 0) return false;
        
        // Only process the frame if it's new
        if (_videoPlayer.frame == _lastProcessedFrame) return false;
        
        _lastProcessedFrame = _videoPlayer.frame;
        return true;
    }

    public override bool IsProcessedOnce()
    {
        return false;
    }

    public override void Play()
    {
        _videoPlayer.Play();
    }
}
