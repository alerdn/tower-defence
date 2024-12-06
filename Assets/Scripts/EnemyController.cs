using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public event Action<EnemyController, bool> OnDeath;

    [Header("Settings")]
    [SerializeField] private int _health;
    [SerializeField] private float _stopingDistance;

    private Vector3 _goal;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _goal = GameObject.FindWithTag("Goal").transform.position + Vector3.up / 2f;

        _agent.SetDestination(_goal);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _goal) <= _stopingDistance)
        {
            OnDeath?.Invoke(this, false);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        _health = Mathf.Max(_health - damage, 0);

        if (_health == 0)
        {
            OnDeath?.Invoke(this, true);
            Destroy(gameObject);
        }
    }
}
