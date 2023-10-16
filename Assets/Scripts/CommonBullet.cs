using System;
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
    [SerializeField] private GameObject _bulletExplode;

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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag == "PlayerBullet" && collision.gameObject.tag != "Player")
        {
            if (collision.gameObject.tag == "Enemy") CollisionResult(collision);
        }
        if (gameObject.tag == "EnemyBullet" && collision.gameObject.tag != "Enemy")
        {
            if (collision.gameObject.tag == "Player") CollisionResult(collision);
        }
        if(collision.gameObject.tag == "Wall")
        {
            Instantiate(_bulletExplode, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
             
    }

    private void CollisionResult(Collision2D collision)
    {
        SubstractLife(collision, _damage);
        Instantiate(_bulletExplode, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void SubstractLife(Collision2D collision, int damage)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().GetDamage(damage);
        }
     
        if (collision.gameObject.tag == "Enemy") { 
            if(collision.gameObject.GetComponent<TankEnemyController>() != null)
                collision.gameObject.GetComponent<TankEnemyController>().GetDamage(damage);
            if (collision.gameObject.GetComponent<BaseTurretController>() != null)
                collision.gameObject.GetComponent<BaseTurretController>().GetDamage(damage);
        }
        Debug.Log(collision.gameObject.tag + ": " + damage);
    }
}
