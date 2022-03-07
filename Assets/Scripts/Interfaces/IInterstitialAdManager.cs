using System;

/// <summary>
/// Interface <c>IInterstitialAdManager</c> should be used in classes
/// for working with interstitial ads without a dependance on a certain mediation
/// </summary>
public interface IInterstitialAdManager { 

    public void Initialize(string id);

    public void Load();

    public bool IsLoaded();

    public void Show();

    public string GetAdInfo();

    public Action OnLoaded { get; set; }
    public Action<string> OnLoadedFailed { get; set; }
    public Action OnShowed { get; set; }
    public Action<string> OnShowedFailed { get; set; }
    public Action<bool> OnClosed { get; set; }
    public Action OnClicked { get; set; }

}
