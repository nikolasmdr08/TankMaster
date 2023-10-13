using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] private GameObject _prefabBullet;
    [SerializeField] private Transform _firePoint;

    void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        RotateToMouse();
    }

    public void EndAnimation()
    {
        _playerController.EndAnimationShoot();
    }

    public void InstantiateBullet()
    {
        Instantiate(_prefabBullet, _firePoint.position, transform.rotation); // Usar la rotación actual de la torreta
    }

    private void RotateToMouse() { 
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle-90);
    }
}
