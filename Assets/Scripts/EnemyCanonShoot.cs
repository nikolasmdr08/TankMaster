using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonController : MonoBehaviour
{
    private PlayerController _playerController;
    private Animator _animatorCannon;
    private bool _isShooting;
    [SerializeField] private GameObject _prefabBullet;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _minRotationDistance = 5f;
    [SerializeField] private float _detectionDistance = 10f;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _rayLength = 3f;

    void Start(){
        GameObject _playerObj = GameObject.FindGameObjectWithTag("Player");
        if (_playerObj != null){
            _playerController = _playerObj.GetComponent<PlayerController>();
        }
        _animatorCannon = GetComponent<Animator>();
        _isShooting = false;
    }

    void Update(){
        if (_playerController != null){
            RotateToTarget();
            ShootInRange();
        }
    }

    public void EndAnimation(){
        _isShooting = false;
        _animatorCannon.SetBool("isShooting", _isShooting);
    }

    //TODO: TP2 - Fix - Calling these methods from animation events makes the logic really hard to follow and prevent the game designers from making changes to shoot times and other variables
    public void InstantiateBullet(){
        if (_playerController != null){
            Instantiate(_prefabBullet, _firePoint.position, _firePoint.rotation);
        }
    }

    private void RotateToTarget(){
        float _distanceToPlayer = Vector3.Distance(transform.position, _playerController.transform.position);
        if (_distanceToPlayer <= _minRotationDistance){
            Vector3 _directionToPlayer = (_playerController.transform.position - transform.position).normalized;
            float _angle = Mathf.Atan2(_directionToPlayer.y, _directionToPlayer.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, _angle);
        }
    }

    private void ShootInRange(){
        Vector3 _directionToPlayer = (_playerController.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, _directionToPlayer * _rayLength, Color.blue);
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, _directionToPlayer, _detectionDistance, _obstacleMask);
        if (_hit && !_isShooting && _hit.collider.CompareTag("Player")){
            _isShooting = true;
            _animatorCannon.SetBool("isShooting", _isShooting);
        }
    }
}
