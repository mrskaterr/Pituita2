using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskHandler : MonoBehaviour
{
    //[SerializeField] TMP_Text progressTxt;
    [SerializeField] TMP_Text toDo;
    //[SerializeField] WayPoint wayPoint;
    static int indexRoom;
    static MissionData mission;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2.1f);
        indexRoom = 0;// eManager.instance.GetComponent<NetworkRandomizeManager>().GetRandomNumber(0, 4);
        mission = GameManager.instance.missionManager.rooms[indexRoom].missions[0];


       
        for (int i = 0; i < mission.currentStep.missionObjects.Count; i++)
        {
            mission.currentStep.missionObjects[i].Enable();
        }
        //wayPoint.SetTarget(mission.currentStep.missionObjects[0].transform);
        Debug.Log(mission.currentStep.missionObjects[0].transform.name) ;
    }

    void Update()
    {
        if(GameManager.instance == null) { return; }
        if (toDo != null && mission !=null)
        {
            toDo.text = mission.isDone ? "Mission accomplished" : mission.currentStep.description;

            //wayPoint.SetTarget( mission.currentStep.missionObjects[0].transform);
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