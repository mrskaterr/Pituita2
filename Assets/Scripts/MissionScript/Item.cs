using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MissionObject,IInteractable
{
    [SerializeField] Transform defaultPartent;
    [SerializeField] EnumItem.Item item;
    [SerializeField] float blockInteract=1f;
    Collider coll;
    private const string interactableLayerName = "Interactable";
    private const string notvisible = "P4";
    private int ID;
    private GameObject lastPlayer;
    void Start()
    {
        coll=transform.GetComponent<Collider>();
        ID=(int)item;
    }

    void Update()
    {
        if(transform.parent==defaultPartent){}

        else if(transform.GetComponentInParent<Morph>() 
        && transform.GetComponentInParent<Morph>().index==-1)
        {
            transform.position=transform.parent.position;
            this.gameObject.layer=LayerMask.NameToLayer(notvisible);
        }
        else
        {
            SeteDefaultPartent(null);
        }
    }
    protected override void OnInteract(GameObject @object)
    {

    }
    void TriggerInteract(GameObject @object)
    {
        if(lastPlayer==@object)
            return;
        if(@object.GetComponent<Equipment>().itemHolder.childCount==0)
        {
            coll.enabled=false;
            @object.GetComponent<Equipment>().Add(transform);
        }
        else if(@object.GetComponent<Equipment>().itemHolder.childCount==1)
        {
            coll.enabled=false;
            @object.GetComponent<Equipment>().itemHolder.GetChild(0).GetComponent<Item>().SeteDefaultPartent(@object);
            @object.GetComponent<Equipment>().Add(transform);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Morph>() && LayerMask.NameToLayer(interactableLayerName)==gameObject.layer)
            TriggerInteract(other.gameObject);
    }
    void OnTriggerExit(Collider other)
    {
        StartCoroutine(BlockInteract());
    }
    
    IEnumerator BlockInteract()
    {
        yield return new WaitForSeconds(blockInteract);
        lastPlayer=null;
    }
    public int GetID()
    {
        return ID;
    }
    public void SeteDefaultPartent(GameObject @object)
    {
        lastPlayer=@object;
        coll.enabled=true;
        transform.SetParent(defaultPartent);
        this.gameObject.layer=LayerMask.NameToLayer(interactableLayerName);
    }

}
