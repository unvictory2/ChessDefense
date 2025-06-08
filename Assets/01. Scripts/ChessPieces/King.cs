using UnityEngine;
using TMPro;

public class King : ChessPiece
{
    [Header("King Settings")]
    [SerializeField] private int _maxHealth = 100;
    public TextMeshProUGUI healthText;

    protected override void Awake()
    {
        base.Awake();
        Health = _maxHealth;
        UpdateHealthUI();
    }

    // King은 이동하지 않으므로 MoveTo 오버라이드
    public override void MoveTo(BoardTile targetTile)
    {
        // King은 이동하지 않음 - 아무것도 하지 않음
        Debug.Log("King cannot move!");
    }

    // King은 이동 옵션을 보여주지 않음 (virtual이므로 구현 안 해도 됨)
    public override void ShowMoveOptions(BoardManager board)
    {
        // King은 이동하지 않으므로 아무것도 하지 않음
    }

    // 피해 받을 때 UI 업데이트 및 게임오버 처리
    public new void TakeDamage(int damage)
    {
        Health -= damage;
        UpdateHealthUI();

        if (Health <= 0)
        {
            GameOver();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = $"King Health: {Health}";
    }

    private void GameOver()
    {
        Debug.Log("Game Over! King has been defeated!");
        Time.timeScale = 0; // 게임 일시정지
        // 여기에 게임오버 UI 표시 로직 추가
    }
}
