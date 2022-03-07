using System;
using Unity.Services.Mediation;
using UnityEngine;

namespace AdMediation.UnityMediation
{
    /// <summary>
    /// Class <c>UnityMediationInterstitialAd</c> is a class that initializes,
    /// loads and shows the interstitial ad,
    /// implements all the necessary methods from <c>IInterstitialAdManager</c>.
    /// And, also, implements unique for Unity Mediation SDK events like ImpressionEvents
    /// </summary>
    public class UnityMediationInterstitialAd : IInterstitialAdManager
    {
        private string adUnitId;
        private IInterstitialAd ad;

        // There's no obvious way in API to check if an interstitial ad was skipped or not,
        // so I use a timer to determine that. If a user closes the ad and spends less time,
        // then it counts as skipped.
        private int minAdTimer = 30;

        private DateTime snapshotTime;

        public event Action OnLoaded;
        public event Action<string> OnLoadedFailed;
        public event Action OnShowed;
        public event Action<string> OnShowedFailed;
        public event Action<bool> OnClosed;
        public event Action OnClicked;

        public void Initialize(string id)
        {
            adUnitId = id;

            //Create
            ad = MediationService.Instance.CreateInterstitialAd(adUnitId);

            //Subscribe to events
            ad.OnLoaded += Loaded;
            ad.OnFailedLoad += FailedLoad;

            ad.OnShowed += Shown;
            ad.OnFailedShow += FailedShow;
            ad.OnClosed += Closed;
            ad.OnClicked += Clicked;

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

            snapshotTime = DateTime.Now;

            OnShowed?.Invoke();
        }

        private void Closed(object sender, EventArgs e)
        {
            Debug.Log("Ad has closed");

            // Execute logic after an ad has been closed.
            OnClosed?.Invoke((DateTime.Now - snapshotTime).TotalSeconds > minAdTimer);
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


        private void ImpressionEvent(object sender, ImpressionEventArgs args)
        {
            if (args.AdUnitId != adUnitId) return;

            var impressionData = args.ImpressionData != null ? JsonUtility.ToJson(args.ImpressionData, true) : "null";
            Debug.Log($"Impression event from ad unit id {args.AdUnitId} {impressionData}");
        }
    }

}