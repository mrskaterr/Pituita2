using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct NetworkInputData : INetworkInput
{
    //TOIMPROVE: chunky variables
    public Vector2 movementInput;
    public Vector3 aimForwardVector;
    public NetworkBool isJumpPressed;
    public NetworkBool isFirePressed;
    public NetworkBool isDashPressed;
    public NetworkBool isSprintPressed;
    public NetworkBool isKneelingPressed;
}