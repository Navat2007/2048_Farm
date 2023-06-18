using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Transform _panel;
    [SerializeField] private Transform _winGamePanel;
    [SerializeField] private Transform _loseGamePanel;
    [SerializeField] private TMP_Text _scoreText;
    
    public void CloseButton()
    {
        EventBus.UIEvents.OnMainMenuWindowShow?.Invoke();
        
        HidePanels();
    }
    
    public void RestartButton()
    {
        EventBus.GameEvents.OnGameStarted?.Invoke();
    }
    
    private void Awake()
    {
        EventBus.GameEvents.OnGameStarted += HidePanels;
        EventBus.GameEvents.OnGameEnded += OnEndGame;
    }

    private void OnDestroy()
    {
        EventBus.GameEvents.OnGameStarted -= HidePanels;
        EventBus.GameEvents.OnGameEnded -= OnEndGame;
    }

    private void Start()
    {
        HidePanels();
    }

    private void HidePanels()
    {
        _panel.gameObject.SetActive(false);
        _winGamePanel.gameObject.SetActive(false);
        _loseGamePanel.gameObject.SetActive(false);
    }
    
    private void OnEndGame(bool success)
    {
        _panel.gameObject.SetActive(true);
        
        if (success)
        {
            _winGamePanel.gameObject.SetActive(true);
        }
        else
        {
            _loseGamePanel.gameObject.SetActive(true);
            _scoreText.SetText(ServiceLocator.CurrencyManager.GetCurrency(Currency.SCORE).ToString());
        }
    }
}
