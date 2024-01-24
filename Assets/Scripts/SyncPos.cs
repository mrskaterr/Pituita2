using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPos : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update()
    {
        transform.position = target.position + target.forward * .6f;
        transform.rotation = target.rotation;
    }
}