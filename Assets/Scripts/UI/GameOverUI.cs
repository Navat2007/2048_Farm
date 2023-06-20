using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Transform _panel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TMP_Text _scoreText;
    
    private void Awake()
    {
        _restartButton.onClick.AddListener(() =>
        {
            EventBus.GameEvents.OnGameStarted?.Invoke();
        });
        
        _exitButton.onClick.AddListener(() =>
        {
            EventBus.UIEvents.OnMainMenuWindowShow?.Invoke();
        
            HidePanels();
        });
        
        EventBus.GameEvents.OnGameStarted += HidePanels;
        EventBus.GameEvents.OnGameEnded += OnEndGame;
        
        HidePanels();
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
    }
    
    private void OnEndGame(bool success)
    {
        _panel.gameObject.SetActive(true);
        
        _scoreText.SetText(ServiceLocator.CurrencyManager.GetCurrency(Currency.SCORE).ToString());
    }
}
