using TMPro;
using UnityEngine;

public class GameUI : BasePanelsUI
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _maxScoreText;

    public void NewGame()
    {
        EventBus.GameEvents.OnGameStarted?.Invoke();
    }

    protected override void Awake()
    {
        EventBus.GameEvents.OnGameStarted += OnStartGame;
        EventBus.GameEvents.OnGameEnded += OnEndGame;
        EventBus.ScoreEvents.OnScoreChanged += OnScoreChanged;
        EventBus.ScoreEvents.OnMaxScoreChanged += OnMaxScoreChanged;
    }

    private void OnDestroy()
    {
        EventBus.GameEvents.OnGameStarted -= OnStartGame;
        EventBus.GameEvents.OnGameEnded -= OnEndGame;
        EventBus.ScoreEvents.OnScoreChanged -= OnScoreChanged;
        EventBus.ScoreEvents.OnMaxScoreChanged -= OnMaxScoreChanged;
    }

    private void OnStartGame()
    {
        _panel.gameObject.SetActive(true);
    }
    
    private void OnEndGame(bool obj)
    {
        _panel.gameObject.SetActive(false);
    }
    
    private void OnScoreChanged(float score)
    {
        _scoreText.SetText($"{score}");
    }

    private void OnMaxScoreChanged(float score)
    {
        _maxScoreText.SetText($"{score}");
    }
}
