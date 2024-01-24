using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.Events;

public class WeaponHandler : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnFireChanged))]
    public bool isFiring { get; set; }

    [SerializeField] private ArcHandler fireVFX;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject unmorphVFX;
    [SerializeField] private Transform aimPoint;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private GameObject hitMarker;
    [SerializeField] private GunMode gunMode;
    private bool isGadgetActive;
    [SerializeField] private UnityEvent gadgetAction;

    [SerializeField] private GameObject[] gun;
    [SerializeField] private GameObject gadget;
    Vector3 aim;
    [SerializeField] private int damage = 1;

    private float lastTimeFired = 0;

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData _networkInputData))
        {
            if(_networkInputData.isFirePressed && isGadgetActive)
            {
                gadgetAction?.Invoke();
            }
            if (_networkInputData.isFirePressed && gunMode.fireMode)
            {
                Fire(_networkInputData.aimForwardVector);
            }
            if (_networkInputData.isFirePressed && !gunMode.fireMode)
            {
                UnMorph(_networkInputData.aimForwardVector);
            }
            aim = _networkInputData.aimForwardVector;
        }
    }

    private void Update()
    {
        if (gadget != null && Input.GetAxis("Mouse ScrollWheel") != 0) { RPC_SwapGadget(); }
        if (Object.HasInputAuthority && Input.GetMouseButton(0) && !isGadgetActive)
        {
            if (gunMode.fireMode)
            {
                fireVFX.Target = target;
            }
            else
            {
                unmorphVFX.SetActive(false);
                RaycastHit[] tmp = Physics.RaycastAll(aimPoint.position, aim, 100);
                if (tmp.Length > 0)
                {
                    unmorphVFX.SetActive(true);
                    unmorphVFX.transform.position = tmp[0].point;
                }
            }
        }
        else {  fireVFX.Target = null;}
    }

    private void UnMorph(Vector3 _aimForwardVector)
    {
        if(Time.time - lastTimeFired < .15f)//TODO: MN
        {
            return;
        }
        Runner.LagCompensation.Raycast(aimPoint.position, _aimForwardVector, 100, Object.InputAuthority, out var hitInfo, targetLayerMask, HitOptions.IncludePhysX); //TODO: MN

        float hitDistance = 100;

        if(hitInfo.Distance > 0) { hitDistance = hitInfo.Distance; }

        if(hitInfo.Hitbox != null)
        {
            Debug.Log($"{Time.time} {transform.name} hit hitbox {hitInfo.Hitbox.transform.root.name}");

            hitInfo.Hitbox.transform.root.GetComponent<Morph>().RPC_UnMorph();
        }
    }
    private void Fire(Vector3 _aimForwardVector)
    {
        if(Time.time - lastTimeFired < .05f)//TODO: MN
        {
            return;
        }

        //StartCoroutine(FireFX());

        Runner.LagCompensation.Raycast(aimPoint.position, _aimForwardVector, 100, Object.InputAuthority, out var hitInfo, targetLayerMask, HitOptions.IncludePhysX); //TODO: MN

        float hitDistance = 100;
        bool isHitOtherPlayer = false;

        if(hitInfo.Distance > 0) { hitDistance = hitInfo.Distance; }

        if(hitInfo.Hitbox != null)
        {
            Debug.Log($"{Time.time} {transform.name} hit hitbox {hitInfo.Hitbox.transform.root.name}");

            if (Object.HasStateAuthority && hitInfo.Hitbox.transform.root.GetComponent<Morph>().index==-1)
            {
                hitInfo.Hitbox.transform.root.GetComponent<HealthSystem>().RPC_OnTakeDamage(damage);
                StartCoroutine(HitFX());
            }
            isHitOtherPlayer = true;
        }
        else if(hitInfo.Collider != null)
        {
            Debug.Log($"{Time.time} {transform.name} hit PhysX collider {hitInfo.Collider.transform.name}");
        }

        if (isHitOtherPlayer)
        {
            Debug.DrawRay(aimPoint.position, _aimForwardVector * hitDistance, Color.red, 1);
        }
        else
        {
            Debug.DrawRay(aimPoint.position, _aimForwardVector * hitDistance, Color.green, 1);
        }

        lastTimeFired = Time.time;
    }

    private IEnumerator FireFX()
    {
        isFiring = true;
        fireVFX.Target = target;
        yield return new WaitForSeconds(.09f);//TOIMPROVE: define this
        fireVFX.Target = null;
        isFiring = false;
    }

    private IEnumerator HitFX()
    {
        hitMarker.SetActive(true);
        yield return new WaitForSeconds(.2f);
        hitMarker.SetActive(false);
    }

    private static void OnFireChanged(Changed<WeaponHandler> _changed)
    {
        bool isFiringCurrent = _changed.Behaviour.isFiring;

        _changed.LoadOld();

        bool isFiringOld = _changed.Behaviour.isFiring;

        if (isFiringCurrent && !isFiringOld)
        {
            _changed.Behaviour.OnFireRemote();
        }
    }

    private void OnFireRemote()
    {
        if (!Object.HasInputAuthority)
        {
            //fireParticleSystem.Play();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SwapGadget()
    {
        if (isGadgetActive)
        {
            gadget.SetActive(false);
            foreach (GameObject part in gun)
            {
                part.SetActive(true);
            }
        }
        else
        {
            gadget.SetActive(true);
            foreach (GameObject part in gun)
            {
                part.SetActive(false);
            }
        }
        isGadgetActive = !isGadgetActive;
    }
}