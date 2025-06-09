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
        if (CurrentGold >= 10000)
        {
            GameManager.Instance.GameOver(true, "Reached Gold Goal!");
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
        goldText.text = $"Gold: {CurrentGold}";
    }
}
