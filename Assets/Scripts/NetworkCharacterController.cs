using System;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[OrderBefore(typeof(NetworkTransform))]
[DisallowMultipleComponent]
public class NetworkCharacterController : NetworkTransform
{
    [Header("Character Controller Settings")]
    public float gravity = -20.0f;
    public float jumpImpulse = 8.0f;
    public float acceleration = 10.0f;
    public float braking = 10.0f;
    public float maxSpeed = 2.0f;
    public float walkSpeed;
    private float originalWalkSpeed;
    public float rotationSpeed;
    public float viewVerticalSpeed;
    [Networked]
    [HideInInspector]
    public bool IsGrounded { get; set; }
    [HideInInspector]
    public bool StartDashing;
    public bool StartNinjaMode;
    
    [HideInInspector]
    public bool IsSprinting;
    [Networked]
    [HideInInspector]
    public Vector3 Velocity { get; set; }
    protected override Vector3 DefaultTeleportInterpolationVelocity => Velocity;
    protected override Vector3 DefaultTeleportInterpolationAngularVelocity => new Vector3(0f, 0f, RotationSpeed());
    public CharacterController Controller { get; private set; }
    private WeaponHandler weaponHandler;
    //private SprintSystem sprintSystem;
    //private DashSystem dashSystem;
    //private NinjaSystem ninjaSystem;
    //private AudioHandler audioHandler;

    void Start()
    {
        //ninjaSystem = GetComponent<NinjaSystem>();
        //dashSystem =  GetComponent<DashSystem>();
        weaponHandler = GetComponent<WeaponHandler>();
        //sprintSystem = GetComponent<SprintSystem>();
       // audioHandler = GetComponent<AudioHandler>();
        
    }
    protected override void Awake()
    {
        base.Awake();
        CacheController();
        walkSpeed=maxSpeed;
        originalWalkSpeed=walkSpeed;
    
    }
    public override void FixedUpdateNetwork()
    {     
       // sprintSystem?.Sprint();
        //ninjaSystem?.NinjaMode(StartNinjaMode);
        //ashSystem?.Dash(StartDashing);

    }
    public override void Spawned()
    {
        base.Spawned();
        CacheController();
    }

    private void CacheController()
    {
        if (Controller == null)
        {
            Controller = GetComponent<CharacterController>();

            Assert.Check(Controller != null, $"An object with {nameof(NetworkCharacterControllerPrototype)} must also have a {nameof(CharacterController)} component.");
        }
    }

    protected override void CopyFromBufferToEngine()
    {
        Controller.enabled = false;

        base.CopyFromBufferToEngine();

        Controller.enabled = true;
    }
    public virtual void Jump(bool ignoreGrounded = false, float? overrideImpulse = null)
    {
        if (IsGrounded || ignoreGrounded)
        {
            IsGrounded = false;
            var newVel = Velocity;
            newVel.y += overrideImpulse ?? jumpImpulse;
            Velocity = newVel;
        }
    }
    public virtual void Move(Vector3 direction)
    {
        var deltaTime = Runner.DeltaTime;
        var previousPos = transform.position;
        var moveVelocity = Velocity;

        direction = direction.normalized;

        if (IsGrounded)
        {
            moveVelocity.y = 0f;
        }

        moveVelocity.y += gravity * Runner.DeltaTime;

        var horizontalVel = default(Vector3);
        horizontalVel.x = moveVelocity.x;
        horizontalVel.z = moveVelocity.z;

        if (direction == default)
        {
            horizontalVel = Vector3.Lerp(horizontalVel, default, braking * deltaTime);
        }
        else
        {
            //if(IsGrounded)audioHandler.PlayStepAudio();
            horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * acceleration * deltaTime, maxSpeed);
        }

        moveVelocity.x = horizontalVel.x;
        moveVelocity.z = horizontalVel.z;

        Controller.Move(moveVelocity * deltaTime);

        Velocity = (transform.position - previousPos) * Runner.Simulation.Config.TickRate;
        IsGrounded = Controller.isGrounded;
    }
    public void MaxSpeed(bool _b)
    {
        if(_b)
            walkSpeed=originalWalkSpeed;
        else
            walkSpeed=0.5f;
    }

    public void Rotate(float _rotationInput)
    {
        transform.Rotate(0, _rotationInput * Runner.DeltaTime * RotationSpeed(), 0);
    }
    public float RotationSpeed()//ViewVerticalSpeed()
    { 
        if(weaponHandler)
            if(weaponHandler.isFiring)
                return rotationSpeed * 0.1f;
        return rotationSpeed;
    }
    public float ViewVerticalSpeed()
    {
        if(weaponHandler)
            if(weaponHandler.isFiring)
            return viewVerticalSpeed * 0.1f;
        
        return viewVerticalSpeed;

    }
}