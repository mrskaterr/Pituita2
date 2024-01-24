using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class JobHandler : NetworkBehaviour
{ 
    [Networked(OnChanged = nameof(OnVarChange))]
    public bool VarTask { get; set; }

    [SerializeField] GameObject TaskIndicator;
    static GameObject TaskIndicator2;

    void Start()
    {
        if(Object.HasInputAuthority)
            TaskIndicator2=TaskIndicator;

        
    }
    // Start is called before the first frame update
    static void OnVarChange(Changed<JobHandler> _changed)
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
        TaskIndicator2.SetActive(true);
    } 
}
