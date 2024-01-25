using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WayPoint : MonoBehaviour
{
    [SerializeField] Image pointImg;
    [SerializeField] Transform target;
    [SerializeField] TMP_Text meter;
    [SerializeField] Camera cam;
    void Update()
    {

        float minX = pointImg.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = pointImg.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        pointImg.transform.position = cam.WorldToScreenPoint(target.position);
        
        Vector2 pos = cam.WorldToScreenPoint(target.position);

        if(Vector3.Dot((target.position - transform.position),transform.forward) < 0)
        {
            if(pos.x < Screen.width/2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        pointImg.transform.position = pos;

        meter.text = Vector3.Distance(target.position, transform.position).ToString() + "m";
        
    }
}



