using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPoint : MonoBehaviour
{
    [SerializeField] private GameObject gunPoint;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        gunPoint.SetActive(true);
    }
}