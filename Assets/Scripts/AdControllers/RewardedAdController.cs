using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class <c>RewardedAdController</c> is a class that controls everything a user needs for creating,
/// loading and showing a rewarded ad in this application.
/// Interstitial and Rewarded ads are somewhat similar in terms of API, so it only makes sense
/// to inherit here from InterstitialAdController
/// </summary>
public class RewardedAdController : InterstitialAdController
{
    private IRewardedAdManager _rewardedAdManager;

    public Text RewardedCounterText;
    private int _rewardCounter = 0;

    public override void Initialize(IInterstitialAdManager adsManager)
    {
        base.Initialize(adsManager);

        _rewardedAdManager = adsManager as IRewardedAdManager;

        _rewardedAdManager.OnReward += OnReward;
    }

    /// <summary>
    /// In RewardedAdController we want to check if user is rewarded and count every reward
    /// </summary>
    private void OnReward(string amount)
    {
        _rewardCounter++;

        if (StatusWindow != null)
        {
            StatusWindow.SetActive(true);
            RewardedCounterText.text = $"Count: {_rewardCounter}";
        }
    }
}
