using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class InitUGS : MonoBehaviour
{

    public string environment = "ecca3819-4f90-4548-8425-2ef615743991";

    async void Start()
    {
        try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);
        }
        catch (Exception exception)
        {
            // An error occurred during initialization.
        }
    }
}
