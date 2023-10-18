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
        Instantiate(_prefabBullet, _firePoint.position, transform.rotation); 
    }

    private void RotateToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distanceToGround;
        Plane groundPlane = new Plane(Vector3.forward, Vector3.zero);
        if (groundPlane.Raycast(ray, out distanceToGround))
        {
            Vector3 hitPoint = ray.GetPoint(distanceToGround);
            Vector3 direction = hitPoint - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
}
