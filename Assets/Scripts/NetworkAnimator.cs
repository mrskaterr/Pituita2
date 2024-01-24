using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkAnimator : NetworkBehaviour
{
    [SerializeField] private BodyType bodyType;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bodyAimTarget;

    private NetworkCharacterController controller;

    private const string move_ParamName = "isMoving";
    private const string sprint_ParamName = "isSprinting";
    private const string grounded_ParamName = "isGrounded";
    private const string hold_ParamName = "isHolding";

    [Networked(OnChanged = nameof(OnHeightChange))]
    private float height { get; set; }

    private void Awake()
    {
        controller = GetComponent<NetworkCharacterController>();
    }

    private void Update()
    {
        animator.SetBool(grounded_ParamName, controller.IsGrounded);
    }

    public void SetMoveAnim(bool _isMoving)
    {
        animator.SetBool(move_ParamName, _isMoving);
    }

    public void SetHoldAnim(bool _isHolding)
    {
        animator.SetBool(hold_ParamName, _isHolding);
    }

    public void SetAimTargetPos(float _height)
    {
        if(bodyType == BodyType.Human)
        {
            height = (1f - Mathf.Clamp01(_height)) * 2f - 1f;
        }
    }

    private static void OnHeightChange(Changed<NetworkAnimator> _changed)
    {
        _changed.Behaviour.SetSpine();
    }

    private void SetSpine()
    {
        bodyAimTarget.transform.localPosition = new Vector3(bodyAimTarget.transform.localPosition.x, height, bodyAimTarget.transform.localPosition.z);
    }

    private enum BodyType { Human, Blob }
}