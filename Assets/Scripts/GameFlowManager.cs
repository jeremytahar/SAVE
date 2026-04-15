using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject cinematicGroup;
    public GameObject menuGroup;
    public GameObject endGroup;
    public PlayerController playerController;
    public RisingWater waterScript;
    public GameObject projectorBlackScreen;

    // --- NOUVELLE VARIABLE ---
    public GameObject startingBlackScreen;

    private bool isWaitingForVideo = true;
    private bool isWaitingToStart = false;
    private bool isGameFinished = false;
    private Rigidbody playerRb;

    private void Awake()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
            playerRb = playerController.GetComponent<Rigidbody>();
            if (playerRb != null) playerRb.isKinematic = true;
        }

        if (waterScript != null) waterScript.enabled = false;

        if (videoPlayer != null)
        {
            videoPlayer.Prepare(); // On le garde pour précharger la vidéo en mémoire
        }
    }

    private void Start()
    {
        cinematicGroup.SetActive(true);
        menuGroup.SetActive(false);
        endGroup.SetActive(false);

        if (projectorBlackScreen != null) projectorBlackScreen.SetActive(true);

        // --- ON ACTIVE L'ÉCRAN NOIR AU DÉMARRAGE ---
        if (startingBlackScreen != null) startingBlackScreen.SetActive(true);

        videoPlayer.loopPointReached += EndVideo;
    }

    private void EndVideo(VideoPlayer vp)
    {
        cinematicGroup.SetActive(false);
        menuGroup.SetActive(true);
        isWaitingToStart = true;
    }

    public void OSCPlayVideo(int value)
    {
        if (value > 0 && isWaitingForVideo)
        {
            isWaitingForVideo = false;

            // --- ON DÉSACTIVE L'ÉCRAN NOIR AU LANCEMENT ---
            if (startingBlackScreen != null) startingBlackScreen.SetActive(false);

            videoPlayer.Play();
        }
    }

    public void OSCStartGame(int value)
    {
        if (value > 0 && isWaitingToStart)
        {
            isWaitingToStart = false;
            menuGroup.SetActive(false);
            if (projectorBlackScreen != null) projectorBlackScreen.SetActive(false);

            playerController.enabled = true;
            if (playerRb != null) playerRb.isKinematic = false;
            if (waterScript != null) waterScript.enabled = true;
        }
    }

    public void OSCRestartGame(int value)
    {
        if (value > 0 && isGameFinished)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ShowReplay()
    {
        isGameFinished = true;
        endGroup.SetActive(true);
        if (projectorBlackScreen != null) projectorBlackScreen.SetActive(true);

        if (playerRb != null) playerRb.isKinematic = true;
        playerController.enabled = false;
    }
}