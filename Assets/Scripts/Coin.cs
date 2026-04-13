using UnityEngine;

public class Coin : MonoBehaviour
{
    private AudioSource audioSource;
    private CoinManager manager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        manager = FindAnyObjectByType<CoinManager>();

        if (audioSource == null)
        {
            Debug.LogWarning($"Attention : Pas d'AudioSource sur {gameObject.name} !");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource != null && audioSource.clip != null)
            {
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            }

            if (manager != null)
            {
                manager.AddCoin();
            }

            gameObject.SetActive(false);
        }
    }
}