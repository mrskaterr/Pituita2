using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableHold : MissionObject, IInteractableHold
{
    [SerializeField] EnumItem.Item ItemToNeed;
    public int interactionAmount;
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
        iterator++;
        step.Invoke();

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
            if(@object.GetComponent<HackerSystem>())
            {
                holdTime = 0.01f;

            }

            @object.GetComponent<AudioHandler>().InteractLoading(true);
            StartCoroutine(Holding(@object));

        }    
        else
            Debug.Log("null");
    }

    public void StopInteract(GameObject @object)
    {
       // @object.GetComponent<AudioHandler>().InteractLoading(false);
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
        if (interactionAmount <= iterator)
            Completed();
            
    }

    private IEnumerator Holding(GameObject @object)
    {
        percent = 0;
        endTime = Time.time + holdTime;
        yield return new WaitForSeconds(holdTime);
        OnFill(@object);
        blobHud.StopInteract();
    }
    public void DestroyUsedItem()
    {
        Destroy(itemToDestroy.gameObject);
    }
    public void ChangeDescriptioon(string a)
    {
        mission.currentStep.description = a + " (" + iterator + "/" + interactionAmount + ")";
    }
}