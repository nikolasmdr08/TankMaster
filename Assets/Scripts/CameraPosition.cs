using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    // Update is called once per frame
    void Update()
    {
        if( _player != null)
        {
            transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, -10);
        }
    }
}
