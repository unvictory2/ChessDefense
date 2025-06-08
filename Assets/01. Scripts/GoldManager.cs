using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;
    public int CurrentGold = 5555;
    public TextMeshProUGUI goldText;

    void Awake()
    {
        Instance = this;
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        CurrentGold += amount;
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
