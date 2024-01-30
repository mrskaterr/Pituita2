using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTask : NetworkBehaviour, IInteractable
{
    [SerializeField] private GameObject VFX;
    [SerializeField] private GameObject oldModel, newModel;
    [SerializeField] private int score = 10;

    public void Interact(GameObject @object)
    {
        GameManager.instance.scoreManager.Score += score;
        Rpc_ChangeModel();
    }

    [Rpc]
    public void Rpc_ChangeModel()
    {
        oldModel.SetActive(false);
        VFX.SetActive(true);
        newModel.SetActive(true);
        GetComponent<BoxCollider>().enabled = false;
    }
}