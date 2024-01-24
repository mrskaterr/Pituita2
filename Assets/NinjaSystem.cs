using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI; 
using TMPro;  
public class NinjaSystem : MonoBehaviour 
{ 
    [SerializeField] TMP_Text NinjaAmount; 
    [SerializeField] Image NinjaBarFill; 
    [SerializeField] float NinjaSpeed=10f; 
    [SerializeField] int NinjaMaxAmount=2; 
    [SerializeField] float maxNinjaTime=1f; 
    [SerializeField] float NinjaResetTime=100f; 
    private NetworkCharacterController controller;  
    private AudioHandler audioHandler;
    private int NinjaCurrentAmount; 
    private float currentNinjaTime; 
    private float currentNinjaResetTime;
    bool activeNinjaMode=false; 

    void Start() 
    {
        NinjaCurrentAmount=NinjaMaxAmount; 
        audioHandler=GetComponent<AudioHandler>();
        controller=GetComponent<NetworkCharacterController>(); 
        currentNinjaTime = maxNinjaTime; 
        NinjaAmount.text=(NinjaMaxAmount-NinjaCurrentAmount).ToString(); 
        currentNinjaResetTime=NinjaResetTime; 
    } 
    public void NinjaMode(bool start) 
    { 
        if (start && !activeNinjaMode  &&  NinjaCurrentAmount<NinjaMaxAmount) 
        { 
            
            StartCoroutine(WaitAndDisable(maxNinjaTime));
        }
        else 
        { 
            currentNinjaResetTime += Time.fixedDeltaTime; 
            NinjaBarFill.fillAmount = currentNinjaResetTime/NinjaResetTime; 
            if(currentNinjaResetTime>=NinjaResetTime) 
            { 
                NinjaCurrentAmount=0; 
                NinjaAmount.text=(NinjaMaxAmount-NinjaCurrentAmount).ToString(); 
            }        
        } 
    }
    private IEnumerator WaitAndDisable(float time)
    {
        activeNinjaMode=true;
        audioHandler.NinjaMode(false);
        NinjaCurrentAmount++; 
        currentNinjaTime = 0.0f; 
        currentNinjaResetTime= 0.0f; 
        NinjaAmount.text=(NinjaMaxAmount-NinjaCurrentAmount).ToString(); 
        yield return new WaitForSeconds(time);
        audioHandler.NinjaMode(true);
        activeNinjaMode=false;
    }
} 
