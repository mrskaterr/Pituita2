using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class BlobStation : NetworkBehaviour
{
    [SerializeField] private LayerMask jarLayer;
    [SerializeField] private float radius = 1f;
    [SerializeField] private Vector3 offset = Vector3.zero;

    private void Update()
    {
        var res = Physics.OverlapSphere(transform.position + offset, radius, jarLayer);
        if (res.Length > 0)
        {
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i].GetComponent<ReferenceHold>() != null)
                {
                    res[i].GetComponent<ReferenceHold>().reference.GetComponent<CaptureHandler>().isCaptured = true;
                    res[i].GetComponent<ReferenceHold>().reference.GetComponent<CaptureHandler>().ChangeView();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, radius);
    }
}