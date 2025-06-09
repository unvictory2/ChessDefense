using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI finalGoldText;
    public Button retryButton;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameOverPanel.SetActive(false);
        retryButton.onClick.AddListener(ReloadScene);
    }

    public void GameOver(bool isVictory, string message)
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);

        resultText.text = isVictory ? "Won!" : "Lost!";
        finalGoldText.text = $"{message}\nGold: {GoldManager.Instance.CurrentGold}";
    }

    void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
