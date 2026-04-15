using UnityEngine;
using System.Collections;

public class FinalSequenceManager : MonoBehaviour
{
    [Header("Configuration")]
    public string coinTag = "Coin";
    public Transform doorPivot;
    public Transform cornerPoint;
    public Transform danceFloorPoint;
    public float openAngle = 90f;
    public float moveSpeed = 3f;
    public float rotationSpeed = 10f;
    public float successAnimDuration = 2.5f;
    public float danceDuration = 5f;

    [Header("UI")]
    public GameObject coinAchievementUI;
    public float achievementDisplayTime = 3f;

    [Header("Références")]
    public RisingWater waterScript;
    public GameObject player;
    public GameFlowManager flowManager;

    private bool sequenceStarted = false;
    private Animator anim;
    private float startTime;

    private void Start()
    {
        startTime = Time.time; // On note l'heure de démarrage
        if (player != null) anim = player.GetComponentInChildren<Animator>();
        if (coinAchievementUI != null) coinAchievementUI.SetActive(false);
    }

    private void OnTriggerStay(Collider other) // Utiliser Stay est plus sûr que Enter au restart
    {
        // Sécurité : On ignore le trigger pendant les 2 premières secondes du jeu
        if (Time.time < startTime + 2f) return;

        if (other.CompareTag("Player") && !sequenceStarted)
        {
            int coinsLeft = GameObject.FindGameObjectsWithTag(coinTag).Length;
            if (coinsLeft == 0)
            {
                sequenceStarted = true;
                StartCoroutine(HandleFullSequence());
            }
        }
    }

    IEnumerator HandleFullSequence()
    {
        if (coinAchievementUI != null)
        {
            coinAchievementUI.SetActive(true);
            StartCoroutine(HideAchievementMessage());
        }

        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        player.transform.position = new Vector3(player.transform.position.x, cornerPoint.position.y, player.transform.position.z);

        Vector3 lookDir = (cornerPoint.position - player.transform.position).normalized;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
        {
            player.transform.rotation = Quaternion.LookRotation(lookDir);
        }

        if (anim != null) anim.Play("Success", -1, 0f);

        if (waterScript != null) waterScript.canRise = false;
        Quaternion targetDoorRot = Quaternion.Euler(0, openAngle, 0);

        StartCoroutine(OpenDoorOverTime(targetDoorRot));
        yield return new WaitForSeconds(successAnimDuration);

        if (anim != null) anim.Play("Run", -1, 0f);

        yield return StartCoroutine(MoveToPoint(cornerPoint.position));
        yield return StartCoroutine(MoveToPoint(danceFloorPoint.position));

        if (anim != null) anim.Play("Dance", -1, 0f);

        Quaternion faceCamera = Quaternion.Euler(0, 0, 0);
        float rotTime = 0;
        while (rotTime < 1f)
        {
            rotTime += Time.deltaTime * 4f;
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, faceCamera, rotTime);
            yield return null;
        }

        yield return new WaitForSeconds(danceDuration);

        if (coinAchievementUI != null) coinAchievementUI.SetActive(false);

        if (flowManager != null)
        {
            flowManager.ShowReplay();
        }
    }

    IEnumerator HideAchievementMessage()
    {
        yield return new WaitForSeconds(achievementDisplayTime);
        if (coinAchievementUI != null) coinAchievementUI.SetActive(false);
    }

    IEnumerator OpenDoorOverTime(Quaternion targetRot)
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            doorPivot.localRotation = Quaternion.Slerp(doorPivot.localRotation, targetRot, t);
            yield return null;
        }
        doorPivot.localRotation = targetRot;
    }

    IEnumerator MoveToPoint(Vector3 targetPoint)
    {
        Vector3 lockedTarget = new Vector3(targetPoint.x, targetPoint.y, targetPoint.z);
        float distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z),
                                          new Vector2(lockedTarget.x, lockedTarget.z));

        while (distance > 0.05f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, lockedTarget, moveSpeed * Time.deltaTime);
            Vector3 flatDirection = (lockedTarget - player.transform.position).normalized;
            flatDirection.y = 0;

            if (flatDirection != Vector3.zero)
            {
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(flatDirection), rotationSpeed * Time.deltaTime);
            }
            distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z),
                                        new Vector2(lockedTarget.x, lockedTarget.z));
            yield return null;
        }
    }
}