using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTurretController : MonoBehaviour
{
    private LifeManager _life;
    [SerializeField] private int _maxLife;
    [SerializeField] private GameObject _DeathPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _life = new LifeManager();
        _life.SetMaxLife(_maxLife);
        _life.SetCurrentLife(_maxLife);
    }

    // Update is called once per frame
    void Update()
    {
        if (_life.GetCurrentLife() <= 0)
        {
            Death();
        }
    }
    public void GetDamage(int damage)
    {
        _life.SubtractLife(damage);
    }

    private void Death()
    {
        Instantiate(_DeathPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
