using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonBullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    private GameObject _playerController;
    private Transform _playerPosition;
    private Vector2 _direction;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private string _target;
    [SerializeField] private int _damage;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerController = GameObject.FindGameObjectWithTag(_target);
        _playerPosition = _playerController.transform;
        _direction = _playerPosition.position - transform.position;
    }

    void Update()
    {
        _rb.velocity = _direction.normalized * _moveSpeed;
    }

    public int GetDamage()
    {
        return _damage;
    }
}
