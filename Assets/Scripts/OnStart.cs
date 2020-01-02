using UnityEngine;
using UnityEngine.Events;

public class OnStart : MonoBehaviour
{
    [SerializeField] private UnityEvent OnStartEvent = new UnityEvent();

    private void Start()
    {
        OnStartEvent.Invoke();
    }
}
