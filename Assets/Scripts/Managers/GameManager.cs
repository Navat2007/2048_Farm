using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameState _state = GameState.PAUSE;
    
    public enum GameState
    {
        PLAY,
        PAUSE,
        MENU
    }
    
    public GameState GetState => _state;

    private void Awake()
    {
        ServiceLocator.GameManager = this;
        
        //Time.timeScale = 0;
        
        EventBus.GameEvents.OnGameStarted += OnResume;
        EventBus.GameEvents.OnGameEnded += OnGameEnd;
        EventBus.GameEvents.OnPause += OnPause;
        EventBus.GameEvents.OnUnPause += OnResume;
    }

    private void OnDestroy()
    {
        EventBus.GameEvents.OnGameStarted -= OnResume;
        EventBus.GameEvents.OnGameEnded -= OnGameEnd;
        EventBus.GameEvents.OnPause -= OnPause;
        EventBus.GameEvents.OnUnPause -= OnResume;
    }

    private async void Start()
    {
        Application.targetFrameRate = -1;
        
        ServiceLocator.CheckServices();
        
        await ServiceLocator.SaveLoadManager.Init();
        ServiceLocator.SaveLoadManager.LoadCurrency();
    }

    private void OnResume()
    {
        _state = GameState.PLAY;
    }
    
    private void OnPause()
    {
        _state = GameState.PAUSE;
    }
    
    private void OnGameEnd(bool success)
    {
        _state = GameState.MENU;
    }

    private async void OnApplicationQuit()
    {
        await ServiceLocator.SaveLoadManager.AsyncSave();
    }
}
