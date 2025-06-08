using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    private int _damage;
    private LayerMask _targetLayer;
    private Vector3 _direction;

    public void Initialize(int damage, LayerMask targetLayer, Vector3 direction)
    {
        _damage = damage;
        _targetLayer = targetLayer;
        _direction = direction;
        Destroy(gameObject, 5f);
    }

    void Update() => transform.Translate(_direction * _speed * Time.deltaTime, Space.World);

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _targetLayer) != 0)
        {
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}