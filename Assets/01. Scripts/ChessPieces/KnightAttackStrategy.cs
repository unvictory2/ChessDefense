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

    public KnightAttackStrategy(float cooldown, int damage)
    {
        _attackCooldown = cooldown;
        _damage = damage;
    }

    public void Attack(ChessPiece knight)
    {
        if (Time.time - _lastAttackTime < _attackCooldown) return;
        if (_knight == null) _knight = knight as Knight;

        // ���� ������ ���� üũ
        _detectedEnemies.RemoveAll(enemy => enemy == null);
        if (_detectedEnemies.Count == 0) return;

        _knight.StartCoroutine(JumpAttackCoroutine());
        _lastAttackTime = Time.time;
    }

    // ���� ������ �� ���/���� �޼���
    public void OnEnemyDetected(Enemy enemy) => _detectedEnemies.Add(enemy);
    public void OnEnemyLeft(Enemy enemy) => _detectedEnemies.Remove(enemy);

    private IEnumerator JumpAttackCoroutine()
    {
        // ����Ʈ ���� ���� ����
        Vector3 startPos = _knight.transform.position;
        Enemy target = GetNearestEnemy(startPos);
        if (target == null) yield break;

        // ��ũ ���� �̵�
        float duration = 0.4f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            float y = 2f * Mathf.Sin(progress * Mathf.PI);
            _knight.transform.position = Vector3.Lerp(startPos, target.transform.position, progress)
                                       + Vector3.up * y;
            yield return null;
        }

        // ���� ����
        ApplyAreaDamage(target.transform.position);

        // ����
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

    // ���� ������
    private void ApplyAreaDamage(Vector3 center)
    {
        Collider[] hits = Physics.OverlapSphere(center, 2.5f, LayerMask.GetMask("Enemy")); 
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
