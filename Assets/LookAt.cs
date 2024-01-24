using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    public void SetTarget(FindingTriggerMission target)
    {
        this.transform.LookAt(new Vector3( target.player.position.x,  this.transform.position.y, target.player.position.z ) );
    }

}
