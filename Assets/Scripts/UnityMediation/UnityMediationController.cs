using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Class <c>UnityMediationController</c> is a class that controls Unity Mediation service
/// and used for starting the initialization along with used ads, interstitial and rewarded
/// </summary>
public class UnityMediationController : MonoBehaviour
{
    private UnityMediationInitializer _initializer;

    [SerializeField]
    private string gameID_iOS;
    [SerializeField]
    private string gameID_Android;

    public InterstitialAdController InterstitialAdController;
    public RewardedAdController RewardedAdController;

    public Button InitializeButton;

    private bool _isInitialized;

    public void Awake()
    {
        _initializer = new UnityMediationInitializer();
        InitializeButton.onClick.AddListener(Initialize);
    }

    private void Initialize()
    {
        if (!_isInitialized)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                _initializer.Initialize(gameID_Android);
            }
            else 
            {
                _initializer.Initialize(gameID_iOS);
            }

            _initializer.OnInitialized += Initialized;
            _isInitialized = true;
        }
    }

    private void Initialized()
    {
        InterstitialAdController.Initialize(new UnityMediationInterstitialAd());
        RewardedAdController.Initialize(new UnityMediationRewardedAd());
    }
}
