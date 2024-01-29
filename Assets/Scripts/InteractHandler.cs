using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractHandler : MonoBehaviour
{
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] Transform rayOriginPoint;
    [SerializeField] private float range = 3;
    [SerializeField] private GameObject indicator;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private Camera tpsCam;
    [SerializeField] private LocalCameraHandler cameraHandler;



    [Space]
    private NetworkPlayer networkPlayer;
    private Morph morph;
    private CarryHandler carryHandler;
    private IInteractable interactable;
    private InteractableHold interactable2;

    private Vector3 screenCenter = new Vector3(.5f, .5f, 0);

    private PlayerHUD playerHUD;

    private void Awake()
    {
        morph = GetComponent<Morph>();
        networkPlayer = GetComponent<NetworkPlayer>();
        carryHandler = GetComponent<CarryHandler>();
        playerHUD = GetComponent<PlayerHUD>();
    }

    private void Update()
    {
        if (!networkPlayer.isRemote)
        {
            Look4Interaction();
            if (interactable2 != null)
            {
                indicator.SetActive(true);//TODO: Set other indicator
                if (Input.GetMouseButtonDown(1))
                {
                    interactable2.StartInteract(gameObject);
                    playerHUD.InitInteract(interactable2.desc, interactable2.percent);
                }
                else if (Input.GetMouseButton(1))
                {
                    playerHUD.SetInteractPercent(interactable2.percent);
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    interactable2.StopInteract(gameObject);
                    playerHUD.StopInteract();
                }
            }
            else if (interactable != null)
            {
                indicator.SetActive(true);

                if (Input.GetMouseButtonDown(1))
                {
                    interactable.Interact(gameObject);
                }

            }
            else
            {
                indicator.SetActive(false);
                if (Input.GetMouseButtonDown(1))
                {
                    if (RPCManager.Local.IsHuman())
                        carryHandler?.RPC_Leave();//carryHandler.PutDown();
                    else
                        morph.index = -1;
                }
            } 
        }
    }

    private void Look4Interaction()
    {
        Ray ray;
        Camera cam = fpsCam;
        if (!cameraHandler.fps)
        {
            cam = tpsCam;
            ray = new Ray(rayOriginPoint.position, cam.transform.forward);
        }
        else
        {
            ray = cam.ViewportPointToRay(screenCenter);
        }
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range, interactableMask))
        {
            interactable = hit.transform.GetComponent<IInteractable>();
            interactable2 = hit.transform.GetComponent<InteractableHold>();
        }
        else
        {
            interactable2?.StopInteract(gameObject);
            playerHUD.StopInteract();
            interactable = null;
            interactable2 = null;
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(new Ray(rayOriginPoint.position, tpsCam.transform.forward));
    //}
}

interface IInteractable//TOIMPROVE: change 4 virtual void
{
    void Interact(GameObject @object);
}

interface IInteractableHold
{
    void StartInteract(GameObject @object);
    void StopInteract(GameObject @object);
}