using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI; 
using TMPro;  
public class DashSystem : MonoBehaviour 
{ 
    [SerializeField] TMP_Text dashAmount; 
    [SerializeField] Image dashBarFill; 
    [SerializeField] float dashSpeed=10f; 
    [SerializeField] int dashMaxAmount=2; 
    [SerializeField] float maxDashTime=1f; 
    [SerializeField] float dashResetTime=100f; 
    private NetworkCharacterController controller;  
    private int dashCurrentAmount=0; 
    private float currentDashTime; 
    private float currentDashResetTime; 

    void Start() 
    { 
        controller=GetComponent<NetworkCharacterController>(); 
        currentDashTime = maxDashTime; 
        dashAmount.text=(dashMaxAmount-dashCurrentAmount).ToString(); 
        currentDashResetTime=dashResetTime; 
    } 
    public void Dash(bool startDashing) 
    { 
        if (startDashing &&  dashCurrentAmount<dashMaxAmount) 
        { 
            dashCurrentAmount++; 
            currentDashTime = 0.0f; 
            currentDashResetTime= 0.0f; 
            dashAmount.text=(dashMaxAmount-dashCurrentAmount).ToString(); 
            startDashing=false; 
        } 
        if (currentDashTime < maxDashTime) 
        { 
            controller.maxSpeed=dashSpeed; 
            currentDashResetTime=0; 
            dashBarFill.fillAmount = currentDashResetTime/dashResetTime; 
            currentDashTime += Time.fixedDeltaTime; 
        } 
        else 
        { 
            controller.maxSpeed=controller.walkSpeed; 
            currentDashResetTime += Time.fixedDeltaTime; 
             
            dashBarFill.fillAmount = currentDashResetTime/dashResetTime; 
            if(currentDashResetTime>=dashResetTime) 
            { 
                dashCurrentAmount=0; 
                dashAmount.text=(dashMaxAmount-dashCurrentAmount).ToString(); 
            } 
                 
        } 
    } 
} 
