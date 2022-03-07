using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Mediation;
using UnityEngine;

namespace AdMediation.UnityMediation
{
    /// <summary>
    /// Class <c>UnityMediationInitializer</c> is a class that initializes
    /// Unity Mediation SDK
    /// </summary>
    public class UnityMediationInitializer
    {
        public event Action OnInitialized;

        public async Task Initialize(string key)
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
            Debug.Log("Unity Mediation Initialized");

            OnInitialized?.Invoke();
        }

        private void InitializationFailed(Exception e)
        {
            throw e;
        }
    }
}
