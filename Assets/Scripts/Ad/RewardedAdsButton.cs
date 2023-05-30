using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using My;
using static Define;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] public Button _showSkillReSelectAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // Disable the button until the ad is ready to show:
        _showSkillReSelectAdButton.interactable = true;
    }

    void Start()
    {
        if (gameObject.name == "AdsSkillSelectBtn" && GameManager.GMInstance.CurrentChar == ECharacterType.WizardChar)
        {
            GameManager.GMInstance.RewardedAdsButtonRef = this;
        }
        else if (gameObject.name == "AdsAcherSkillSelectBtn" && GameManager.GMInstance.CurrentChar == ECharacterType.AcherChar)
        {
            GameManager.GMInstance.RewardedAdsButtonRef = this;
        }
    }


    // Call this public method when you want to get an ad ready to show.
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            _showSkillReSelectAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            _showSkillReSelectAdButton.interactable = true;
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Disable the button:
        _showSkillReSelectAdButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");

            if (gameObject.name == "MagicStoneRewardButton")
            {
                /** 마정석 150 추가 */
                GameManager.GMInstance.MagicStone += 150;
                /** 마정석 텍스트 수량 초기화 */
                GameManager.GMInstance.LobbySceneManagerRef.MagicStonText.text = GameManager.GMInstance.MagicStone.ToString();

                GameManager.GMInstance.CoinManagerRef.JsonSave();
            }
            else if (gameObject.name == "EnterTicketRewardBtn")
            {
                /** 입장권 1 추가 */
                GameManager.GMInstance.EnterTicket++;
                /** 입장권 텍스트 수량 초기화 */
                GameManager.GMInstance.LobbySceneManagerRef.EnterTicketText.text = "x " + GameManager.GMInstance.EnterTicket.ToString();

                GameManager.GMInstance.CoinManagerRef.JsonSave();
            }
            else if (gameObject.name == "AdsSkillSelectBtn")
            {    
                GameManager.GMInstance.UiLevelUp.Next();
            }
            else if(gameObject.name == "EquipRewardBtn")
            {
                Debug.Log("You've gained 1 EquipBox");
            }
            else if (gameObject.name == "AdsAcherSkillSelectBtn")
            {
                GameManager.GMInstance.AcherLevelUpRef.Next();
            }
            else if (gameObject.name == "GetGoldAdsBtn")
            {
                /** 마정석 2배 획득 */
                GameManager.GMInstance.MagicStone += GameManager.GMInstance.PlaySceneManagerRef.GetGameEndMagicStone();
                /** 텍스트 초기화 */
                GameManager.GMInstance.PlaySceneManagerRef.GameClearMagicStoneTxt.text = GameManager.GMInstance.PlaySceneManagerRef.GetGameEndMagicStone() * 2 + " 마정석";

                GameManager.GMInstance.CoinManagerRef.JsonSave();
            }
            else if (gameObject.name == "GetOverGoldAdsBtn")
            {
                /** 마정석 2배 획득 */
                GameManager.GMInstance.MagicStone += GameManager.GMInstance.PlaySceneManagerRef.GetGameEndMagicStone();
                
                /** 텍스트 초기화 */
                GameManager.GMInstance.PlaySceneManagerRef.GameOverMagicStoneTxt.text = GameManager.GMInstance.PlaySceneManagerRef.GetGameEndMagicStone() * 2 + " 마정석";
                
                GameManager.GMInstance.CoinManagerRef.JsonSave();
            }
            // Grant a reward.
        }
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

    void OnDestroy()
    {
        // Clean up the button listeners:
        _showSkillReSelectAdButton.onClick.RemoveAllListeners();
    }
}