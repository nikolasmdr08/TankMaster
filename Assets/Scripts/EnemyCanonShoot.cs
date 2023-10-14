using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonController : MonoBehaviour
{
    private PlayerController _playerController;
    private Animator _animatorCannon;
    [SerializeField] private GameObject _prefabBullet;
    [SerializeField] private Transform _firePoint;
    private bool _isShooting;
    [SerializeField] private float _minRotationDistance = 5f;
    [SerializeField] private float _detectionDistance = 10f;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _rayLength = 3f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _playerController = playerObj.GetComponent<PlayerController>();
            if (_playerController == null)
            {
                Debug.LogError("Player object does not have PlayerController component!");
            }
        }
        _animatorCannon = GetComponent<Animator>();
        _isShooting = false;
    }

    void Update()
    {
        if (_playerController != null)
        {
            RotateToTarget();
            ShootInRange();
        }
    }

    public void EndAnimation()
    {
        _isShooting = false;
        _animatorCannon.SetBool("isShooting", _isShooting);
    }

    public void InstantiateBullet()
    {
        Instantiate(_prefabBullet, _firePoint.position, Quaternion.Euler(0, 0, -90));
    }


    private void RotateToTarget()
    {
        float _distanceToPlayer = Vector3.Distance(transform.position, _playerController.transform.position);
        if (_distanceToPlayer <= _minRotationDistance)
        {
            Vector3 _directionToPlayer = (_playerController.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(_directionToPlayer.y, _directionToPlayer.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void ShootInRange()
    {
        Vector3 _directionToPlayer = (_playerController.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, _directionToPlayer * _rayLength, Color.blue);
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, _directionToPlayer, _detectionDistance, _obstacleMask);
        if (!_isShooting && _hit.collider.CompareTag("Player"))
        {
            _isShooting = true;
            _animatorCannon.SetBool("isShooting", _isShooting);
        }
    }
}
