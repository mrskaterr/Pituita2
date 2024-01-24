using UnityEngine;
using TMPro;

public static class TMPTextExtension
{
    public static TMP_Text SetContent(this TMP_Text _text, string _content)
    {
        _text.text = _content;
        return null;
    }
    public static TMP_Text SetContent(this TMP_Text _text, string _content, Color _color)
    {
        _text.text = _content;
        _text.color = _color;
        return null;
    }
}