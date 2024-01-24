using UnityEngine;
using UnityEngine.Events;

public class ActivityEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnable;
    [SerializeField] private UnityEvent onDisable;

    private void OnEnable()
    {
        onEnable.Invoke();
    }
    private void OnDisable()
    {
        onDisable.Invoke();
    }
}