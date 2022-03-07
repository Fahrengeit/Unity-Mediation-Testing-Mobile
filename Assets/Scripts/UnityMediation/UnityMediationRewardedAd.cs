using System;
using Unity.Services.Mediation;
using UnityEngine;
using Unity.Services.Core;

/// <summary>
/// Class <c>UnityMediationRewardedAd</c> is a class that initializes,
/// loads and shows the rewarded ad,
/// implements all the necessary methods from <c>IRewardedAdManager</c>.
/// And, also, implements unique for Unity Mediation SDK events like ImpressionEvents
/// </summary>
public class UnityMediationRewardedAd : IRewardedAdManager
{

    private string _adUnitId;
    private IRewardedAd _ad;

    public Action OnLoaded { get; set; }
    public Action<string> OnLoadedFailed { get; set; }
    public Action OnShowed { get; set; }
    public Action<string> OnShowedFailed { get; set; }
    public Action<bool> OnClosed { get; set; }
    public Action OnClicked { get; set; }
    public Action<string> OnReward { get; set; }

    public void Initialize(string id)
    {
        _adUnitId = id;

        //Create
        _ad = MediationService.Instance.CreateRewardedAd(_adUnitId);

        //Subscribe to events
        _ad.OnLoaded += AdLoaded;
        _ad.OnFailedLoad += AdFailedLoad;

        _ad.OnShowed += AdShown;
        _ad.OnFailedShow += AdFailedShow;
        _ad.OnClosed += AdClosed;
        _ad.OnClicked += AdClicked;
        _ad.OnUserRewarded += UserRewarded;


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

        OnShowed?.Invoke();
    }

    void AdClosed(object sender, EventArgs e)
    {
        Debug.Log("Ad has closed");
        // Execute logic after an ad has been closed.

        OnClosed?.Invoke(true);
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

    void UserRewarded(object sender, RewardEventArgs e)
    {
        Debug.Log($"Received reward: type:{e.Type}; amount:{e.Amount}");

        OnReward?.Invoke(e.Amount);
    }


    private void ImpressionEvent(object sender, ImpressionEventArgs args)
    {
        if (args.AdUnitId != _adUnitId) return;

        var impressionData = args.ImpressionData != null ? JsonUtility.ToJson(args.ImpressionData, true) : "null";
        Debug.Log("Impression event from ad unit id " + args.AdUnitId + " " + impressionData);
    }


}
