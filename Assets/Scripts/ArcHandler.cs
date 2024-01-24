using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ArcHandler : MonoBehaviour
{
    [SerializeField] private VisualEffect vfx;
    [SerializeField] private Transform origin;
    [SerializeField] private Transform centerPointA, centerPointB;
    [SerializeField] private Transform bezierHandleA, bezierHandleB;
    [SerializeField] private Transform end;

    [Range(0f, 100f)]
    [SerializeField] private float percentA, percentB;

    [SerializeField] private float radiusA, radiusB;
    [SerializeField] private float rotateSpeedA, rotateSpeedB;

    private float angleA, angleB;

    public Transform Target
    {
        get => target;
        set
        {
            target = value;
            SetVFX();
        }
    }
    private Transform target;

    private void Update()
    {
        angleA += rotateSpeedA * Time.deltaTime;
        angleB += rotateSpeedB * Time.deltaTime;
        origin.LookAt(end);

        if (target == null)
        {
            return;
        }

        end.position = target.position;
        centerPointA.position = CalculatePointPosition(percentA);
        centerPointB.position = CalculatePointPosition(percentB);
        centerPointA.eulerAngles = origin.eulerAngles + angleA * Vector3.forward;
        centerPointB.eulerAngles = origin.eulerAngles + angleB * Vector3.forward;
        bezierHandleA.localPosition = radiusA * Vector3.up;
        bezierHandleB.localPosition = radiusB * Vector3.up;
    }

    private void SetVFX()
    {
        vfx.enabled = target != null;
        //StopAllCoroutines();
        //StartCoroutine(Toggle());

        //IEnumerator Toggle()
        //{
        //    bool p = target != null;
        //    if (p)
        //    {
        //        vfx.enabled = true;
        //        vfx.Play();
        //    }
        //    else
        //    {
        //        vfx.Stop();
        //        yield return new WaitForSeconds(3);
        //        vfx.enabled = false;
        //    }
        //}
    }

    private Vector3 CalculatePointPosition(float _percent)
    {
        
        return percentPosition();

        Vector3 percentPosition()
        {
            float distance = Mathf.Lerp(0, Vector3.Distance(origin.position, end.position), _percent * .01f);
            Vector3 dir = end.position - origin.position;
            dir.Normalize();
            return origin.position + (distance * dir);
        }
    }
}