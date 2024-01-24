using UnityEngine;
using TMPro;

public class TestLogin : MonoBehaviour
{
    [SerializeField] TMP_InputField nick;
    [SerializeField] TMP_InputField pass;

    public void SetLogin(string _nick)
    {
        nick.text = _nick;
        pass.text = "123456";
    }
}