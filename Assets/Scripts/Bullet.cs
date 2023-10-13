using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed;
    [SerializeField] private float _lifeTime;

    void Start()
    {
        Invoke("DestroyBullet", _lifeTime);
    }

    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
