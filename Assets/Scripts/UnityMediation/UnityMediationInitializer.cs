using System;
using Unity.Services.Core;
using Unity.Services.Mediation;

/// <summary>
/// Class <c>UnityMediationInitializer</c> is a class that initializes
/// Unity Mediation SDK
/// </summary>
public class UnityMediationInitializer
{
    public Action OnInitialized { get; set; }

    public async void Initialize(string key)
    {
        try
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetGameId(key);
            await UnityServices.InitializeAsync(initializationOptions);
            InitializationSuccess();
        }
        catch (Exception e)
        {
            InitializationFailed(e);
        }       

    }

    private void InitializationSuccess()
    {
        Console.WriteLine("Unity Mediation Initialized");

        OnInitialized?.Invoke();
    }

    private void InitializationFailed(Exception e)
    {
        throw e;
    }
}
