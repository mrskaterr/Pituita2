using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class HealthSystem : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnHPChanged))]
    private int HP { get; set; }
    [HideInInspector]
    [Networked(OnChanged = nameof(OnStateChanged))]
    public bool isDead { get; set; }

    private bool isInitialized = false;

    [SerializeField] private int startingHP = 100;

    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject jar;
    [SerializeField] private GameObject body;
    [SerializeField] float timeToRegeneration=5f;
    [SerializeField] float timeToStepRegeneration=0.1f;
    private CharacterController controller;
    private NetworkCharacterController networkController;
    private PlayerHUD HUD;
    private CaptureHandler captureHandler;
    private List<Coroutine>  coroutines;
    //private bool isInitialized = false;

    private void Awake()
    {
        networkController=GetComponent<NetworkCharacterController>();
        controller = GetComponent<CharacterController>();
        HUD = GetComponent<PlayerHUD>();
        captureHandler = GetComponent<CaptureHandler>();
    }

    private void Start()
    {

        coroutines=new List<Coroutine>();
        HP = startingHP;
        isDead = false;
        //isInitialized = true;
    }

    private void Update()
    {
        
        jar.SetActive(isDead && !captureHandler.isCarried);
    }
    
    private IEnumerator OnHit()
    {
        yield return new WaitForSeconds(2f);;
        networkController.MaxSpeed(true);
        HUD.PlayHitAnimation();
    }
    IEnumerator HealthRegeneration()
    {
        yield return new WaitForSeconds(timeToRegeneration);
        while(!isDead && HP < startingHP)
        {
            HP ++;
            yield return new WaitForSeconds(timeToStepRegeneration);
        }
    }
    [Rpc]//TOIMPROVE: source & target
    public void RPC_OnTakeDamage(int _dmg)
    {
        HP -= _dmg;
        for(int i=0;i< coroutines.Count;i++)
            StopCoroutine(coroutines[i]);
        
        coroutines.Clear();
        
        coroutines.Add( StartCoroutine(HealthRegeneration()));

//        Debug.Log($"{transform.name} took damage got {HP} left");
        if (HP <= 0)
        {
            Debug.Log($"{transform.name} died");
            isDead = true;
        }
        if (isDead)
        {
            controller.enabled = false;
            body.SetActive(false);
            captureHandler.isFree = false;
            HUD.ToggleMiniGame(true);
            return;
        }
    }
    private void Damage()
    {
        //yield return new WaitForSeconds(0.2f);
    }

    private static void OnHPChanged(Changed<HealthSystem> _changed)
    {
        _changed.Behaviour.DebugHP();
        int newHP = _changed.Behaviour.HP;
        _changed.LoadOld();
        int oldHP = _changed.Behaviour.HP;
        if (newHP < oldHP) 
        {
            _changed.Behaviour.OnHPReduced();
        }
    }

    private void OnHPReduced()
    {
        StartCoroutine(OnHit());
        if (!isInitialized) { return; }

        OnHit();
    }

    private void DebugHP() 
    { 
        healthBar.fillAmount = (float)HP/(float)startingHP;
        healthBar.transform.parent.gameObject.SetActive(healthBar.fillAmount < 1f && healthBar.fillAmount > 0f);
    }

    private static void OnStateChanged(Changed<HealthSystem> _changed)
    {

    }

    public void Restore()
    {
        HP = startingHP;
        isDead = false;
        controller.enabled = true;
        body.SetActive(true);
        HUD.ToggleCrosshair(true);
        //HUD.ToggleOnHitImage(false);
    }
}