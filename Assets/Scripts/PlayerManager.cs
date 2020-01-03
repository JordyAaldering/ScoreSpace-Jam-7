using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private UnityEvent OnGameOverEvent = new UnityEvent();
    private bool gameOver;

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
        {
            gameOver = true;
            OnGameOverEvent.Invoke();
        }

        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            if (Physics2D.gravity.y > 0f) Physics2D.gravity = -Physics2D.gravity;
            SceneManager.LoadScene(0);
        }
    }
}
