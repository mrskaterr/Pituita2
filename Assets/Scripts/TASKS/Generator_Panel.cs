using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator_Panel : MonoBehaviour
{
    public int diffrend;
    [SerializeField] Job job;
    [SerializeField] Transform left;
    [SerializeField] Image[] images;
    int lRot;
    int lRand;
    [SerializeField] Color green;
    [SerializeField] Color blue;
    int c = 0;
    bool stop;

    private void Start()
    {
        Rand();
    }

    private void FixedUpdate()
    {
        if (c <= 3 && !stop)
        {
            left.Rotate(Vector3.back * 1.5f);

            lRot = (int)left.localEulerAngles.z;

            if (CheckLeft())
            { 
                left.GetComponent<Image>().color = green; 
            }
            else
            { 
                left.GetComponent<Image>().color = blue; 
            }
            
            if(!CheckLeft() && Input.GetKey(KeyCode.Space))
            {
                if(c>0)
                    c--;
                StartCoroutine(Depass());
            }
            else if (CheckLeft() && Input.GetKey(KeyCode.Space))
            {
                c++;
                StartCoroutine(Pass());
                if (c == 3)
                {
                    job.active = false;
                    stop = true;
                    return;
                }
                Rand();
            } 
        }
    }

    void Rand()
    {
        lRand = Random.Range(0, 360);
    }

    bool CheckLeft()
    {
        int L = lRot;
        int target = lRand;
        int diff = Mathf.Abs(L - target);
        Debug.Log(diff);
        return diff <= diffrend;
    }

    IEnumerator Pass()
    {
        stop = true;
        left.GetComponent<Image>().color = green;
        if (c <= 3) { images[c - 1].color = green; }
        yield return new WaitForSeconds(.5f);
        stop = false;
        left.localEulerAngles=new Vector3(0,0,0);
    }
    IEnumerator Depass()
    {
        stop = true;
        images[c].color = blue;
        yield return new WaitForSeconds(.5f);
        stop = false;
        left.localEulerAngles=new Vector3(0,0,0);
    }
}