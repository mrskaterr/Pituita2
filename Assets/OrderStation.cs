using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderStation : MissionObject
{
    int iterator=0;
    
    public void Done()
    {
        iterator++;
        mission.currentStep.description = "Infect Stations(" + iterator + "/ 5)";
        Debug.Log(iterator);
        if (transform.childCount <= iterator)
        {
            transform.GetChild(0).GetComponent<InteractableHold>().DestroyUsedItem();
            NextTask();
        } 

    }

}
