using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphBullet : MonoBehaviour
{
    private float s;

    private void Start()
    {
        s = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        s += 5 * Time.deltaTime;

        transform.localScale = new Vector3(1 - Mathf.Abs(Mathf.Sin(s) * .25f), 1 - Mathf.Abs(Mathf.Cos(s) * .25f), 1);
    }
}