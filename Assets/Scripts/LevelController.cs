using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using TMPro.Examples;
using UnityEngine;
using TMPro;

public class LevelController : MonoBehaviour
{
    private PlayerController _player;
    private GameObject[] _enemys;

    [SerializeField] private TMP_Text _lifePointsText;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _enemys = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if(_player != null)
        {
            _lifePointsText.text = _player.GetCurrentLife().ToString();
        }
    }
}
