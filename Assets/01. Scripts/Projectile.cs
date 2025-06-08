using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int _damage;
    private LayerMask _targetLayer;
    private Vector3 _direction;

    public void Initialize(int damage, LayerMask targetLayer, Vector3 direction)
    {
        _damage = damage;
        _targetLayer = targetLayer;
        _direction = direction;
    }

    void Update()
    {
        transform.Translate(_direction * 10f * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if ((_targetLayer & (1 << other.gameObject.layer)) != 0)
        {
            other.GetComponent<IDamageable>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
