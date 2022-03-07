using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Class <c>InterstitialAdController</c> is a class that controls everything a user needs for creating,
/// loading and showing an interstitial ad in this application.
/// </summary>
public class InterstitialAdController : MonoBehaviour
{
    private IInterstitialAdManager _interstitialAdManager;

    [SerializeField]
    private string adUnitId_iOS;
    [SerializeField]
    private string adUnitId_Android;

    public Button LoadButton;
    public Text LoadStatus;
    public Button ShowButton;
    public Text ShowInfo;

    public GameObject StatusWindow;
    public Text StatusText;

    /// <summary>
    /// In Awake we add listeners to UI buttons, so we can load and show an ad
    /// And make sure that buttons are disabled, because a user shouldn't be able to press them
    /// before ads are initialized
    /// </summary>
    private void Awake()
    {
        if (LoadButton == null || LoadStatus == null || ShowButton == null || ShowInfo == null)
            throw new NullReferenceException("UI fields must be assigned");

        LoadButton.onClick.AddListener(LoadAd);
        ShowButton.onClick.AddListener(ShowAd);

        LoadButton.interactable = false;
        ShowButton.interactable = false;

        LoadStatus.text = "Not Loaded";
        ShowInfo.text = "No Ad";
    }

    /// <summary>
    /// After ads initialization, we can initialize the ad manager that can
    /// use any mediation as long as every essential method is implemented
    /// </summary>
    public virtual void Initialize(IInterstitialAdManager adManager)
    {    
        _interstitialAdManager = adManager;
        if (Application.platform == RuntimePlatform.Android)
        {
            _interstitialAdManager.Initialize(adUnitId_Android);
        }
        else 
        {
            _interstitialAdManager.Initialize(adUnitId_iOS);
        }
        _interstitialAdManager.OnLoaded += OnLoadedAd;
        _interstitialAdManager.OnClosed += OnClosedAd;

        LoadButton.interactable = true;
    }

    private void LoadAd()
    {
        LoadStatus.text = "Loading...";
        _interstitialAdManager.Load();
    }

    private void OnLoadedAd()
    {
        LoadStatus.text = "Loaded";

        // Only after ad is loaded we unblock the ShowButton, so a user couldn't
        // try to show a not loaded ad
        ShowButton.interactable = true;

        ShowInfo.text = _interstitialAdManager.GetAdInfo() ?? "";
    }

    private void ShowAd()
    {
        _interstitialAdManager.Show();
    }

    private void OnClosedAd(bool status)
    {
        StatusWindow?.SetActive(true);
        if (StatusText != null)
        {
            StatusText.text = status ? "Ad watched completely" : "Ad skipped";
        }
        ResetAd();
    }

    private void ResetAd()
    {
        ShowButton.interactable = false;
        LoadStatus.text = "Not Loaded";
        ShowInfo.text = "No Ad";
    }

}
