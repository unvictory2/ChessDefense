using UnityEngine;
using System.Collections;

public class KnightAttackStrategy : IAttackStrategy
{
    private Knight _knight;
    private float _attackCooldown;
    private float _lastAttackTime;

    public KnightAttackStrategy(float cooldown)
    {
        _attackCooldown = cooldown;
    }

    public void Attack(ChessPiece knight)
    {
        if (Time.time - _lastAttackTime < _attackCooldown) return;
        if (_knight == null) _knight = knight as Knight;
        if (_knight.IsAttacking || _knight.DetectedEnemies.Count == 0) return;

        _knight.StartCoroutine(JumpAttackCoroutine());
        _lastAttackTime = Time.time;
    }

    IEnumerator JumpAttackCoroutine()
    {
        _knight.IsAttacking = true;

        // 1. 가장 가까운 적 선택
        Enemy target = _knight.GetNearestEnemy();
        if (target == null)
        {
            _knight.IsAttacking = false;
            yield break;
        }

        // 2. 아크 점프 이동
        Vector3 startPos = _knight.transform.position;
        Vector3 targetPos = target.transform.position;
        float jumpDuration = 0.4f;
        float height = 2f;

        for (float t = 0; t < jumpDuration; t += Time.deltaTime)
        {
            float progress = t / jumpDuration;
            float y = height * Mathf.Sin(progress * Mathf.PI);
            _knight.transform.position = Vector3.Lerp(startPos, targetPos, progress)
                                        + Vector3.up * y;
            yield return null;
        }

        // 3. 착지 시 범위 공격
        _knight.ApplyAreaDamage(targetPos);

        // 4. 원위치 복귀
        yield return ReturnToStartCoroutine(startPos);
        _knight.IsAttacking = false;
    }

    IEnumerator ReturnToStartCoroutine(Vector3 startPos)
    {
        float returnDuration = 0.3f;
        Vector3 currentPos = _knight.transform.position;

        for (float t = 0; t < returnDuration; t += Time.deltaTime)
        {
            _knight.transform.position = Vector3.Lerp(currentPos, startPos, t / returnDuration);
            yield return null;
        }
    }
}
