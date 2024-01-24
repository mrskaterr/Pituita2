using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryGlobal : MonoBehaviour
{
    public List<CarryHandler> carries = new List<CarryHandler>();

    private void Start()
    {
        carries.Clear();
        var tmp = FindObjectsOfType<CarryHandler>();
        for (int i = 0; i < tmp.Length; i++)
        {
            carries.Add(tmp[i]);
        }
    }

    public CarryHandler GetCarry()
    {
        for (int i = 0; i < carries.Count; i++)
        {
            if (carries[i].available) { return carries[i]; }
        }
        return null;
    }
}