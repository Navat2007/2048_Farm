using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BasePanelsUI
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _maxScoreText;
    [SerializeField] private Button _pauseButton;

    protected override void Awake()
    {
        if (_pauseButton != null)
        {
            _pauseButton.onClick.AddListener(() =>
            {
                EventBus.GameEvents.OnPause?.Invoke();
            });
        }

        EventBus.GameEvents.OnGameStarted += OnStartGame;
        EventBus.GameEvents.OnGameEnded += OnEndGame;
        EventBus.UIEvents.OnMainMenuWindowShow += OnEndGame;
        EventBus.ScoreEvents.OnScoreChanged += OnScoreChanged;
        EventBus.ScoreEvents.OnMaxScoreChanged += OnMaxScoreChanged;
    }

    private void OnDestroy()
    {
        EventBus.GameEvents.OnGameStarted -= OnStartGame;
        EventBus.GameEvents.OnGameEnded -= OnEndGame;
        EventBus.UIEvents.OnMainMenuWindowShow += OnEndGame;
        EventBus.ScoreEvents.OnScoreChanged -= OnScoreChanged;
        EventBus.ScoreEvents.OnMaxScoreChanged -= OnMaxScoreChanged;
    }

    private void OnStartGame()
    {
        _panel.gameObject.SetActive(true);
    }
    
    private void OnEndGame()
    {
        _panel.gameObject.SetActive(false);
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
