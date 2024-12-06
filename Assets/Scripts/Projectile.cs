using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private EnemyController _target;
    private int _damage;

    public void Init(EnemyController enemy, int damage)
    {
        _target = enemy;
        _damage = damage;
    }

    private void Update()
    {
        if (_target)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, 10 * Time.deltaTime);
            if (Vector3.Distance(transform.position, _target.transform.position) < 0.1f)
            {
                _target.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}