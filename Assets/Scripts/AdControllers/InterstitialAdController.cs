using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace AdMediation
{
    /// <summary>
    /// Class <c>InterstitialAdController</c> is a class that controls everything a user needs for creating,
    /// loading and showing an interstitial ad in this application.
    /// </summary>
    public class InterstitialAdController : MonoBehaviour
    {
        private IInterstitialAdManager interstitialAdManager;

        [SerializeField]
        private string adUnitId_iOS;
        [SerializeField]
        private string adUnitId_Android;

        [SerializeField]
        private Button loadButton;
        [SerializeField]
        private Text loadStatus;
        [SerializeField]
        private Button showButton;
        [SerializeField]
        private Text showInfo;

        public GameObject StatusWindow;
        [SerializeField]
        private Text statusText;

        /// <summary>
        /// In Awake we add listeners to UI buttons, so we can load and show an ad
        /// And make sure that buttons are disabled, because a user shouldn't be able to press them
        /// before ads are initialized
        /// </summary>
        private void Awake()
        {
            if (loadButton == null || loadStatus == null || showButton == null || showInfo == null)
                throw new NullReferenceException("UI fields must be assigned");

            loadButton.onClick.AddListener(LoadAd);
            showButton.onClick.AddListener(ShowAd);

            loadButton.interactable = false;
            showButton.interactable = false;

            loadStatus.text = "Not Loaded";
            showInfo.text = "No Ad";
        }

        /// <summary>
        /// After ads initialization, we can initialize the ad manager that can
        /// use any mediation as long as every essential method is implemented
        /// </summary>
        public virtual void Initialize(IInterstitialAdManager adManager)
        {
            interstitialAdManager = adManager;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    interstitialAdManager.Initialize(adUnitId_Android);
                    break;
                case RuntimePlatform.IPhonePlayer:
                    interstitialAdManager.Initialize(adUnitId_iOS);
                    break;
                default:
#if UNITY_EDITOR
                    interstitialAdManager.Initialize(adUnitId_iOS);
#else
                        return;
#endif
                    break;
            }
            interstitialAdManager.OnLoaded += OnLoadedAd;
            interstitialAdManager.OnClosed += OnClosedAd;

            loadButton.interactable = true;
        }

        protected virtual void LoadAd()
        {
            loadStatus.text = "Loading...";
            interstitialAdManager.Load();
        }

        protected virtual void OnLoadedAd()
        {
            loadStatus.text = "Loaded";

            // Only after ad is loaded we unblock the ShowButton, so a user couldn't
            // try to show a not loaded ad
            showButton.interactable = true;

            showInfo.text = interstitialAdManager.GetAdInfo() ?? "";
        }

        protected virtual void ShowAd()
        {
            interstitialAdManager.Show();
        }

        protected virtual void OnClosedAd(bool status)
        {
            if (StatusWindow != null)
            {
                StatusWindow.SetActive(true);
            }
            if (statusText != null)
            {
                statusText.text = status ? "Ad watched completely" : "Ad skipped";
            }
            ResetAd();
        }

        protected virtual void ResetAd()
        {
            showButton.interactable = false;
            loadStatus.text = "Not Loaded";
            showInfo.text = "No Ad";
        }

        private void OnDestroy()
        {
            loadButton.onClick.RemoveListener(LoadAd);
            showButton.onClick.RemoveListener(ShowAd);
        }

    }
}
