using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class BoundingBox : MonoBehaviour
{
    private Image _boxImage;
    private TMP_Text _label;
    private Image _textBg;
    private RectTransform _textRectTransform;

    void Awake()
    {
        _boxImage = GetComponent<Image>();
        _label = GetComponentInChildren<TMP_Text>();
        _textBg = transform.GetChild(0).GetComponent<Image>();
        _textRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void SetColor(Color color1, Color color2)
    {
        if (_boxImage != null)
        {
            _boxImage.color = color1;
            color2.a = 1;
            _textBg.color = color2;
        }
    }

    public void SetLabel(string text)
    {
        if (_label != null)
        {
            _label.text = text;
        }
    }
}
