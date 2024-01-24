using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterMovementHandler : NetworkBehaviour
{
    private NetworkCharacterController networkCharacterController;
    private NetworkAnimator networkAnimator;

    [SerializeField] private Camera localCamera;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private float cameraSens = 1;

    private void Awake()
    {
        networkCharacterController = GetComponent<NetworkCharacterController>();
        networkAnimator = GetComponent<NetworkAnimator>();
    }

    private void Start()
    {
        if (!Object.HasInputAuthority) 
        { 
            localCamera.enabled = false;
            audioListener.enabled = false;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData networkInputData))
        {
            transform.forward = networkInputData.aimForwardVector;//TODO: lerp or something, to animate and look better

            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
            transform.rotation = rotation;

            Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            moveDirection.Normalize();

            networkCharacterController.Move(moveDirection);

            networkAnimator.SetMoveAnim(moveDirection.magnitude > 0);

            if (networkInputData.isJumpPressed)
            {
                networkCharacterController.Jump();
            }
            if (networkInputData.isDashPressed)
            {
                networkCharacterController.StartDashing=true;
            }
            else
            {
                networkCharacterController.StartDashing=false;
            }
            if (networkInputData.isSprintPressed)
            {
                networkCharacterController.IsSprinting=true;
            }
            else
            {
                networkCharacterController.IsSprinting=false;
            }
            if(networkInputData.isKneelingPressed)
            {
                networkCharacterController.isKneeling=true;
            }
            else
            {
                networkCharacterController.isKneeling=false;    
            }

        }

    }
}