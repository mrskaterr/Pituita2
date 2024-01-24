using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI; 
 
public class SprintSystem : MonoBehaviour 
{ 
    [SerializeField] Image staminaBarFill;
    [SerializeField] float maxStamina = 5f;
    [SerializeField] float runSpeed = 20f;  
    private CharacterInputHandler inputHandler;
    private NetworkCharacterController controller;
    private float cunrrentStamina=0;//=maxStamina;
    private float normalSpeed; 
    void Start() 
    { 
        controller = GetComponent<NetworkCharacterController>(); 
        inputHandler = GetComponent<CharacterInputHandler>(); 
        normalSpeed = controller.maxSpeed;
    } 
    public void Sprint() 
    { 
        if(controller.IsSprinting && cunrrentStamina>0f) 
        { 
            controller.maxSpeed=runSpeed;
            cunrrentStamina-=Time.deltaTime; 
            if(cunrrentStamina<=0f) 
                inputHandler.canSprinting=false; 
        } 
        else if(!controller.IsSprinting && cunrrentStamina<=maxStamina) 
        { 
            controller.maxSpeed=normalSpeed;
            cunrrentStamina+=Time.deltaTime; 
            if(cunrrentStamina>=maxStamina) 
                inputHandler.canSprinting=true; 
        }
        staminaBarFill.fillAmount=cunrrentStamina/maxStamina; 
    } 
} 