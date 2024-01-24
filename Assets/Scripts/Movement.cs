using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform Cam;
    [SerializeField] Transform FootPoint;
    [SerializeField] float MouseSensitivity;
    [SerializeField] float LookUpMax ;
    [SerializeField] float LookUpMin ;
    [SerializeField] float Speed;
    //[SerializeField]
    float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float JumpForce ;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] float RadiusOverlapSphere;
    [SerializeField] float CrouchSpeed;
   
    private Rigidbody _Rigidbody;
    private Vector3 _MoveInput;
    private Quaternion _MouseInput=Quaternion.Euler(Vector3.zero);
    private bool _Grounded = true;
    private bool _DoubleJump=false;
    private float _CameraX;
    private float _ScaleY=1;
    private float _rouchHeight;
    private float _Length;
    private float CrouchHeight;


     
    [SerializeField] float MaxDashTime;
    [SerializeField] float DashSpeed ;
    [SerializeField] float DashStoppingSpeed ;
    [SerializeField] float DashResetTime ;
    [SerializeField] float Lock;
    private float currentDashTime;
    private float currentDashResetTime;

    void Start()
    {
        currentDashTime = MaxDashTime;

        CrouchHeight = 0.5f * _ScaleY;
        _Length =  _ScaleY - CrouchHeight;
        walkSpeed=Speed;
        _Rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {   
        _CameraX = Mathf.Clamp(_CameraX-Input.GetAxis("Mouse Y") * MouseSensitivity,LookUpMin,LookUpMax);
        Cam.localRotation= Quaternion.Euler(_CameraX, Cam.localRotation.y, Cam.localRotation.z);

        _MouseInput =  Quaternion.Euler(0,Input.GetAxis("Mouse X") * MouseSensitivity,0);
        _MoveInput = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");

         //Dash
        if (Input.GetKey(KeyCode.Z) &&  currentDashResetTime>DashResetTime)
        {
            currentDashTime = 0.0f;
            currentDashResetTime= 0.0f;
        }
        if (currentDashTime < MaxDashTime)
        {
            Speed=DashSpeed;
            currentDashTime += DashStoppingSpeed;
        }
        else
        {
            Speed=walkSpeed;
            currentDashResetTime += DashStoppingSpeed;
        }

        //Sprint, kucanie

	    if(Input.GetKey(KeyCode.LeftShift)) 
        {
		    Speed = runSpeed;
	    }
        else if(Input.GetKey(KeyCode.LeftControl) || (Physics.Raycast( transform.position , Vector3.up, _Length)/*CanStand*/) ) 
        {
	        transform.localScale = new Vector3(1,CrouchHeight,1);
	        Speed = CrouchSpeed;
        }
        else
        {
            transform.localScale = new Vector3(1,_ScaleY,1);
	        Speed = walkSpeed;
        }

  
        ///JUMP
        if(_Grounded==false && Physics.OverlapSphere(FootPoint.position,RadiusOverlapSphere,GroundLayer).Length>0)
        {
            _Grounded = true;
            _DoubleJump=true;
        }
        if(Input.GetKeyDown(KeyCode.Space) && _Grounded)
        {
            Jump();
        }
        else if(Input.GetKeyDown(KeyCode.Space) && !_Grounded && _DoubleJump)
        {
            Jump();
            _DoubleJump=false;
        }
    }
    
    void FixedUpdate()
    {
        if(!_Grounded)
            Speed*=Lock;
        
        _Rigidbody.MovePosition(transform.position + _MoveInput.normalized * Time.fixedDeltaTime * Speed);
        _Rigidbody.MoveRotation(_Rigidbody.rotation * _MouseInput);
    } 

    private void Jump()
    {
        _Grounded=false;
        _Rigidbody.velocity = new Vector3(_Rigidbody.velocity.x, 0f, _Rigidbody.velocity.z);
        _Rigidbody.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }
}
