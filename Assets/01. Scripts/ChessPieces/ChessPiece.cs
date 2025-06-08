using UnityEngine;
using System.Collections;

public abstract class ChessPiece : MonoBehaviour, IDamageable
{
    public BoardTile CurrentTile { get; set; }
    public int Team;
    public int Health = 100;
    protected IAttackStrategy _attackStrategy;

    public void SetAttackStrategy(IAttackStrategy strategy) => _attackStrategy = strategy;

    public void ShowAttackRange() => _attackStrategy?.ShowAttackRange(this);

    public void PerformAttack() => _attackStrategy?.Attack(this);

    // 추상 메서드 → virtual 메서드로 변경 (King은 구현 안 해도 됨)
    public virtual void ShowMoveOptions(BoardManager board)
    {
        // 기본 구현: 아무것도 하지 않음
    }

    protected virtual void Awake()
    {
        // 공통 초기화
    }

    public virtual void MoveTo(BoardTile targetTile)
    {
        if (CurrentTile != null) CurrentTile.pieceOnTile = null;
        CurrentTile = targetTile;
        targetTile.pieceOnTile = this;
        StartCoroutine(MoveArcRoutine(targetTile.transform.position, 0.5f, 1.5f));
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0) Destroy(gameObject);
    }

    protected IEnumerator MoveArcRoutine(Vector3 targetPos, float duration, float arcHeight)
    {
        Vector3 start = transform.position;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            Vector3 pos = Vector3.Lerp(start, targetPos, t);
            pos.y += Mathf.Sin(Mathf.PI * t) * arcHeight;
            transform.position = pos;
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
    }

    public IEnumerator KnightSpecialAttack(BoardTile targetTile)
    {
        BoardTile originalTile = CurrentTile;
        yield return MoveArcRoutine(targetTile.transform.position, 0.5f, 1.5f);
        PerformAttack();
        yield return MoveArcRoutine(originalTile.transform.position, 0.5f, 1.5f);
    }
}
