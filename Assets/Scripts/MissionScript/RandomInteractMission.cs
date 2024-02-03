using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Fusion;

public class RandomInteractMission : MissionObject,IInteractable
{
    [Networked] public int Rand {get;set;}
    private int iterator=0;
    [SerializeField] private int Max;
    [SerializeField] Transform Item;
    [SerializeField] private UnityEvent step;
    [SerializeField] private UnityEvent completed;
    private void Step()
    {
        step.Invoke();
        Randomize();
    }
    private void Completed(GameObject @object)
    {
        completed.Invoke();
        @object.GetComponent<Equipment>().Add(Item);
        NextTask();
    }
    protected override void OnInteract(GameObject @object)
    {
        if(@object.GetComponent<HackerSystem>())
        {
            Completed(@object);
        }
        if(iterator<Max)
            Step();
        if(Rand==Max-1 )
            Completed(@object);
    }
    private void Randomize()
    {
        Rand = Random.Range(iterator,Max);
        iterator++;
    }
}
