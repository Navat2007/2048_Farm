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
            EventBus.GameEvents.OnUnPause?.Invoke();
            EventBus.GameEvents.OnGameStarted?.Invoke();
        });
        
        _exitButton.onClick.AddListener(() =>
        {
            OnPauseWindowHide();
            
            EventBus.UIEvents.OnMainMenuWindowShow?.Invoke();
        });
        
        EventBus.GameEvents.OnPause += OnPauseWindowShow;
        EventBus.GameEvents.OnUnPause += OnPauseWindowHide;

        OnPauseWindowHide();
    }

    private void OnDestroy()
    {
        EventBus.GameEvents.OnPause -= OnPauseWindowShow;
        EventBus.GameEvents.OnUnPause -= OnPauseWindowHide;
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