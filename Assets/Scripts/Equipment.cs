using System.Collections; 
using System.Collections.Generic; 
using Unity.VisualScripting; 
using UnityEngine; 
using TMPro; 
 
public class Equipment : MonoBehaviour 
{ 
    public Transform itemHolder;
    public GameObject itemIcon; 
    void Update() 
    { 
        if(itemHolder.childCount==0)
            itemIcon.SetActive(false);
        
        if(itemHolder.childCount==1)
            itemIcon.SetActive(true);

    } 
    public Transform isHeHad(int ID) 
    { 
        int count=itemHolder.childCount; 
        if (count==1 && itemHolder.GetChild(0).GetComponent<Item>().GetID()==ID) 
            return itemHolder.GetChild(0); 
        else 
        {        
            Debug.Log("equipment  count:"+count); 
            return null; 
        } 
    } 
 
    public bool Add(Transform t) 
    { 
        t.gameObject.SetActive(true); 
        if(itemHolder.childCount==0) 
        { 
            t.SetParent(itemHolder); 
            return true; 
        } 
        return false; 
    } 
 
} 