using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandingScript : MonoBehaviour
{
    private GameObject _waterSurface;
    private GameObject _player;
    private Player _playerScript;
    private RaycastHit _hit;


    void Awake()
    {
        _waterSurface = GameObject.FindGameObjectWithTag("Water Volume");
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerScript = _player.GetComponent<Player>();   
    }


    void Update()
    {
        bool isHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out _hit, 5f);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down), Color.red);


        if (isHit && _hit.collider.gameObject == _waterSurface && !_playerScript.isLost)
        {
            _playerScript.isWin = true;
        }

    }


}