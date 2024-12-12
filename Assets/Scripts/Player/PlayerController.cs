using System;
using Cinemachine;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;

    private Rigidbody _rigidbody;
    private Vector3 _velocity;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_inputReader.RotateValue != 0)
        {
            transform.Rotate(Vector3.up, _inputReader.RotateValue * _rotateSpeed);
        }
    }

    private void FixedUpdate()
    {
        Vector3 forward = _camera.transform.forward * _inputReader.MovementValue.y;
        Vector3 right = _camera.transform.right * _inputReader.MovementValue.x;

        Vector2 direction = new Vector2(forward.x + right.x, forward.z + right.z).normalized;

        if (direction.sqrMagnitude == 0)
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0, _deceleration * Time.fixedDeltaTime);
            _velocity.z = Mathf.MoveTowards(_velocity.z, 0, _deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, direction.x * _moveSpeed, _acceleration * Time.fixedDeltaTime);
            _velocity.z = Mathf.MoveTowards(_velocity.z, direction.y * _moveSpeed, _acceleration * Time.fixedDeltaTime);
        }

        if (_inputReader.HoverValue == 0)
        {
            _velocity.y = Mathf.MoveTowards(_velocity.y, 0, _deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _velocity.y = Mathf.MoveTowards(_velocity.y, _inputReader.HoverValue * _moveSpeed, _acceleration * Time.fixedDeltaTime);
        }

        _rigidbody.velocity = _velocity;
    }

    public void Init(Vector3 middleObjectPosition, float initialFov)
    {
        transform.position = new Vector3(middleObjectPosition.x, transform.position.y, middleObjectPosition.z);
        _camera.m_Lens.FieldOfView = initialFov;
    }
}