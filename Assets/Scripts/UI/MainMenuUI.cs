using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Transform _menuPanel;
    [Space]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _leaderButton;
    [SerializeField] private Button _exitButton;
    
    private void Awake()
    {
        _playButton.onClick.AddListener(() =>
        {
            EventBus.GameEvents.OnGameStarted?.Invoke();
        });
        
        _optionsButton.onClick.AddListener(() =>
        {
            EventBus.UIEvents.OnSettingsWindowShow?.Invoke();
        });
        
        _leaderButton.onClick.AddListener(() =>
        {
            EventBus.UIEvents.OnLeaderBoardWindowShow?.Invoke();
        });
        
        _exitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        });

        EventBus.GameEvents.OnGameStarted += OnStartLevel;
        EventBus.UIEvents.OnMainMenuWindowShow += OnMainMenuWindowShow;
        
        _menuPanel.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        EventBus.GameEvents.OnGameStarted -= OnStartLevel;
        EventBus.UIEvents.OnMainMenuWindowShow -= OnMainMenuWindowShow;
    }

    private void OnStartLevel()
    {
        _menuPanel.gameObject.SetActive(false);
    }
    
    private void OnMainMenuWindowShow()
    {
        _menuPanel.gameObject.SetActive(true);
    }
}