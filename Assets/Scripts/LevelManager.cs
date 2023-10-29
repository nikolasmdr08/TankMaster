using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private GameObject _player;
    private GameObject[] _enemies;

    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _gameWin;
    [SerializeField] private GameObject _gameLose; 
    [SerializeField] private TMP_Text _targetText;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] _enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        _enemies = new GameObject[_enemyObjects.Length];

        for (int i = 0; i < _enemyObjects.Length; i++)
        {
            _enemies[i] = _enemyObjects[i];
        }
    }

    void Update()
    {
        //TODO: TP2 - Optimization - Cache values/refs
        GameObject[] _actualEnemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        _targetText.text = "Objetivos: " + _actualEnemyObjects.Length  + " / " + _enemies.Length;
        //TODO: TP2 - Spelling error/Code in spanish/Code in spanglish
        // Verificar si el jugador ha muerto
        if (_player == null)
        {
            _menu.SetActive(true);
            _gameLose.SetActive(true);
        }
        else
        {
            //TODO: TP2 - Spelling error/Code in spanish/Code in spanglish
            // Verificar si no quedan enemigos en la escena
            bool allEnemiesDead = true;

            for (int i = 0; i < _enemies.Length; i++)
            {
                if (_enemies[i] != null)
                {
                    allEnemiesDead = false;
                    break;
                }
            }

            if (allEnemiesDead)
            {
                _menu.SetActive(true);
                _gameWin.SetActive(true);
            }
        }
    }
}
