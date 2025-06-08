using UnityEngine;
using System.Collections;

public class Knight : ChessPiece
{
    [SerializeField] private int _health = 150;
    private float _lastAttackTime;

    protected override void Awake()
    {
        base.Awake();
        Health = _health;
        SetAttackStrategy(new MeleeAttack(2, 20));
    }

    public override void MoveTo(BoardTile targetTile)
    {
        if (CurrentTile != null) CurrentTile.pieceOnTile = null;
        CurrentTile = targetTile;
        targetTile.pieceOnTile = this;
        StartCoroutine(MoveArcRoutine(targetTile.transform.position, 0.5f, 1.5f));
        StartCoroutine(PostMoveAttackCheck());
    }

    // L자 이동 가능 범위 표시
    public override void ShowMoveOptions(BoardManager board)
    {
        int[,] moves = { { 2, 1 }, { 1, 2 }, { -1, 2 }, { -2, 1 }, { -2, -1 }, { -1, -2 }, { 1, -2 }, { 2, -1 } };

        for (int i = 0; i < moves.GetLength(0); i++)
        {
            int nx = CurrentTile.x + moves[i, 0];
            int ny = CurrentTile.y + moves[i, 1];

            if (IsValidTile(nx, ny, board))
                board.tiles[nx, ny].Highlight(true);
        }
    }

    private IEnumerator PostMoveAttackCheck()
    {
        yield return new WaitForSeconds(0.5f);
        PerformAttack();
    }

    private bool IsValidTile(int x, int y, BoardManager board)
    {
        return x >= 0 && x < board.boardSize && y >= 0 && y < board.boardSize;
    }
}
