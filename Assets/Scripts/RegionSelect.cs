using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionSelect : MonoBehaviour
{
    [SerializeField] private LobbyManager lobbyManager;
    
    private Animator animator;

    private const string toggle = "toggle";
    private const string regionSelected = "regionSelected";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(regionSelected))
        {
            SetRegion(PlayerPrefs.GetInt(regionSelected));
        }
        else
        {
            PlayerPrefs.SetInt(regionSelected, 0);
            SetRegion(0);
        }
    }

    public void ToggleAnim()
    {
        animator.SetTrigger(toggle);
    }

    public void SetRegion(int _index)
    {
        PlayerPrefs.SetInt(regionSelected, _index);
        lobbyManager.SetRegion(_index);
    }
}