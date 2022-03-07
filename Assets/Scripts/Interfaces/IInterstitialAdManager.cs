using System;

namespace AdMediation
{
    /// <summary>
    /// Interface <c>IInterstitialAdManager</c> should be used in classes
    /// for working with interstitial ads without a dependance on a certain mediation
    /// </summary>
    public interface IInterstitialAdManager
    {
        /// <summary>
        /// Used to initialize the ad
        /// </summary>
        /// <param name="id">Ad ID</param>
        public void Initialize(string id);

        /// <summary>
        /// Used to load the ad
        /// </summary>
        public void Load();

        /// <summary>
        /// Used to check if the ad is loaded
        /// </summary>
        public bool IsLoaded();

        /// <summary>
        /// Used to show the ad
        /// </summary>
        public void Show();

        /// <summary>
        /// Used to return the state of the ad
        /// </summary>
        public string GetAdInfo();

        public event Action OnLoaded;
        public event Action<string> OnLoadedFailed;
        public event Action OnShowed;
        public event Action<string> OnShowedFailed;
        public event Action<bool> OnClosed;
        public event Action OnClicked;

    }
}
