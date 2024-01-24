using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class JobHandler2 : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnVarChange))]
    public bool VarTask { get; set; }

    [SerializeField] GameObject TaskIndicator;
    static GameObject TaskIndicator3;

    void Start()
    {
        if(Object.HasInputAuthority)
            TaskIndicator3=TaskIndicator;

    }
    static void OnVarChange(Changed<JobHandler2> _changed)
    {
        _changed.Behaviour.OnTaskDone();
    }

    void OnTaskDone()
    {
        RPC_OnTaskDone();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_OnTaskDone()
    {
        TaskIndicator3.SetActive(true);
    } 
}
