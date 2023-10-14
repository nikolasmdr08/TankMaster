susing System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObjectAndExplode : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private GameObject _explosionPrefab;

    void Start()
    {
        Invoke("DestroyObject", _lifeTime);
    }

    private void DestroyObject()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }
}
