using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class EscapeGameLogic : NetworkBehaviour
{
    [SerializeField] private Bar progressBar;
    [Networked(OnChanged = nameof(OnScoreChange))]
    public int score { get; set; }
    [SerializeField] private KeyCode[] keys;
    [SerializeField] private CaptureHandler captureHandler;
    private int counter = 0;
    private KeyCode lastKey = KeyCode.None;
    private KeyCode nextKey = KeyCode.None;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            lastKey = KeyCode.W;
            if (nextKey == lastKey || nextKey == KeyCode.None)
            {
                SetNextKey();
                counter++;
            }
            else
            {
                nextKey = KeyCode.None;
                counter = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            lastKey = KeyCode.D;
            if (nextKey == lastKey || nextKey == KeyCode.None)
            {
                SetNextKey();
                counter++;
            }
            else
            {
                nextKey = KeyCode.None;
                counter = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            lastKey = KeyCode.S;
            if (nextKey == lastKey || nextKey == KeyCode.None)
            {
                SetNextKey();
                counter++;
            }
            else
            {
                nextKey = KeyCode.None;
                counter = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            lastKey = KeyCode.A;
            if (nextKey == lastKey || nextKey == KeyCode.None)
            {
                SetNextKey();
                counter++;
            }
            else
            {
                nextKey = KeyCode.None;
                counter = 0;
            }
        }
        else 
        {
            progressBar.SetFill(score);
            return;
        }
        if (counter >= 4)
        {
            score += 5;
            counter = 0;
        }
    }

    private void SetNextKey()
    {
        if (lastKey == KeyCode.W) { nextKey = KeyCode.D; }
        else if (lastKey == KeyCode.D) { nextKey = KeyCode.S; }
        else if (lastKey == KeyCode.S) { nextKey = KeyCode.A; }
        else if (lastKey == KeyCode.A) { nextKey = KeyCode.W; }
    }

    private static void OnScoreChange(Changed<EscapeGameLogic> _changed)
    {
        _changed.Behaviour.OnScore();
    }

    private void OnScore()
    {
        if (score >= 85)
        {
            captureHandler.isFree = true;
            score = 0;
        }
        progressBar.SetFill(score);
    }
}