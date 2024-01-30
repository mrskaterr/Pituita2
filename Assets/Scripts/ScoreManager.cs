using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class ScoreManager : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnScoreUpdate), OnChangedTargets = OnChangedTargets.All)]
    public int Score { get; set; }

    [SerializeField] private Image progressBar;
    [SerializeField] private int scoreTarget = 100;

    private static void OnScoreUpdate(Changed<ScoreManager> _changed)
    {
        _changed.Behaviour.Rpc_MyStaticRpc(_changed.Behaviour.Score);
        //If Score greater than scoreTarget end game ( blobs wins )
    }

    private void UpdateBar(int _points)
    {
        progressBar.fillAmount = (float) _points / (float) scoreTarget;
        Score = _points;
    }

    [Rpc]
    public void Rpc_MyStaticRpc(int a)
    {
        UpdateBar(a);
        if(Score >= scoreTarget)
        {
            RPCManager.Local.RPC_GameOver(RPCManager.Team.Blobs);
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.I))
    //    {
    //        Score += 5;
    //    }
    //}
}