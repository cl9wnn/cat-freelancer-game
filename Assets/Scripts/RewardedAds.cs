using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static RewardedAds S;

    [SerializeField] private string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] private string _iOSAdUnitId = "Rewarded_iOS";

    private string _adUnitId;
   // public Button adButton;
 //   public Button collectButton;
 //   public Button wheelBttn;
    public bool adBoost = false;
    public bool adCollect = false;
    public bool adWheel = false;
    public Game game;
    public Boost boost;
    public Fortune fort;

    void Awake()
    {
        S = this;

        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSAdUnitId
            : _androidAdUnitId;
        LoadAd();
    }

    // Load content to the Ad Unit:
     void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }
    public void AdBoost()
    {
        adBoost = true;
        Advertisement.Show(_adUnitId, this);
    }
    public void AdCollect()
    {
        adCollect = true;
        Advertisement.Show(_adUnitId, this);
    }
    public void AdCircle()
    {
        adWheel = true;
        Advertisement.Show(_adUnitId, this);
    }

    public void ShowAd()
    {
        Advertisement.Show(_adUnitId, this);
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED) && adCollect == true)
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
                game.offlineBonus *= 2;
                game.Score += game.offlineBonus;
                print(game.offlineBonus);
                game.infoPan.SetActive(false);
                adCollect = false;
        }
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED) && adBoost == true)
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            boost.longTimer = 0;
            boost.canBoostAd = false;
            adBoost = false;
        }
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED) && adWheel == true)
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            fort.longTimer = 0;
            fort.canAd = false;
            adWheel = false;

        }
        Advertisement.Load(_adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
}