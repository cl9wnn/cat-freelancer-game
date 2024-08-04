using UnityEngine;
using YG;

public class RewardedAds : MonoBehaviour
{
    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += GetRewardForAds;
    }
    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= GetRewardForAds;
    }

    private void GetRewardForAds(int id)
    {
        
        switch (id)
        {
            case 0: // Колесо фортуны
                GameSingleton.Instance.Fortune.UnlockFortuneWheel();
                break;
            case 1: // Стакан
                GameSingleton.Instance.Boost.AddCoffee(amount: 1);
                break;
            case 2: // Удвоение награды за отсутствие 
                GameSingleton.Instance.Game.GetOfflineIncomeWithMultiplier(2);
                break;
            default:

                break;
        }
    }
}