using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] private GameObject _prefabBullet;
    [SerializeField] private GameObject _target;
    [SerializeField] private Transform _firePoint;

    void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        RotateToTarget();
    }

    public void EndAnimation()
    {
        _playerController.EndAnimationShoot();
    }

    public void InstantiateBullet()
    {
        Instantiate(_prefabBullet, _firePoint.position, transform.rotation); 
    }

    private void RotateToTarget()
    {
        if (_target == null) return;

        Vector3 direction = _target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
    }
}
