using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;


public class HackingMission : MissionObject,IInteractable
{
    [SerializeField]WordsHacking wordsHacking;
    public GameObject player;
    [SerializeField] GameObject can;
    [SerializeField] TMP_Text Password;
    [SerializeField] TMP_Text[] line0;
    [SerializeField] TMP_Text[] line1;
    [SerializeField] TMP_Text[] line2;
    [SerializeField] TMP_Text[] line3;
    [SerializeField] TMP_Text[] line4;
    private char[] word;
    private TMP_Text[] line;
    private TMP_Text[][] line22;
    private int j = 0;
    private string buff = "";
    private bool stop;
    private bool click = false;
    public bool isDone=false;
    protected override void OnInteract(GameObject @object)
    {
        isDone=true;
        gameObject.GetComponent<Collider>().enabled=false;
        mission.NextStep();
    }
    void OnDisable()
    {
        player.GetComponent<CharacterInputHandler>().enabled=true;
    }
    //     player.GetComponent<CharacterInputHandler>().enabled=false;
    //     @object.GetComponent<NetworkCharacterController>().enabled=false;
    //     @object.GetComponent<CharacterController>().enabled=false;
    //     @object.GetComponent<CharacterMovementHandler>().enabled=false;
    //     blokada obracania
    //  }
    void OnEnable()
    {
        word = wordsHacking.GetHackingPassword();
        Password.text = word.ArrayToString();
        player.GetComponent<CharacterInputHandler>().enabled=false;
    }
    //     player.GetComponent<CharacterInputHandler>().enabled=true;
    //     @object.GetComponent<NetworkCharacterController>().enabled=false;
    //     @object.GetComponent<CharacterController>().enabled=false;
    //     @object.GetComponent<CharacterMovementHandler>().enabled=false;
    //     blokada obracania
    // }

    void Start()
    {
        line22 = new TMP_Text[5][];

        line22[0] = line0;
        line22[1] = line1;
        line22[2] = line2;
        line22[3] = line3;
        line22[4] = line4;

        for (int i = 0; i < 5; i++)
            line22[i][Rand()].SetText(word[i].ToString());

    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
            click = true;


        else if (click)
        {
            j++;
            if (j >= 5)
                j = 0;

            click = false;
            for (int i = 0; i < 5; i++)
            {
                if (line22[i][2].text != word[i].ToString())
                    break;

                if (i == 4)
                {
                    Debug.Log("hack done");
                    stop = true;
                    isDone=true;
                    can.SetActive(false);
                    wordsHacking.done();
                    return;
                }
            }

        }
        else if (!stop)
            StartCoroutine(Turn());


    }
    int Rand()
    {
        return Random.Range(0, 5);
    }
    IEnumerator Turn()
    {
        stop = true;
        yield return new WaitForSeconds(0.5f);
        buff = line22[j][0].text;
        for (int i = 0; i < 4; i++)
        {
            line22[j][i].SetText(line22[j][i + 1].text);
        }
        line22[j][4].SetText(buff);
        stop = false;
    }
}