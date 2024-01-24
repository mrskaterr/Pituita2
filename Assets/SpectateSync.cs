using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Fusion;

public class SpectateSync : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnChangeVer))]
    public int ver { get; set; }

    [SerializeField] private Transform anchor;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, anchor.position.y, transform.position.z);
    }

    private static void OnChangeVer(Changed<SpectateSync> _changed)
    {
        _changed.Behaviour.SetVerRot();
    }

    private void SetVerRot()
    {
        transform.eulerAngles = new Vector3(ver * .0001f, transform.eulerAngles.y);
    }
}