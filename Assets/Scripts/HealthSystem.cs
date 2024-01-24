using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class HealthSystem : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnHPChanged))]
    [SerializeField] private int HP { get; set; }

    [Networked(OnChanged = nameof(OnStateChanged))]
    public bool isDead { get; set; }

    private bool isInitialized = false;

    [SerializeField] private int startingHP = 100;

    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject jar;
    [SerializeField] private GameObject body;
    List<Coroutine>  coroutines;

    private PlayerHUD HUD;
    private CaptureHandler captureHandler;

    private void Awake()
    {
        HUD = GetComponent<PlayerHUD>();
        captureHandler = GetComponent<CaptureHandler>();
    }

    private void Start()
    {
        coroutines=new List<Coroutine>();
        HP = startingHP;
        isDead = false;

        isInitialized = true;
    }

    private void Update()
    {
        jar.SetActive(isDead && !captureHandler.isCarried);
    }

    private void OnHit()
    {
        HUD.PlayHitAnimation();
    }
    IEnumerator HealthRegeneration(float FirstWaiting,float ForWainting)
    {
        yield return new WaitForSeconds(FirstWaiting);
        while(!isDead && HP < startingHP)
            {
                HP += 1;
                yield return new WaitForSeconds(ForWainting);
            }
    }
    [Rpc]//TOIMPROVE: source & target
    public void RPC_OnTakeDamage(int _dmg)
    {
        HP -= _dmg;
        for(int i=0;i< coroutines.Count;i++)
            StopCoroutine(coroutines[i]);
        
        coroutines.Clear();
        
        coroutines.Add( StartCoroutine(HealthRegeneration(10f,.1f)));

        Debug.Log($"{transform.name} took damage got {HP} left");
        if (HP <= 0)
        {
            Debug.Log($"{transform.name} died");
            isDead = true;
        }
        if (isDead)
        {
            GetComponent<CharacterController>().enabled = false;
            
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

        if (newHP < oldHP) { _changed.Behaviour.OnHPReduced(); }
    }

    private void OnHPReduced()
    {
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
        GetComponent<CharacterController>().enabled = true;
        body.SetActive(true);
        HUD.ToggleCrosshair(true);
        //HUD.ToggleOnHitImage(false);
    }
}