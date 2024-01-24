using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassHandler : MonoBehaviour
{
    [SerializeField] private Material compassMat;
    [SerializeField] private float offset = .52f;
    [SerializeField] private float turnSpeed = .1f;
    [Space]
    [SerializeField] private AnimationCurve fadeCurve;
    [Space]
    [SerializeField] private RectTransform ping;
    [SerializeField] private Camera cam;

    private Quaternion goal;

    private void Start()
    {
        goal = Quaternion.Euler(new Vector3(0, 7.5f, 0));
    }

    private void Update()
    {
        float parentRot = transform.eulerAngles.y;
        compassMat.SetFloat("_Rotation", Mathf.Clamp01(parentRot / 360f) + offset);
        SetPingPos();
    }

    private void SetPingPos()
    {
        Vector3 viewPos = cam.WorldToViewportPoint(Vector3.zero);
        float X = viewPos.x * 1542.024f;
        ping.anchoredPosition = new Vector2(X, 0);
    }
}