using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Button _startButton;

    void Start()
    {
        // ��ư ������Ʈ ��������
        _startButton = GetComponent<Button>();
        // ��ư Ŭ�� �� GameStart() ȣ��
        _startButton.onClick.AddListener(GameStart);
    }

    void GameStart()
    {
        // SampleScene���� ��ȯ
        SceneManager.LoadScene("SampleScene");
    }
}
