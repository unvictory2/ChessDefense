using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KnightAttackStrategy : IAttackStrategy
{
    private float _attackCooldown;
    private int _damage;
    private float _lastAttackTime;
    private List<Enemy> _detectedEnemies = new List<Enemy>();
    private Knight _knight;
    Vector3 startPos;

    private GameObject _attackParticlesPrefab;

    public KnightAttackStrategy(float cooldown, int damage, GameObject particles)
    {
        _attackCooldown = cooldown;
        _damage = damage;
        _attackParticlesPrefab = particles;
    }


    public void Attack(ChessPiece knight)
    {
        if (Time.time - _lastAttackTime < _attackCooldown) return;
        if (_knight == null)
        {
            _knight = knight as Knight;
            startPos = _knight.transform.position;
        }
        // 폰과 동일한 조건 체크
        _detectedEnemies.RemoveAll(enemy => enemy == null);
        if (_detectedEnemies.Count == 0) return;

        _knight.StartCoroutine(JumpAttackCoroutine());
        _lastAttackTime = Time.time;
    }

    // 폰과 동일한 적 등록/해제 메서드
    public void OnEnemyDetected(Enemy enemy) => _detectedEnemies.Add(enemy);
    public void OnEnemyLeft(Enemy enemy) => _detectedEnemies.Remove(enemy);

    private IEnumerator JumpAttackCoroutine()
    {
        // 나이트 전용 공격 로직
        Enemy target = GetNearestEnemy(startPos);
        if (target == null) yield break;

        // 아크 점프 이동
        float duration = 0.4f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            float y = 2f * Mathf.Sin(progress * Mathf.PI);
            _knight.transform.position = Vector3.Lerp(startPos, target.transform.position, progress)
                                       + Vector3.up * y;
            yield return null;
        }

        // 범위 공격
        ApplyAreaDamage(target.transform.position);

        // 복귀
        yield return ReturnToStartCoroutine(startPos);
    }

    private Enemy GetNearestEnemy(Vector3 origin)
    {
        Enemy nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Enemy enemy in _detectedEnemies)
        {
            float distance = Vector3.Distance(origin, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }
        return nearest;
    }

    // 범위 데미지
    private void ApplyAreaDamage(Vector3 center)
    {
        // 파티클 생성 (GameObject로 처리)
        if (_attackParticlesPrefab != null)
        {
            Vector3 spawnPos = center + Vector3.up * 0.5f; // y축으로 0.5만큼 올림 (필요시 값 조정)
            GameObject particleObj = Object.Instantiate(
                _attackParticlesPrefab,
                spawnPos,
                Quaternion.identity
            );

            // 파티클 시스템 컴포넌트 찾아서 재생
            ParticleSystem ps = particleObj.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                Object.Destroy(particleObj, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                // ParticleSystem이 없으면 5초 후 삭제
                Object.Destroy(particleObj, 5f);
            }
        }

        // 범위 데미지 처리
        Collider[] hits = Physics.OverlapSphere(center, 1.5f, LayerMask.GetMask("Enemy"));
        foreach (Collider hit in hits)
        {
            hit.GetComponent<IDamageable>()?.TakeDamage(_damage);
        }
    }


    private IEnumerator ReturnToStartCoroutine(Vector3 startPos)
    {
        float duration = 0.3f;
        Vector3 currentPos = _knight.transform.position;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            _knight.transform.position = Vector3.Lerp(currentPos, startPos, t / duration);
            yield return null;
        }
    }
}
