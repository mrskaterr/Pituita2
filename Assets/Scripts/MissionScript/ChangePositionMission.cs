using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class ChangePositionMission : MissionObject,IInteractable
{
    [SerializeField] float pushPower;
    [SerializeField] Collider Area;
    private Rigidbody rb;
    [SerializeField] private UnityEvent toDo;
    private void ToDo()
    {
        toDo.Invoke();
        NextTask();
    }
    void Start()
    {
        rb=GetComponent<Rigidbody>();
    }
    protected override void OnInteract(GameObject @object)
    {
        rb.AddForce(@object.transform.forward*pushPower);
    }
    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.name==Area.gameObject.name)
            ToDo();
        
    }
}

