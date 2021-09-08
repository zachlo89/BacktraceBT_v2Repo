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
    // Backtrace client instance
    // private BacktraceClient _backtraceClient;
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
        System.Environment.FailFast("Error happened WHA????");
        
        
        // var serverUrl = "https://submit.backtrace.io/testing-game-dev-zachlo89/2210a76995f21ae1bd1857da832bcb55096767fecf268e61196fd980130ff8b2/json";
        // var gameObjectName = "Backtrace";
        // var databasePath =  "${Application.persistentDataPath}/sample/backtrace/path";
        // var attributes = new Dictionary<string, string>() { {"my-super-cool-attribute-name", "attribute-value"} };
        //
        // // use game object to initialize Backtrace integration
        // _backtraceClient = GameObject.Find(gameObjectName).GetComponent<BacktraceClient>();
        // //Read from manager BacktraceClient instance
        // var database = GameObject.Find(gameObjectName).GetComponent<BacktraceDatabase>();
        //
        // // or initialize Backtrace integration directly in your source code
        // _backtraceClient = BacktraceClient.Initialize(
        //     url: serverUrl,
        //     databasePath: databasePath ,
        //     gameObjectName: gameObjectName,
        //     attributes: attributes);
    }
    
    void Update()
    {
        CalculateMovement();
        CamController();
        // tform local to world space Transform.TransformDirection
        // velocity and tform dir to local dir
        
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
        
        // try
        // {
        //     // throw an exception here
        // }
        // catch (Exception exception)
        // {
        //     var report = new BacktraceReport(
        //         exception: exception,
        //         attributes: new Dictionary<string, object>() { { "key", "value" } },
        //         attachmentPaths: new List<string>() { @"file_path_1", @"file_path_2" }
        //     );
        //     _backtraceClient.Send(report);
        // }
        
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
