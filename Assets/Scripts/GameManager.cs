using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using Backtrace.Unity;
using Backtrace.Unity.Model;

public class GameManager : MonoBehaviour
{
    // Backtrace client instance
    private BacktraceClient _backtraceClient;
    
  
    void Start()
    {
        var serverUrl = "https://submit.backtrace.io/testing-game-dev-zachlo89/2210a76995f21ae1bd1857da832bcb55096767fecf268e61196fd980130ff8b2/json";
        var gameObjectName = "Backtrace";
        var databasePath =  "${Application.persistentDataPath}/sample/backtrace/path";
        var attributes = new Dictionary<string, string>() { {"my-custom-attribute", "attribute-value"} };

        // use game object to initialize Backtrace integration
        _backtraceClient = GameObject.Find(gameObjectName).GetComponent<BacktraceClient>();
        //Read from manager BacktraceClient instance
        var database = GameObject.Find(gameObjectName).GetComponent<BacktraceDatabase>();

        // or initialize Backtrace integration directly in your source code
        _backtraceClient = BacktraceClient.Initialize(
            url: serverUrl,
            databasePath: databasePath ,
            gameObjectName: gameObjectName,
            attributes: attributes);
    }


    void Update()
    {
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
}
