using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonBullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    private GameObject _playerController;
    private Transform _playerPosition;
    private Vector2 _direction;
    private float _speed;

    [SerializeField] private float _moveSpeed;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerController = GameObject.FindGameObjectWithTag("Player");
        _playerPosition = _playerController.transform;
        _direction = transform.position - _playerPosition.position;
    }

    void Update()
    {
        _rb.velocity = _direction.normalized * _moveSpeed;
    }
}
