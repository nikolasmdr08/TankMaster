using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisilleController : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private LayerMask _wallLayer; 
    [SerializeField] private float _avoidDistance = 5.0f; 
    [SerializeField] private GameObject _bulletExplode;
    [SerializeField] private int _damage;
    [SerializeField] private float _bufferDistance = 1.0f;
    private Transform _playerTransform;
    private Vector3 _directionToPlayer;
    private Vector3[] _directions = {  Vector3.up, Vector3.up + Vector3.right, Vector3.right, Vector3.down + Vector3.right,  Vector3.down, Vector3.down + Vector3.left, Vector3.left,  Vector3.up + Vector3.left };

    void Start(){
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null){
            _playerTransform = _player.transform;
        }
    }

    void Update(){
        if (_playerTransform != null){
            _directionToPlayer = (_playerTransform.position - transform.position).normalized;
            Debug.DrawRay(transform.position, _directionToPlayer * _avoidDistance, Color.green);
        }
        else{
            _directionToPlayer = transform.position;
        }
        Vector3 _moveDirection = _directionToPlayer; // por defecto
        bool _shouldAvoidWall = false;
        foreach (Vector3 _dir in _directions){
            RaycastHit2D _hit = Physics2D.Raycast(transform.position, _dir, _avoidDistance + _bufferDistance, _wallLayer);

            if (_hit.collider != null && _hit.distance <= _bufferDistance){
                Vector3 _avoidDirection = Vector3.Cross(_hit.normal, Vector3.forward).normalized;
                transform.position += _avoidDirection * _speed * Time.deltaTime;
                _moveDirection = _avoidDirection;
                _shouldAvoidWall = true;
                break; // Si encuentra una colisión en una de las direcciones, evita la pared y rompe el bucle.
            }
        }
        if (!_shouldAvoidWall){
            Vector3 _newPosition = Vector3.MoveTowards(transform.position, _playerTransform.position, _speed * Time.deltaTime);
            transform.position = _newPosition;
            _moveDirection = _newPosition - transform.position;
        }
        float _moveAngle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, _moveAngle);
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (gameObject.tag == "PlayerBullet" && collision.gameObject.tag != "Player"){
            if (collision.gameObject.tag == "Enemy") CollisionResult(collision);
        }
        if (gameObject.tag == "EnemyBullet" && collision.gameObject.tag != "Enemy"){
            if (collision.gameObject.tag == "Player") CollisionResult(collision);
        }
        if (collision.gameObject.tag == "Wall"){
            Instantiate(_bulletExplode, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

    }

    void CollisionResult(Collision2D collision){
        SubstractLife(collision, _damage);
        Instantiate(_bulletExplode, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void SubstractLife(Collision2D collision, int damage){
        if (collision.gameObject.tag == "Player") collision.gameObject.GetComponent<PlayerController>().GetDamage(damage);

        if (collision.gameObject.tag == "Enemy"){
            if (collision.gameObject.GetComponent<TankEnemyController>() != null)
                collision.gameObject.GetComponent<TankEnemyController>().GetDamage(damage);
            if (collision.gameObject.GetComponent<BaseTurretController>() != null)
                collision.gameObject.GetComponent<BaseTurretController>().GetDamage(damage);
        }
    }

}
