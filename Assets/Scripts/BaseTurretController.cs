using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTurretController : MonoBehaviour
{
    private LifeManager _lifePlayer;
    [SerializeField] private int _maxLifePlayer;

    // Start is called before the first frame update
    void Start()
    {
        _lifePlayer = new LifeManager();
        _lifePlayer.SetMaxLife(_maxLifePlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
