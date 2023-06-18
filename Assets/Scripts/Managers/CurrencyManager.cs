using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private float _time = 0;
    [SerializeField] private float _score = 0;
    [SerializeField] private float _maxScore = 0;

    public float GetCurrency(Currency currency)
    {
        switch (currency)
        {
            case Currency.TIME:
                return _time;
            case Currency.SCORE:
                return _score;
            case Currency.MAX_SCORE:
                return _maxScore;
            default:
                return 0;
        }
    }

    public void SetCurrency(Currency currency, float value)
    {
        switch (currency)
        {
            case Currency.TIME:
                _time = value;
                break;
            case Currency.SCORE:
                _score = value;
                EventBus.ScoreEvents.OnScoreChanged?.Invoke(_score);
                break;
            case Currency.MAX_SCORE:
                _maxScore = value;
                EventBus.ScoreEvents.OnMaxScoreChanged?.Invoke(_maxScore);
                break;
        }
    }

    private void Awake()
    {
        ServiceLocator.CurrencyManager = this;

        EventBus.GameEvents.OnGameStarted += OnLevelStart;
        EventBus.GameEvents.OnTileMerged += OnTileMerged;
    }

    private void OnDestroy()
    {
        EventBus.GameEvents.OnGameStarted -= OnLevelStart;
        EventBus.GameEvents.OnTileMerged -= OnTileMerged;
    }

    private void OnLevelStart()
    {
        SetCurrency(Currency.SCORE, 0);
    }
    
    private void OnTileMerged(int number)
    {
        if(number > _score)
            SetCurrency(Currency.SCORE, number);
        
        if(number > _maxScore)
            SetCurrency(Currency.MAX_SCORE, number);
    }
}

public enum Currency
{
    TIME,
    SCORE,
    MAX_SCORE
}