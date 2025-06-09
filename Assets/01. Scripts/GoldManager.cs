using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;
    public int CurrentGold = 0;
    public TextMeshProUGUI goldText;

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
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        CurrentGold += amount;
        if (CurrentGold >= 2000)
        {
            GameManager.Instance.GameOver(true, "목표치 달성!");
        }
        UpdateUI();
    }

    public bool SpendGold(int amount)
    {
        if (CurrentGold >= amount)
        {
            CurrentGold -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    void UpdateUI()
    {
        goldText.text = $"돈: {CurrentGold}";
    }
}
