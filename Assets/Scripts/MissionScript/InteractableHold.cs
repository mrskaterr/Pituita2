using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;

public class InteractableHold : MissionObject, IInteractableHold
{
    [SerializeField] EnumItem.Item ItemToNeed;
    public int itemAmount=1;
    [SerializeField] protected float holdTime = 1f;
    public string desc = "Opening ...";
    //public string Description { get; protected set; } = "Opening ...";
    [SerializeField] protected bool saveProgress = false;
    public float percent { get; protected set; } = 0;
    private readonly float interval = .1f;
    private Transform itemToDestroy;
    private int iterator=0;
    [Space]
    [SerializeField] private UnityEvent step;
    [SerializeField] private UnityEvent completed;
    public bool oneStep=true;


    private void Step()
    {
        step.Invoke();
        iterator++;
    }
    private void Completed()
    {
        completed.Invoke();
        if(oneStep)NextTask();
    }
    public void StartInteract(GameObject @object)
    {
        
        if(@object.GetComponent<Equipment>().isHeHad((int)ItemToNeed)!=null)
        {
            itemToDestroy=@object.GetComponent<Equipment>().isHeHad((int)ItemToNeed);
            @object.GetComponent<AudioHandler>().InteractLoading(true);
            StartCoroutine(Holding(@object));
        }    
        else
            Debug.Log("null");
    }

    public void StopInteract(GameObject @object)
    {
        @object.GetComponent<AudioHandler>().InteractLoading(false);
        StopAllCoroutines();
        if (!saveProgress)
        {
            percent = 0;
        }
    }

    public virtual void OnFill(GameObject @object)
    {
        @object.GetComponent<AudioHandler>().InteractLoading(false);
        @object.GetComponent<AudioHandler>().InteractDone();
        Step();
        if(itemAmount<=iterator)
            Completed();
    }

    private IEnumerator Holding(GameObject @object)
    {
        while (percent < holdTime)
        {
            yield return new WaitForSeconds(interval);
            percent += interval;
        }
        OnFill(@object);
    }
    public void DestroyUsedItem()
    {
        Destroy(itemToDestroy.gameObject);
    }
}