using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisilleController : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private LayerMask _wallLayer; 
    [SerializeField] private float _avoidDistance = 5.0f; 
    [SerializeField] private float _raycastOffset = 2.0f;
    [SerializeField] private GameObject _bulletExplode;
    [SerializeField] private int _damage;
    private Transform _playerTransform;

    void Start()
    {
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            _playerTransform = _player.transform;
        }
    }

    void Update()
    {
        if (_playerTransform != null)
        {
            Vector3 _direction = (_playerTransform.position - transform.position).normalized;
            bool _isObstacleForward = Physics.Raycast(transform.position, _direction, _avoidDistance, _wallLayer);
            bool _isObstacleLeft = Physics.Raycast(transform.position + transform.right * -_raycastOffset, _direction, _avoidDistance, _wallLayer);
            bool _isObstacleRight = Physics.Raycast(transform.position + transform.right * _raycastOffset, _direction, _avoidDistance, _wallLayer);
            if (_isObstacleForward)
            {
                if (_isObstacleLeft && !_isObstacleRight)
                {
                    _direction += transform.right;
                }
                else if (!_isObstacleLeft && _isObstacleRight)
                {
                    _direction -= transform.right;
                }
                else if (!_isObstacleLeft && !_isObstacleRight)
                {
                    _direction += Random.Range(-1, 2) * transform.right;
                }
            }
            transform.position += _direction.normalized * _speed * Time.deltaTime;
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle-90);
        }
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
        if (collision.gameObject.tag == "Wall")
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

        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<TankEnemyController>() != null)
                collision.gameObject.GetComponent<TankEnemyController>().GetDamage(damage);
            if (collision.gameObject.GetComponent<BaseTurretController>() != null)
                collision.gameObject.GetComponent<BaseTurretController>().GetDamage(damage);
        }
        Debug.Log(collision.gameObject.tag + ": " + damage);
    }

}
