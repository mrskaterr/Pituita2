using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        GetComponent<BoxCollider>().enabled = true;
    }
}