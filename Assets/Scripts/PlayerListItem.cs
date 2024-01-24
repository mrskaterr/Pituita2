using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityInspector;

public class PlayerListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameTxt;
    [SerializeField] private Image icon;

    public void SetContent(string _playerName, Sprite _sprite)
    {
        playerNameTxt.text = _playerName;
        icon.sprite = _sprite;
    }

    public void SetColor(Color _color)
    {
        playerNameTxt.color = _color;
    }

    public string GetName() { return playerNameTxt.text; }
}