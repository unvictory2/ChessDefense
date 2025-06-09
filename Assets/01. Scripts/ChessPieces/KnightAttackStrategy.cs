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
        // ��ƼŬ ���� (GameObject�� ó��)
        if (_attackParticlesPrefab != null)
        {
            Vector3 spawnPos = center + Vector3.up * 0.5f; // y������ 0.5��ŭ �ø� (�ʿ�� �� ����)
            GameObject particleObj = Object.Instantiate(
                _attackParticlesPrefab,
                spawnPos,
                Quaternion.identity
            );

            // ��ƼŬ �ý��� ������Ʈ ã�Ƽ� ���
            ParticleSystem ps = particleObj.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                Object.Destroy(particleObj, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                // ParticleSystem�� ������ 5�� �� ����
                Object.Destroy(particleObj, 5f);
            }
        }

        // ���� ������ ó��
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
