using System;

/// <summary>
/// Interface <c>IRewardedAdManager</c> should be used in classes
/// for working with interstitial ads without a dependance on a certain mediation.
/// As rewarded ads are similar to interstitial, interface inherits from <c>IInterstitialAdManager</c>
/// </summary>
public interface IRewardedAdManager : IInterstitialAdManager
{
    public Action<string> OnReward { get; set; }
}
