using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTurretController : MonoBehaviour
{
    private LifeManager _life;
    [SerializeField] private int _maxLife;
    [SerializeField] private GameObject _DeathPrefab;

    void Start()
    {
        _life = new LifeManager();
        _life.SetMaxLife(_maxLife);
        _life.SetCurrentLife(_maxLife);
    }

    void Update(){
        if (_life.GetCurrentLife() <= 0){
            Death();
        }
    }
    public void GetDamage(int _damage){
        _life.SubtractLife(_damage);
    }

    private void Death(){
        Instantiate(_DeathPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
