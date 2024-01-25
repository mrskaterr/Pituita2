using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    bool isOnSpot;
    [SerializeField] Collider Spot;
    void Start()
    {
        isOnSpot=false;
    }
    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.name==Spot.gameObject.name)
        {
            isOnSpot=true;
        }
        else if(isOnSpot && coll.gameObject.GetComponent<Morph>()!=null)
        {
            gameObject.gameObject.SetActive(false);
        }

    }
}
