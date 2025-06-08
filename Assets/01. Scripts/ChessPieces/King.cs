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

    // King�� �̵����� �����Ƿ� MoveTo �������̵�
    public override void MoveTo(BoardTile targetTile)
    {
        // King�� �̵����� ���� - �ƹ��͵� ���� ����
        Debug.Log("King cannot move!");
    }

    // King�� �̵� �ɼ��� �������� ���� (virtual�̹Ƿ� ���� �� �ص� ��)
    public override void ShowMoveOptions(BoardManager board)
    {
        // King�� �̵����� �����Ƿ� �ƹ��͵� ���� ����
    }

    // ���� ���� �� UI ������Ʈ �� ���ӿ��� ó��
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
        Time.timeScale = 0; // ���� �Ͻ�����
        // ���⿡ ���ӿ��� UI ǥ�� ���� �߰�
    }
}
