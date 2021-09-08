using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private CharacterController _charController;
    private Transform _player;
    [SerializeField] private float _speed = 2.0f;
    private float _gravity = 20.0f;
    private Vector3 _velocity;
    
    void Start()
    {
        _charController = GetComponent<CharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (_charController.isGrounded == true)
        {
            Vector3 direction = _player.position - transform.position;
            direction.Normalize(); // len 1 unit distance
            direction.y = 0;
            // rot twd player
            transform.localRotation = Quaternion.LookRotation(direction);
            _velocity = direction * _speed;
        }

        _velocity.y -= _gravity;
        _charController.Move(_velocity * Time.deltaTime);
    }
}
