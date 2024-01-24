using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInfoPrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameTxt;
    [SerializeField] private Image roleIcon;

    public void SetRoleIcon(int _index)
    {
        roleIcon.sprite = LobbyManager.playerRoles.roles[_index].icon;
    }

    public void SetNick(string _nick)
    {
        playerNameTxt.text = _nick;
    }
}