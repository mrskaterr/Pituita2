using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCanvas : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    
    private static GameObject loading;

    public static LoadingCanvas instance;

    private void Awake()
    {
        

        if(instance != null) { Destroy(gameObject); }
        else { instance = this; }

        loading = loadingScreen;

        DontDestroyOnLoad(gameObject);
    }

    public static void SetActive(bool _p)
    {
        loading.SetActive(_p);
    }
}