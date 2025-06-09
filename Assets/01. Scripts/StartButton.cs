using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Button _startButton;

    void Start()
    {
        // 버튼 컴포넌트 가져오기
        _startButton = GetComponent<Button>();
        // 버튼 클릭 시 GameStart() 호출
        _startButton.onClick.AddListener(GameStart);
    }

    void GameStart()
    {
        // SampleScene으로 전환
        SceneManager.LoadScene("SampleScene");
    }
}
