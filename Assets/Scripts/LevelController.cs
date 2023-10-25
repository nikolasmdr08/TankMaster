using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class LevelController : MonoBehaviour
{
    [SerializeField] private TMP_Text _lifePointsText;
    private PlayerController _player;
    private GameObject[] _enemys;

    void Start(){
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _enemys = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update(){
        if(_player != null){
            _lifePointsText.text = _player.GetCurrentLife().ToString();
        }
    }
}
