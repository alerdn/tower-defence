using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float _range;
    [SerializeField] private int _damage;
    [SerializeField] private float _fireRate;
    [SerializeField] private Projectile _projectilePrefab;

    [Header("Debug")]
    [SerializeField] private EnemyController _target;
    private Coroutine _shootRoutine;

    private void Update()
    {
        CheckTarget();
        CheckFireRoutine();
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / _fireRate);

            Collider[] colliders = Physics.OverlapSphere(transform.position, _range, LayerMask.GetMask("Enemy"));
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out EnemyController enemy))
                {
                    Projectile projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity, transform);
                    projectile.Init(enemy, _damage);
                    break;
                }
            }
        }
    }

    private void CheckTarget()
    {
        if (!_target)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _range, LayerMask.GetMask("Enemy"));
            if (colliders.Length > 0)
            {
                _target = colliders[0].gameObject.GetComponent<EnemyController>();
                _target.OnDeath += (EnemyController enemy, bool isKilled) =>
                {
                    if (_target == enemy)
                    {
                        _target = null;
                    }
                };
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, _target.transform.position) > _range)
            {
                _target = null;
            }
        }
    }

    private void CheckFireRoutine()
    {
        if (_target)
        {
            _shootRoutine ??= StartCoroutine(ShootRoutine());
        }
        else
        {
            if (_shootRoutine != null)
            {
                StopCoroutine(_shootRoutine);
                _shootRoutine = null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}