#pragma warning disable 0649
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float value = 1f;
    [SerializeField] private ParticleSystem particle;
    
    [SerializeField] private FloatVariable score;
    [SerializeField] private FloatVariable highScore;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            score.value += value;
            if (score.value > highScore.value) highScore.value = score.value;

            Transform t = transform;
            Instantiate(particle, t.position, Quaternion.identity, t.parent);
            Destroy(gameObject);
        }
    }
}
