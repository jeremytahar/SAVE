using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public Transform doorPivot;
    public float openAngle = 90f;
    public float openSpeed = 2f;

    private bool isPlayerNearby = false;
    private bool isOpening = false;
    private Quaternion targetRotation;

    void Start()
    {
        // On définit la rotation cible (fermée au début)
        targetRotation = doorPivot.localRotation;
    }

    void Update()
    {
        // Si le joueur est là et appuie sur E (ou ta touche)
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            StartOpening();
        }

        // Animation fluide de l'ouverture
        if (isOpening)
        {
            doorPivot.localRotation = Quaternion.Slerp(
                doorPivot.localRotation,
                targetRotation,
                Time.deltaTime * openSpeed
            );
        }
    }

    public void StartOpening()
    {
        targetRotation = Quaternion.Euler(0, openAngle, 0);
        isOpening = true;

        // C'est ici qu'on ajoutera plus tard :
        // 1. Stop l'eau
        // 2. Change de caméra Syphon
        // 3. Lance la danse
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = false;
    }
}