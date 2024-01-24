using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private UnityEvent step;
    [SerializeField] private UnityEvent completed;
    float endTime;
    PlayerHUD blobHud;

    private void Update()
    {
        if (Time.time < endTime)
        {
            percent += Time.deltaTime;
        }
        else
        {
            percent = 0;
        }
    }

    private void Step()
    {
        step.Invoke();
        iterator++;
    }
    private void Completed()
    {
        completed.Invoke();
        NextTask();
    }
    public void StartInteract(GameObject @object)
    {
        //TODO: check if Todd, if yes then skip
        blobHud = @object.GetComponent<PlayerHUD>();
        if(@object.GetComponent<Equipment>().isHeHad((int)ItemToNeed)!=null)
        {
            itemToDestroy=@object.GetComponent<Equipment>().isHeHad((int)ItemToNeed);
            StartCoroutine(Holding());
        }    
        else
            Debug.Log("null");
    }

    public void StopInteract()
    {
        StopAllCoroutines();
        if (!saveProgress)
        {
            percent = 0;
        }
    }

    public virtual void OnFill()
    {
        Step();
        if(itemAmount<=iterator)
            Completed();
    }

    private IEnumerator Holding()
    {
        percent = 0;
        endTime = Time.time + holdTime;
        yield return new WaitForSeconds(holdTime);
        OnFill();
        blobHud.StopInteract();
    }
    public void DestroyUsedItem()
    {
        Destroy(itemToDestroy.gameObject);
    }
}