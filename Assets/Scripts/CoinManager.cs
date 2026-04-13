using UnityEngine;
using TMPro; // Nécessaire pour le texte UI

public class CoinManager : MonoBehaviour
{
    public int score = 0;
    public int totalCoins;
    public TextMeshProUGUI scoreText;
    public GameObject messageVictoire;

    private void Start()
    {
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        UpdateUI();
        messageVictoire.SetActive(false);
    }

    public void AddCoin()
    {
        score++;
        UpdateUI();

        if (score >= totalCoins)
        {
            messageVictoire.SetActive(true);

            var tmp = messageVictoire.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = "Toutes les pièces ont été récupérées !";
            }
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Pièces : " + score + " / " + totalCoins;
    }

    public void ResetGame()
    {
        score = 0;
        UpdateUI();
        messageVictoire.SetActive(false);

        foreach (Coin coin in Resources.FindObjectsOfTypeAll<Coin>())
        {
            coin.gameObject.SetActive(true);
        }
    }
}