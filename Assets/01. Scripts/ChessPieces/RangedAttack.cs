using UnityEngine;

public class RangedAttack : IAttackStrategy
{
    private float _attackRange;
    private int _damage;
    private GameObject _projectilePrefab;

    public RangedAttack(float range, int damage, GameObject projectile)
    {
        _attackRange = range;
        _damage = damage;
        _projectilePrefab = projectile;
    }

    public void Attack(ChessPiece piece)
    {
        if (_projectilePrefab == null) return;
        GameObject projectile = Object.Instantiate(_projectilePrefab, piece.transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Initialize(_damage, LayerMask.GetMask("Enemy"), Vector3.forward); // ������ ���� Ÿ�ٿ� ���� ���� �ʿ�
    }

    public void ShowAttackRange(ChessPiece piece)
    {
        int x = piece.CurrentTile.x;
        int y = piece.CurrentTile.y;

        for (int i = 1; i <= _attackRange; i++)
        {
            if (y + i < BoardManager.Instance.boardSize)
            {
                BoardTile tile = BoardManager.Instance.tiles[x, y + i];
                if (tile.pieceOnTile == null || tile.pieceOnTile is Enemy)
                    tile.Highlight(true);
            }
        }
    }

    public float GetAttackRange() => _attackRange;
    public float GetAttackRate() => 1f; // ���� ��, �ʿ�� ����
}
