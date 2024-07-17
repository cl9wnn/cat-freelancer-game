using UnityEngine;
using UnityEngine.UI;

public class PopupText : MonoBehaviour
{
    [SerializeField] private Text _numberText;
    
    private ObjectPool _pool;

    private Game _game;
    private Boost _boost;
    private Fortune _fortune;

    private void Awake()
    {
        _pool = GameSingleton.Instance.Pool;

        _game = GameSingleton.Instance.Game;
        _boost = GameSingleton.Instance.Boost;
        _fortune = GameSingleton.Instance.Fortune;
    }

    private void OnEnable()
    {
        _numberText.text = "+" + StringMethods.FormatMoney(_game.ScoreIncrease * GetMultiplier());

        if (_boost.BoostOn || _fortune.coffeeRewarded)
        {
            _numberText.color = new Color(255, 215, 0);
        }
    }

    private int GetMultiplier()
    {
        return (_boost.BoostOn || _fortune.coffeeRewarded) ? 3 : 1;
    }

    public void ReturnPopupTextToPool()
    {
        _pool.ReturnToPool(gameObject);
    }
}
