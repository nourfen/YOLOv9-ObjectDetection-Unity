using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
public class FileLoader : MonoBehaviour
{
    public Action<SourceType, string> OnSourceDetected;
    public TMP_Text fileStatus;

    private List<SourceType> _sourceTypes = new List<SourceType> { SourceType.ImageSource, SourceType.CameraSource, SourceType.VideoSource };
    private string _path = "";
    private SourceType _sourceType = SourceType.ImageSource;

    public Color statusOk;
    public Color statusError;
    // Method to open the file browser
    public void OpenFileBrowser()
    {
        // Set filters for images and videos
        FileBrowser.SetFilters(true,
            new FileBrowser.Filter("Images", ".jpg", ".png", ".jpeg"),
            new FileBrowser.Filter("Videos", ".mp4", ".avi", ".mov"));

        // Set default filter
        FileBrowser.SetDefaultFilter(".jpg");

        // Show the file browser
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    // Coroutine to handle the file selection
    IEnumerator ShowLoadDialogCoroutine()
    {
        // Wait for the user to select a file
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Select File", "Load");

        // Check if a file was selected
        if (FileBrowser.Success)
        {
            fileStatus.color = statusOk;
            string path = FileBrowser.Result[0];

            // Get the file extension
            string extension = Path.GetExtension(path).ToLower();
            string fileName = Path.GetFileName(path);
            
            // Check if the file is an image
            if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
            {
                LoadImage(path);
                fileStatus.text = $"Successfully loaded image: {fileName}";
            }
            // Check if the file is a video
            else if (extension == ".mp4" || extension == ".avi" || extension == ".mov")
            {
                LoadVideo(path);
                fileStatus.text = $"Successfully loaded video: {fileName}";
            }
            else
            {
                Debug.LogWarning("Unsupported file type");
                fileStatus.text = "Error: Unsupported file type.";
                fileStatus.color = statusError;
            }
        }
        else
        {
            fileStatus.text = "Error while loading file.";
            fileStatus.color = statusError;
        }
    }

    public void SetDefaultFilter(SourceType sourceType)
    {
        _sourceType = sourceType;
        if (sourceType == SourceType.ImageSource)
        {
            Debug.Log("Setting Default To: Image");
            FileBrowser.SetDefaultFilter(".jpg");
        }
        else if (sourceType == SourceType.VideoSource)
        {
            Debug.Log("Setting Default To: Video");
            FileBrowser.SetDefaultFilter(".mp4");
        } else
        {
            OnSourceDetected?.Invoke(sourceType, "");
        }
    }

    // Method to load and play the video
    void LoadVideo(string path)
    {
        _path = path;
        OnSourceDetected?.Invoke(_sourceType, path);
    }

    // Coroutine to load and display the image
    void LoadImage(string path)
    {
        _path = path;
        OnSourceDetected.Invoke(_sourceType, path);
    }

    public void Dispose()
    {
        fileStatus.text = "";
        _path = "";
        //_sourceType = SourceType.ImageSource;
    }
}
