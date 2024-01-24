using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{   
    // [SerializeField] LayerMask acceptableLayers;
    // [Space]
    [SerializeField] AudioSource OpenSound;
    [SerializeField] AudioSource CloseSound;
    Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CloseSound.Stop();
        animator.SetBool("open", true);
        OpenSound.Play();
    }
    private void OnTriggerExit(Collider other)
    {
        OpenSound.Stop();
        animator.SetBool("open", false);
        CloseSound.Play();
    }
}