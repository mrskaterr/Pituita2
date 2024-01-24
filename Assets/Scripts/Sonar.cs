using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Sonar : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private List<Transform> blobs = new List<Transform>();

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        foreach (var blob in FindObjectsOfType<Morph>())
        {
            blobs.Add(blob.transform);
        }
    }

    private void Update()
    {
        text.text = $"{(int) GetNearestBlob()} m";
    }

    private float GetNearestBlob()
    {
        float distance = float.MaxValue;
        for (int i = 0; i < blobs.Count; i++)
        {
            if (distance > Vector3.Distance(transform.position, blobs[i].position))
            {
                distance = Vector3.Distance(transform.position, blobs[i].position);
            }
        }
        return distance;
    }
}