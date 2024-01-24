using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;

public class Equipment : MonoBehaviour
{
    public Transform itemHolder;
    [SerializeField] TMP_Text nameItem;
    [SerializeField] Image iconItem;
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
            //iconItem.sprite = t.GetComponent<Item>().icon;
            nameItem.text=t.name;
            return true;
        }
        return false;
    }
    public void ResetIcon()
    {
        //iconItem.sprite=null;
        nameItem.text="";
    }

}
