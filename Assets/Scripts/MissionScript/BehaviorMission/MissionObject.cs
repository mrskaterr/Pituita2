using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionObject : MonoBehaviour, IInteractable 
{
    [HideInInspector] public MissionData mission;
    static ScoreManager scoreManager;
    private const string defaultLayerName = "Default";
    private const string interactableLayerName = "Interactable";
    void Start()
    {
        scoreManager=GameManager.instance.GetComponent<ScoreManager>();
    }
    public void Disable()
    {
        int layerIndex = LayerMask.NameToLayer(defaultLayerName);
        gameObject.layer = layerIndex;
    }

    public void Enable()
    {
        int layerIndex = LayerMask.NameToLayer(interactableLayerName);
        gameObject.layer = layerIndex;
    }

    public void Interact(GameObject @object)
    {
        OnInteract(@object);
    }

    protected virtual void OnInteract(GameObject @object)
    {
        mission.NextStep();
    }
    public void NextTask(int score)
    {
        scoreManager.AddScore(score);
        mission.NextStep();
    }
        public void NextTask()
    {
        scoreManager.AddScore(5);
        mission.NextStep();
    }
    public void AddScore(int score)
    {
        scoreManager.AddScore(score);
    }
}