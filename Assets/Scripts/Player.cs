using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Backtrace.Unity;
using Backtrace.Unity.Model;

public class Player : MonoBehaviour
{

    private CharacterController _charController;
    
    [Header(("Controller Settings"))]
    [SerializeField] float _speed = 5.0f;
    [SerializeField] private float _jumpHeight = 8.0f;
    [SerializeField] private float _gravity = 20.0f;
    
    private Vector3 _direction;
    private Vector3 _velocity;

    //public float mouseX;
    
    private Camera _mainCam;
    
    [Header(("Camera Settings"))]
    [SerializeField] private float _camSensitivity = 1.5f;
    
    void Start()
    {
        _charController = GetComponent<CharacterController>();
        if (_charController == null)
        {
            //Debug.LogError("Character Controller is NULL");
            throw new NullReferenceException("NO CHARACTER CONTROLLER!");

        }
        
        
        _mainCam = Camera.main;
        if (_mainCam == null)
        {
            Debug.LogError("Main Cam is NULL");
        }
        
        // lock cursor & hide when game starts
        Cursor.lockState = CursorLockMode.Locked;

        System.Diagnostics.Debugger.Launch();
        
    }
    
    void Update()
    {
        CalculateMovement();
        CamController();
 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
    }
    
    void CalculateMovement()
    {
        if (_charController.isGrounded == true)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            _direction = new Vector3(horizontal, 0, vertical);
            _velocity = _direction * _speed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _velocity.y = _jumpHeight;
            }
        }
        
        // controller not grounded apply gravity
        _velocity.y -= _gravity * Time.deltaTime;
        
        // tform local to world space Transform.TransformDirection
        // velocity and tform dir to local dir
        // move in dir facing
        _velocity = transform.TransformDirection(_velocity);

        _charController.Move(_velocity * Time.deltaTime);
    }

    
    void CamController()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
            
        // apply mouseX to player rot y (look L & R)
        // transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + mouseX,
        // transform.localEulerAngles.z);

        Vector3 rotY = transform.localEulerAngles;
        rotY.y += mouseX * _camSensitivity;
            
        //transform.localEulerAngles = rotY;
        // alt for gimble lock
        transform.localRotation = Quaternion.AngleAxis(rotY.y, Vector3.up);
            
        // apply mouseY to cam rot x (look UP & DOWN)
        Vector3 currentCamRotation = _mainCam.gameObject.transform.localEulerAngles;
        currentCamRotation.x -= mouseY * _camSensitivity;
        _mainCam.gameObject.transform.localRotation = Quaternion.AngleAxis(currentCamRotation.x, Vector3.right); // rot around axis
    }
}
