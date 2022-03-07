using System;
using Unity.Services.Mediation;
using UnityEngine;

/// <summary>
/// Class <c>UnityMediationInterstitialAd</c> is a class that initializes,
/// loads and shows the interstitial ad,
/// implements all the necessary methods from <c>IInterstitialAdManager</c>.
/// And, also, implements unique for Unity Mediation SDK events like ImpressionEvents
/// </summary>
public class UnityMediationInterstitialAd : IInterstitialAdManager
{
    private string _adUnitId;
    private IInterstitialAd _ad;

    // There's no obvious way in API to check if an interstitial ad was skipped or not,
    // so I use a timer to determine that. If a user closes the ad and spends less time,
    // then it counts as skipped.
    [SerializeField]
    private int minAdTimer = 30;

    private DateTime snapshotTime;

    public Action OnLoaded { get; set; }
    public Action<string> OnLoadedFailed { get; set; }
    public Action OnShowed { get; set; }
    public Action<string> OnShowedFailed { get; set; }
    public Action<bool> OnClosed { get; set; }
    public Action OnClicked { get; set; }

    public void Initialize(string id)
    {
        _adUnitId = id;

        //Create
        _ad = MediationService.Instance.CreateInterstitialAd(_adUnitId);

        //Subscribe to events
        _ad.OnLoaded += AdLoaded;
        _ad.OnFailedLoad += AdFailedLoad;

        _ad.OnShowed += AdShown;
        _ad.OnFailedShow += AdFailedShow;
        _ad.OnClosed += AdClosed;
        _ad.OnClicked += AdClicked;
        // Impression Event
        MediationService.Instance.ImpressionEventPublisher.OnImpression += ImpressionEvent;
    }

    public void Load()
    {
        _ad.Load();
    }

    public bool IsLoaded()
    {
        return _ad.AdState == AdState.Loaded;
    }

    public string GetAdInfo()
    {
        return "Ad Loaded";
    }

    public void Show()
    {
        if (_ad.AdState == AdState.Loaded)
        {
            _ad.Show();
        }
    }

    void AdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Ad loaded");

        OnLoaded?.Invoke();
    }

    void AdFailedLoad(object sender, LoadErrorEventArgs args)
    {
        Debug.Log("Failed to load ad");
        Debug.Log(args.Message);

        OnLoadedFailed?.Invoke(args.Message);
    }

    void AdShown(object sender, EventArgs args)
    {
        Debug.Log("Ad shown!");

        snapshotTime = DateTime.Now;

        OnShowed?.Invoke();
    }

    void AdClosed(object sender, EventArgs e)
    {
        Debug.Log("Ad has closed");

        // Execute logic after an ad has been closed.
        OnClosed?.Invoke((DateTime.Now - snapshotTime).TotalSeconds > minAdTimer);
    }

    void AdClicked(object sender, EventArgs e)
    {
        Debug.Log("Ad has been clicked");

        // Execute logic after an ad has been clicked.
        OnClicked?.Invoke();
    }

    void AdFailedShow(object sender, ShowErrorEventArgs args)
    {
        Debug.Log(args.Message);

        OnShowedFailed?.Invoke(args.Message);
    }


    private void ImpressionEvent(object sender, ImpressionEventArgs args)
    {
        if (args.AdUnitId != _adUnitId) return;

        var impressionData = args.ImpressionData != null ? JsonUtility.ToJson(args.ImpressionData, true) : "null";
        Debug.Log("Impression event from ad unit id " + args.AdUnitId + " " + impressionData); 
    }
}
