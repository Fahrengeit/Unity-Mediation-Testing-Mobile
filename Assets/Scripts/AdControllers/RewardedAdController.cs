using UnityEngine;
using UnityEngine.UI;

namespace AdMediation
{
    /// <summary>
    /// Class <c>RewardedAdController</c> is a class that controls everything a user needs for creating,
    /// loading and showing a rewarded ad in this application.
    /// Interstitial and Rewarded ads are somewhat similar in terms of API, so it only makes sense
    /// to inherit here from InterstitialAdController
    /// </summary>
    public class RewardedAdController : InterstitialAdController
    {
        private IRewardedAdManager rewardedAdManager;

        [SerializeField]        
        private Text rewardedCounterText;
        private int rewardCounter = 0;

        /// <summary>
        /// After ads initialization, we can initialize the ad manager that can
        /// use any mediation as long as every essential method is implemented
        /// </summary>
        public override void Initialize(IInterstitialAdManager adsManager)
        {
            base.Initialize(adsManager);

            rewardedAdManager = adsManager as IRewardedAdManager;

            rewardedAdManager.OnReward += OnReward;
        }

        /// <summary>
        /// In RewardedAdController we want to check if user is rewarded and count every reward
        /// </summary>
        private void OnReward(string amount)
        {
            rewardCounter++;

            if (StatusWindow != null)
            {
                StatusWindow.SetActive(true);
                rewardedCounterText.text = $"Count: {rewardCounter}";
            }
        }

        protected override void OnClosedAd(bool status)
        {
            if (StatusWindow != null)
            {
                StatusWindow.SetActive(true);
                rewardedCounterText.text = $"Count: {rewardCounter}";
            }
            ResetAd();
        }
    }
}
