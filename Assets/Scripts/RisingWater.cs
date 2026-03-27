using UnityEngine;
using TMPro;
using System.Collections;

public class RisingWater : MonoBehaviour
{
    [Header("Réglages")]
    public float growthSpeed = 0.5f;
    public float startDelay = 3f;

    [Header("UI & Positions")]
    public TextMeshProUGUI countdownText;
    public Transform spawnPoint;
    public GameObject player;

    private bool canRise = false;
    private Vector3 initialScale;
    private Vector3 initialPosition;
    private Coroutine currentRoutine;

    void Start()
    {
        initialScale = transform.localScale;
        initialPosition = transform.position;
        RestartSequence();
    }

    public void RestartSequence()
    {
        canRise = false;
        transform.localScale = initialScale;
        transform.position = initialPosition;

        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        // 1. PHASE D'ALERTE UNIQUEMENT
        if (countdownText != null)
        {
            countdownText.text = "ATTENTION !\nL'EAU VA MONTER";
            countdownText.fontSize = 80; // Ajustez cette taille selon vos réglages de Width/Height
        }

        // On attend le temps défini dans startDelay (ex: 3 secondes)
        yield return new WaitForSeconds(startDelay);

        // 2. EFFACER LE TEXTE
        if (countdownText != null)
        {
            countdownText.text = "";
        }

        // L'eau commence à monter immédiatement après
        canRise = true;
    }

    void Update()
    {
        if (canRise)
        {
            transform.localScale += new Vector3(0, growthSpeed * Time.deltaTime, 0);
            transform.position += new Vector3(0, (growthSpeed * Time.deltaTime) / 2, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.transform.position = spawnPoint.position;
            RestartSequence();
        }
    }
}