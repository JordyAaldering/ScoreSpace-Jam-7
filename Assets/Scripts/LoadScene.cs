using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private bool thisScene = false;
    [SerializeField] private int sceneIndex = 0;

    public void Load()
    {
        SceneManager.LoadScene(thisScene ? SceneManager.GetActiveScene().buildIndex : sceneIndex);
    }
}
