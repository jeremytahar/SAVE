using UnityEngine;
using TMPro;
using System.Collections;

public class RisingWater : MonoBehaviour
{
    public float growthSpeed = 0.5f;
    public int countdownSeconds = 3;

    public TextMeshProUGUI countdownText;
    public Transform spawnPoint;
    public GameObject player;

    public Transform waterSurface;

    private bool canRise = false;
    private Vector3 initialScale;
    private Vector3 initialPosition;
    private Vector3 initialSurfacePosition;
    private Coroutine currentRoutine;

    void Start()
    {
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
                yield return new WaitForSeconds(1f);
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
            transform.localScale += new Vector3(0, growthSpeed * Time.deltaTime, 0);
            transform.position += new Vector3(0, (growthSpeed * Time.deltaTime) / 2, 0);

            if (waterSurface != null)
            {
                waterSurface.position += new Vector3(0, growthSpeed * Time.deltaTime, 0);
            }
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