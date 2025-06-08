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
        Debug.Log($"���� ����: {message}");
        Time.timeScale = 0; // ���� ����
        // ���⿡ UI ǥ�� ���� �߰�
    }
}
