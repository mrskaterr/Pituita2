using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Words_Panel : MonoBehaviour
{
    [SerializeField] char[] word;
    [SerializeField] GameObject can;
    [SerializeField] Job job;
    [SerializeField] TMP_Text[] line0;
    [SerializeField] TMP_Text[] line1;
    [SerializeField] TMP_Text[] line2;
    [SerializeField] TMP_Text[] line3;
    [SerializeField] TMP_Text[] line4;
    private TMP_Text[] line;
    private TMP_Text[][] line22;
    private int j = 0;
    private string buff = "";
    private bool stop;
    private bool click = false;
    public bool isDone=false;

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
                    Debug.Log("donee");
                    stop = true;
                    isDone=true;
                    can.SetActive(false);
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