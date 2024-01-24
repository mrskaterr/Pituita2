using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskHandler : MonoBehaviour
{
    //[SerializeField] TMP_Text progressTxt;
    [SerializeField] TMP_Text toDo;
    int index = 0;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2.1f);
        index = GameManager.instance.GetComponent<NetworkRandomizeManager>().GetRandomNumber(0, 4);
    }

    void Update()
    {
        if(GameManager.instance == null) { return; }
        var mission = GameManager.instance.missionManager.rooms[index].missions[0];
        if (toDo.text != null)
        {
            toDo.text = mission.isDone ? "Mission accomplished" : mission.currentStep.description;
        }
    }
    //private void Init()
    //{
    //    List<MissionData> missions = GameManager.instance.missionManager.missions;

    //    for (int i = 0; i < missions.Count; i++)
    //    {
    //        missions[i].onDone += CheckProgress;
    //    }
    //}

    //private void CheckProgress()
    //{
    //    List<MissionData> missions = GameManager.instance.missionManager.missions;
    //    int c = 0;
    //    for (int i = 0; i < missions.Count; i++)
    //    {
    //        if (missions[i].isDone) { c++; }
    //    }
        
    //    progressTxt.text = $"{c} / {missions.Count}";
    //}
}