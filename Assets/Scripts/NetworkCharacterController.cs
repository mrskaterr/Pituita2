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
    public float maxStamina=5f;
    public float maxSpeed = 2.0f;
    public float runSpeed = 20f;
    private float walkSpeed;
    public float kneelingSpeed=1f;
    public float rotationSpeed = 15.0f;
    public float viewVerticalSpeed = 50;
    public float DashSpeed=10f;
    public int DashMaxAmount=2;
    public int DashCurrentAmount=0;
    
    public float MaxDashTime=1f;
    public float DashResetTime=100f;
    private float currentDashTime;
    private float currentDashResetTime;
    [Networked]
    [HideInInspector]
    public bool IsGrounded { get; set; }
    [HideInInspector]
    public bool StartDashing;
    
    [HideInInspector]
    public bool IsSprinting;
    private float cunrrentStamina=0;
    [HideInInspector]
    public bool isKneeling;

    [Networked]
    [HideInInspector]
    public Vector3 Velocity { get; set; }
    protected override Vector3 DefaultTeleportInterpolationVelocity => Velocity;
    protected override Vector3 DefaultTeleportInterpolationAngularVelocity => new Vector3(0f, 0f, rotationSpeed);

    [SerializeField] Hitbox hitbox;
    private Vector3 orginalControlerCenter;
    private float orginalHeighControler;
    private Vector3 kneelingControlerCenter=new Vector3(0,-0.5f,0);
    private float kneelingHeigh=1f; 

    public CharacterController Controller { get; private set; }
    private CharacterInputHandler inputHandler;
    protected override void Awake()
    {
        base.Awake();
        CacheController();
        currentDashTime = MaxDashTime;
        walkSpeed=maxSpeed;
        orginalHeighControler=Controller.height;
        orginalControlerCenter=Controller.center;
        inputHandler=GetComponent<CharacterInputHandler>();
    }
    public override void FixedUpdateNetwork()
    {
        if(IsSprinting && cunrrentStamina>0f)
        {
            cunrrentStamina-=Time.deltaTime;
            if(cunrrentStamina<=0f)
                inputHandler.canSprinting=false;
        }
        else if(!IsSprinting && cunrrentStamina<=maxStamina)
        {
            cunrrentStamina+=Time.deltaTime;
            if(cunrrentStamina>=maxStamina)
                inputHandler.canSprinting=true;
        }

      
        if (StartDashing &&  DashCurrentAmount<DashMaxAmount )
        {
            DashCurrentAmount++;
            currentDashTime = 0.0f;
            currentDashResetTime= 0.0f;
            StartDashing=false;
        }
        if (currentDashTime < MaxDashTime)
        {
            maxSpeed=DashSpeed;
            currentDashTime += Time.fixedDeltaTime;
        }
        else
        {
            maxSpeed=walkSpeed;
            currentDashResetTime += Time.fixedDeltaTime;
            if(currentDashResetTime>=DashResetTime)
                DashCurrentAmount=0;
        }
    }
    public override void Spawned()
    {
        base.Spawned();
        CacheController();
    }
    public void Kneeling(LocalCameraHandler camera)
    {
        Controller.height=kneelingHeigh;
        Controller.center=kneelingControlerCenter;
        camera.ChangePositionCam(kneelingHeigh-orginalHeighControler);
    }
    public void Standing(LocalCameraHandler camera)
    {
        Controller.height=orginalHeighControler;
        Controller.center=orginalControlerCenter;
        camera.ChangePositionCam(orginalHeighControler-kneelingHeigh);
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
            horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * acceleration * deltaTime, isKneeling ? kneelingSpeed : IsSprinting ? runSpeed : maxSpeed);
        }

        moveVelocity.x = horizontalVel.x;
        moveVelocity.z = horizontalVel.z;

        Controller.Move(moveVelocity * deltaTime);

        Velocity = (transform.position - previousPos) * Runner.Simulation.Config.TickRate;
        IsGrounded = Controller.isGrounded;
    }

    public void Rotate(float _rotationInput)
    {
        transform.Rotate(0, _rotationInput * Runner.DeltaTime * rotationSpeed, 0);
    }
}