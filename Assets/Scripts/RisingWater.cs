using UnityEngine;
using TMPro;
using System.Collections;

public class RisingWater : MonoBehaviour
{
    [Header("Settings")]
    public float growthSpeed = 0.5f;
    public int countdownSeconds = 3;

    [Header("References")]
    public TextMeshProUGUI countdownText;
    public Transform spawnPoint;
    public GameObject player;
    public Transform waterSurface;

    public bool canRise = false;
    private Vector3 initialScale;
    private Vector3 initialPosition;
    private Vector3 initialSurfacePosition;
    private Coroutine currentRoutine;

    private WaitForSeconds oneSecondWait;
    private CoinManager coinManager;

    void Start()
    {
        oneSecondWait = new WaitForSeconds(1f);
        coinManager = FindAnyObjectByType<CoinManager>();

        initialScale = transform.localScale;
        initialPosition = transform.position;

        if (waterSurface != null)
        {
            initialSurfacePosition = waterSurface.position;
        }

        RestartSequence();
    }

    public void RestartSequence()
    {
        canRise = false;
        transform.localScale = initialScale;
        transform.position = initialPosition;

        if (waterSurface != null)
        {
            waterSurface.position = initialSurfacePosition;
        }

        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        if (countdownText != null)
        {
            for (int i = countdownSeconds; i > 0; i--)
            {
                countdownText.text = $"ATTENTION !\nL'EAU VA MONTER\n{i}";
                yield return oneSecondWait;
            }
            countdownText.text = "";
        }
        else
        {
            yield return new WaitForSeconds(countdownSeconds);
        }

        canRise = true;
    }

    void Update()
    {
        if (canRise)
        {
            float translation = growthSpeed * Time.deltaTime;

            transform.localScale += new Vector3(0, translation, 0);
            transform.position += new Vector3(0, translation / 2f, 0);

            if (waterSurface != null)
            {
                waterSurface.position += new Vector3(0, translation, 0);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player != null && spawnPoint != null)
            {
                player.transform.position = spawnPoint.position;
            }

            RestartSequence();

            if (coinManager != null)
            {
                coinManager.ResetGame();
            }
        }
    }
}