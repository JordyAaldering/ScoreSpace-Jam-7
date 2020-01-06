#pragma warning disable 0649
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private FloatVariable score, highScore;
    
    [SerializeField] private UnityEvent OnGameOverEvent = new UnityEvent();
    public Action OnGameOverAction { get; set; } = delegate { };
    
    private bool gameOver;

    private float startX;
    private Transform playerTransform;

    private void Start()
    {
        score.value = 0f;
        if (Physics2D.gravity.y > 0f) Physics2D.gravity = -Physics2D.gravity;

        highScore.value = PlayerPrefs.GetFloat("HighScore", 0f);
        
        playerTransform = FindObjectOfType<PlayerController>().transform;
        startX = playerTransform.position.x;
    }

    private void Update()
    {
        Vector2 playerPos = playerTransform.position;
        if (playerPos.x < startX - 0.1f || Mathf.Abs(playerPos.y) > 4f)
            GameOver();

        if (gameOver && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (!gameOver)
        {
            score.value += Time.deltaTime * 10f;
            if (score.value > highScore.value)
                highScore.value = score.value;
        }
    }

    public void GameOver()
    {
        gameOver = true;
        
        score.value = 0f;
        PlayerPrefs.SetFloat("HighScore", highScore.value);
        
        OnGameOverEvent.Invoke();
    }
}
