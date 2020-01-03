using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private UnityEvent OnGameOverEvent = new UnityEvent();

    private float startX;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
        startX = playerTransform.position.x;
    }

    private void Update()
    {
        if (playerTransform.position.x < startX - 0.1f)
            OnGameOverEvent.Invoke();
    }
}
