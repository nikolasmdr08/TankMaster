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
        if (_playerController != null){
            _playerPosition = _playerController.transform;
            _direction = _playerPosition.position - transform.position;
        }
    }

    void Update(){
        _rb.velocity = _direction.normalized * _moveSpeed;
    }

    public int GetDamage(){
        return _damage;
    } 

    private void OnCollisionEnter2D(Collision2D _collision){
        if (gameObject.tag == "PlayerBullet" && _collision.gameObject.tag != "Player"){
            if (_collision.gameObject.tag == "Enemy") 
                CollisionResult(_collision);
            if (_collision.gameObject.tag == "EnemyBullet")
            {
                Instantiate(_bulletExplode, transform.position, Quaternion.identity);
                Destroy(_collision.gameObject);
                Destroy(this.gameObject);
            }
        }
        if (gameObject.tag == "EnemyBullet" && _collision.gameObject.tag != "Enemy"){
            if (_collision.gameObject.tag == "Player") CollisionResult(_collision);
        }
        if(_collision.gameObject.tag == "Wall"){
            Instantiate(_bulletExplode, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }    
    }

    private void CollisionResult(Collision2D _collision){
        SubstractLife(_collision, _damage);
        Instantiate(_bulletExplode, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void SubstractLife(Collision2D _collision, int _damage)
    {
        if (_collision.gameObject.tag == "Player") _collision.gameObject.GetComponent<PlayerController>().GetDamage(_damage);
     
        if (_collision.gameObject.tag == "Enemy") { 
            if(_collision.gameObject.GetComponent<TankEnemyController>() != null)
                _collision.gameObject.GetComponent<TankEnemyController>().GetDamage(_damage);
            if (_collision.gameObject.GetComponent<BaseTurretController>() != null)
                _collision.gameObject.GetComponent<BaseTurretController>().GetDamage(_damage);
        }
    }
}
