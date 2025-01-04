using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoundingBox : MonoBehaviour
{
    private Image boxImage;
    private TMP_Text label;

    void Awake()
    {
        boxImage = GetComponent<Image>();
        label = GetComponentInChildren<TMP_Text>();
    }

    public void SetColor(Color color)
    {
        if (boxImage != null)
        {
            boxImage.color = color;
            label.color = Color.red;
        }
    }

    public void SetLabel(string text)
    {
        if (label != null)
        {
            label.text = text;
        }
    }
}
