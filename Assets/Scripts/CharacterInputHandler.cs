using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    public bool canSneak = false;
    public bool canSprinting;
    [SerializeField] LocalCameraHandler cameraHandler;

    private Vector2 moveInput = Vector2.zero;
    private Vector2 viewInput = Vector2.zero;
    private bool jumpInput = false;
    private bool fireInput = false;//TODO: interact
    private float speedStep = 0;
    private bool sneakyInput = false;
    private bool dashInput= false;
    private bool sprintInput=false;
    private bool kneelingInput=false;
    private Vector3 sneakRot = Vector3.zero;
    private bool isHuman;

    private CharacterMovementHandler characterMovementHandler;
    private NetworkCharacterController controler;
    private void Awake()
    {
        
        if(gameObject.GetComponent<Morph>())
            isHuman=false;
        else
            isHuman=true;
        characterMovementHandler = GetComponent<CharacterMovementHandler>();
    }
    private void Start()
    {
        controler=GetComponent<NetworkCharacterController>();
        Cursor.lockState = CursorLockMode.Locked;//TOIMPROVE: Utils
        Cursor.visible = false;

    }

    private void Update()
    {
        if (!characterMovementHandler.Object.HasInputAuthority) { return; }

        viewInput.x = Input.GetAxis("Mouse X");
        viewInput.y = Input.GetAxis("Mouse Y") * -1;

        moveInput.x = Input.GetAxis("Horizontal");//TODO: new input system
        moveInput.y = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            jumpInput = true;
        }

        if (Input.GetButton("Fire1"))
        {
            fireInput = true;
        }

        if (Input.GetButtonDown("Fire2") && canSneak)
        {
            sneakRot = cameraHandler.transform.forward;
            sneakyInput = true;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            sneakyInput = false;
        }
    
        if(!isHuman && Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashInput=true;
        }
        if(isHuman && canSprinting && Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            
		    sprintInput=true;
	    }
        if(isHuman && Input.GetKeyUp(KeyCode.LeftShift) || !canSprinting) 
        {
		    sprintInput=false;
	    }
        
        if(isHuman && Input.GetKeyDown(KeyCode.Z)) 
        {
		    kneelingInput=true;
            controler.Kneeling(cameraHandler);
	    }
        if(isHuman && Input.GetKeyUp(KeyCode.Z))
        {
		    kneelingInput=false;
            controler.Standing(cameraHandler);
	    }
        moveInput.y += speedStep;

        cameraHandler.SetViewInput(viewInput);
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        if (sneakyInput)
        {
            networkInputData.aimForwardVector = sneakRot;
        }
        else
        {
            networkInputData.aimForwardVector = cameraHandler.transform.forward;
        }

        networkInputData.movementInput = moveInput;

        networkInputData.isJumpPressed = jumpInput;

        networkInputData.isFirePressed = fireInput;

        networkInputData.isDashPressed = dashInput;

        networkInputData.isSprintPressed = sprintInput;

        networkInputData.isKneelingPressed = kneelingInput;
        
        jumpInput = false;
        fireInput = false;
        dashInput = false;
        return networkInputData;
    }
}