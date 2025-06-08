using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void GameOver(string message)
    {
        Debug.Log($"게임 오버: {message}");
        Time.timeScale = 0; // 게임 정지
        // 여기에 UI 표시 로직 추가
    }
}
