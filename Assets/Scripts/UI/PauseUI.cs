using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Transform _pausePanel;
    [Space]
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    
    private void Awake()
    {
        _settingsButton.onClick.AddListener(() =>
        {
            EventBus.UIEvents.OnSettingsWindowShow?.Invoke();
        });
        
        _resumeButton.onClick.AddListener(() =>
        {
            EventBus.UIEvents.OnPauseWindowHide?.Invoke();
            EventBus.GameEvents.OnUnPause?.Invoke();
        });
        
        _restartButton.onClick.AddListener(() =>
        {
            EventBus.UIEvents.OnPauseWindowHide?.Invoke();
            EventBus.GameEvents.OnGameStarted?.Invoke();
        });
        
        _exitButton.onClick.AddListener(() =>
        {
            EventBus.UIEvents.OnPauseWindowHide?.Invoke();
            EventBus.UIEvents.OnMainMenuWindowShow?.Invoke();
        });
        
        EventBus.UIEvents.OnPauseWindowShow += OnPauseWindowShow;
        EventBus.UIEvents.OnPauseWindowHide += OnPauseWindowHide;
        
        _pausePanel.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventBus.UIEvents.OnPauseWindowShow -= OnPauseWindowShow;
    }

    private void OnPauseWindowShow()
    {
        _pausePanel.gameObject.SetActive(true);
    }
    
    private void OnPauseWindowHide()
    {
        _pausePanel.gameObject.SetActive(false);
    }
}