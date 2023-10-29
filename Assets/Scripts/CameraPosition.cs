using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    //TODO: TP2 - Remove redundant comments
    // Update is called once per frame
    void Update()
    {
        if( _player != null)
        {
            //TODO: TP2 - Fix - Hardcoded value/s
            transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, -10);
        }
    }
}
