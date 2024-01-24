using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hack_Panel : MonoBehaviour
{
    public GameObject player;
    [SerializeField] Job job;
    [SerializeField] Transform left, right;
    [SerializeField] Image[] images;
    int lRot, rRot;
    int lRand, rRand;


    float XScal, YScal;


    [SerializeField] Color green;
    [SerializeField] Color blue;
    [SerializeField] RectTransform sinRand;
    [SerializeField] RectTransform sin;
    int c = 0;
    bool stop;

    private void Start()
    {
        Rand();
        sinRand.localScale=new Vector3(XScal, YScal, 1);
    }

    private void FixedUpdate()
    {
        if (c <= 3 && !stop)
        {
            if (Input.GetKey(KeyCode.A))
            {
                left.Rotate(Vector3.back * 1.5f);
            }


            else if (Input.GetKey(KeyCode.D))
            {
                left.Rotate(Vector3.forward * 1.5f);
            }
            //sin.localScale = new Vector3((lRand)/359f, sin.localScale.y, 1f);

            if (Input.GetKey(KeyCode.LeftArrow)) { right.Rotate(Vector3.back * 1.5f); }
            else if (Input.GetKey(KeyCode.RightArrow)) { right.Rotate(Vector3.forward * 1.5f); }

            lRot = (int)left.localEulerAngles.z;
            rRot = (int)right.localEulerAngles.z;

            if (CheckLeft()) { left.GetComponent<Image>().color = green; }
            else { left.GetComponent<Image>().color = blue; }

            if (CheckRight()) { right.GetComponent<Image>().color = green; }
            else { right.GetComponent<Image>().color = blue; }

            if (CheckLeft() && CheckRight())
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
        XScal = Random.Range(0.5f, 6f);
        YScal = Random.Range(0.10f,1f);
        Debug.Log((int)(XScal / 6f * 359));
        Debug.Log((int)(YScal / 6f * 359));
        lRand = (int)(XScal / 6f * 359);
        rRand = (int)(YScal / 1f * 359);
    }


    bool CheckLeft()
    {
        int L = lRot;
        int target = lRand;
        int diff = Mathf.Abs(L - target);
        return diff <= 5;
    }

    bool CheckRight()
    {
        int R = rRot;
        int target = rRand;
        int diff = Mathf.Abs(R - target);
        return diff <= 5;
    }

    IEnumerator Pass()
    {
        stop = true;
        left.GetComponent<Image>().color = green;
        right.GetComponent<Image>().color = green;
        if (c <= 3) { images[c - 1].color = green; }
        yield return new WaitForSeconds(.5f);
        stop = false;
    }
}