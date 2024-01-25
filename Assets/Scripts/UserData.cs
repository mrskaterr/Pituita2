using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserData : MonoBehaviour
{
    [SerializeField] private TMP_Text nameTxt;
    [SerializeField] private TMP_Text scoreTxt;
    [SerializeField] private Image image;
    [Space]
    [SerializeField] private Sprite[] roleIcons;

    public void SetData(string _name, int _score, int _roleIndex)
    {
        nameTxt.text = _name;
        scoreTxt.text = _score.ToString();
        image.sprite = roleIcons[_roleIndex];
    }
}