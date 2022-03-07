using System;
using Unity.Services.Mediation;
using UnityEngine;
using Unity.Services.Core;

namespace AdMediation.UnityMediation
{
    /// <summary>
    /// Class <c>UnityMediationRewardedAd</c> is a class that initializes,
    /// loads and shows the rewarded ad,
    /// implements all the necessary methods from <c>IRewardedAdManager</c>.
    /// And, also, implements unique for Unity Mediation SDK events like ImpressionEvents
    /// </summary>
    public class UnityMediationRewardedAd : IRewardedAdManager
    {

        private string adUnitId;
        private IRewardedAd ad;

        public event Action OnLoaded;
        public event Action<string> OnLoadedFailed;
        public event Action OnShowed;
        public event Action<string> OnShowedFailed;
        public event Action<bool> OnClosed;
        public event Action OnClicked;
        public event Action<string> OnReward;

        public void Initialize(string id)
        {
            adUnitId = id;

            //Create
            ad = MediationService.Instance.CreateRewardedAd(adUnitId);

            //Subscribe to events
            ad.OnLoaded += Loaded;
            ad.OnFailedLoad += FailedLoad;

            ad.OnShowed += Shown;
            ad.OnFailedShow += FailedShow;
            ad.OnClosed += Closed;
            ad.OnClicked += Clicked;
            ad.OnUserRewarded += UserRewarded;


            // Impression Event
            MediationService.Instance.ImpressionEventPublisher.OnImpression += ImpressionEvent;
        }

        public void Load()
        {
            ad.Load();
        }

        public bool IsLoaded()
        {
            return ad.AdState == AdState.Loaded;
        }

        public string GetAdInfo()
        {
            return "Ad Loaded";
        }

        public void Show()
        {
            if (ad.AdState == AdState.Loaded)
            {
                ad.Show();
            }
        }

        private void Loaded(object sender, EventArgs args)
        {
            Debug.Log("Ad loaded");

            OnLoaded?.Invoke();
        }

        private void FailedLoad(object sender, LoadErrorEventArgs args)
        {
            Debug.Log($"Failed to load ad: {args.Message}");

            OnLoadedFailed?.Invoke(args.Message);
        }

        private void Shown(object sender, EventArgs args)
        {
            Debug.Log("Ad shown!");

            OnShowed?.Invoke();
        }

        private void Closed(object sender, EventArgs e)
        {
            Debug.Log("Ad has closed");

            // Execute logic after an ad has been closed.
            OnClosed?.Invoke(true);
        }

        private void Clicked(object sender, EventArgs e)
        {
            Debug.Log("Ad has been clicked");

            // Execute logic after an ad has been clicked.
            OnClicked?.Invoke();
        }

        private void FailedShow(object sender, ShowErrorEventArgs args)
        {
            Debug.Log(args.Message);

            OnShowedFailed?.Invoke(args.Message);
        }

        private void UserRewarded(object sender, RewardEventArgs e)
        {
            Debug.Log($"Received reward: type:{e.Type}; amount:{e.Amount}");

            OnReward?.Invoke(e.Amount);
        }


        private void ImpressionEvent(object sender, ImpressionEventArgs args)
        {
            if (args.AdUnitId != adUnitId) return;

            var impressionData = args.ImpressionData != null ? JsonUtility.ToJson(args.ImpressionData, true) : "null";
            Debug.Log($"Impression event from ad unit id {args.AdUnitId} {impressionData}");
        }


    }
}
