using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MissionData : NetworkBehaviour
{
    public string title = "Title";
    public List<MissionStep> steps = new List<MissionStep>();
    public MissionStep currentStep;
    [Networked(OnChanged = nameof(OnDoneChanged))] public bool isDone { get; set; } = false;

    public Action onNextStep;
    public Action onDone;

    private int stepIndex = 0;
    public void Init()
    {
        currentStep = steps[0];
        for (int i = 0; i < steps.Count; i++)
        {
            steps[i].Init(this);
        }
    }

    public void NextStep()//TOFIX: Add bool and check last with false
    {
        currentStep.LockStep();
        stepIndex++;
        if(stepIndex >= steps.Count)
        {
            Done();
            return;
        }
        currentStep = steps[stepIndex];
        currentStep.UnlockStep();
        onNextStep?.Invoke();
    }

    private void Done()
    {
        isDone = true;
    }

    private static void OnDoneChanged(Changed<MissionData> _changed)
    {
        _changed.Behaviour.OnDone();
    }

    private void OnDone()
    {
        if(onDone != null) { onDone(); }
    }

    [System.Serializable]
    public class MissionStep
    {
        public string description = "Description";
        public List<MissionObject> missionObjects = new List<MissionObject>();

        public void Init(MissionData _mission)
        {
            for (int i = 0; i < missionObjects.Count; i++)
            {
                if(missionObjects[i]==null)
                    continue;
                missionObjects[i].mission = _mission;
                //missionObjects[i].Enable();
            }
        }

        public void LockStep()
        {
            for (int i = 0; i < missionObjects.Count; i++)
            {
                missionObjects[i].Disable();
            }
        }

        public void UnlockStep()
        {
            for (int i = 0; i < missionObjects.Count; i++)
            {
                missionObjects[i].Enable();
            }
        }
    }
}