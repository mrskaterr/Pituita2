using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArc : MonoBehaviour
{
    [SerializeField] private Transform testTarget;
    private bool tmp;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            tmp = !tmp;
            GetComponent<ArcHandler>().Target = tmp ? testTarget : null;
        }
    }
}