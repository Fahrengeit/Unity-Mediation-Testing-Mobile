using UnityEngine;
using UnityEngine.UI;

namespace AdMediation.UnityMediation
{
    /// <summary>
    /// Class <c>UnityMediationController</c> is a class that controls Unity Mediation service
    /// and used for starting the initialization along with used ads, interstitial and rewarded
    /// </summary>
    public class UnityMediationController : MonoBehaviour
    {
        private UnityMediationInitializer initializer;

        [SerializeField]
        private string gameID_iOS;
        [SerializeField]
        private string gameID_Android;
        [SerializeField]
        private InterstitialAdController interstitialAdController;
        [SerializeField]
        private RewardedAdController rewardedAdController;
        [SerializeField]
        private Button initializeButton;

        private bool _isInitialized;

        private void Awake()
        {
            initializer = new UnityMediationInitializer();
            initializeButton.onClick.AddListener(Initialize);
        }

        private async void Initialize()
        {
            if (!_isInitialized)
            {
                initializer.OnInitialized += Initialized;

                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        await initializer.Initialize(gameID_Android);
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        await initializer.Initialize(gameID_iOS);
                        break;
                    default:
#if UNITY_EDITOR
                        await initializer.Initialize(gameID_iOS);
#else
                        return;
#endif
                        break;
                }
                _isInitialized = true;
            }
        }

        private void Initialized()
        {
            interstitialAdController.Initialize(new UnityMediationInterstitialAd());
            rewardedAdController.Initialize(new UnityMediationRewardedAd());
        }
    }
}
