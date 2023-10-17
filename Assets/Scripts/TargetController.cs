using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    void Update()
    {
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distanceToGround = 1000f; // Puedes ajustar esto según tus necesidades
        Plane _groundPlane = new Plane(Vector3.forward, Vector3.zero); // Plano XY en la posición Z=0

        if (_groundPlane.Raycast(_ray, out distanceToGround))
        {
            Vector3 _hitPoint = _ray.GetPoint(distanceToGround);
            transform.position = _hitPoint;
        }
    }
}
