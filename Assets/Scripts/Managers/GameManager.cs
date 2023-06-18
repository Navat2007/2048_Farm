using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameState _state = GameState.PAUSE;
    
    public enum GameState
    {
        PLAY,
        PAUSE
    }
    
    public GameState GetState => _state;

    private void Awake()
    {
        ServiceLocator.GameManager = this;
        
        //Time.timeScale = 0;
        
        EventBus.GameEvents.OnGameStarted += OnResume;
        EventBus.GameEvents.OnGameEnded += OnPause;
    }

    private void OnDestroy()
    {
        EventBus.GameEvents.OnGameStarted -= OnResume;
        EventBus.GameEvents.OnGameEnded -= OnPause;
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
        //Time.timeScale = 1;
        _state = GameState.PLAY;
    }

    private void OnPause(bool success)
    {
        //Time.timeScale = 0;
        _state = GameState.PAUSE;
    }

    private async void OnApplicationQuit()
    {
        await ServiceLocator.SaveLoadManager.AsyncSave();
    }
}
