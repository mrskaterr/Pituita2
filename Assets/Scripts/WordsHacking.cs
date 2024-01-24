using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class WordsHacking : MissionObject,IInteractable
{
    public GameObject player;
    [SerializeField] GameObject hackingPanel;
    [SerializeField] HackingMission hackingMission;
    [SerializeField] Transform PendriveWirus;
    Transform EmptyPendrive;
    public bool isDone=false;
    public string[] Words;

    protected override void OnInteract(GameObject @object)
    {
        player = @object.gameObject;
        EmptyPendrive=@object.GetComponent<Equipment>().isHeHad((int)EnumItem.Item.EmptyPendrive);
        if(EmptyPendrive!=null)
        {
            @object.GetComponent<CharacterInputHandler>().enabled=false;
            hackingPanel.SetActive(true);
            hackingMission.player=@object.gameObject;
        }

    }
    public void done()
    {
        Destroy(EmptyPendrive.gameObject);
        Debug.Log(player.name);
        PendriveWirus.gameObject.SetActive(true);
        player.GetComponentInChildren<Equipment>().Add(PendriveWirus);
        PendriveWirus.SetParent(player.GetComponentInChildren<Equipment>().itemHolder);
        NextTask();
    }
    public char[] GetHackingPassword()
    {
        return Words[Random.Range(0, Words.Length)].ToCharArray();
    }
}
