using UnityEngine;

public class Timer : MonoBehaviour
{
    private float _currentTime;
    
    public float CurrentTime => _currentTime;

    private void Awake()
    {
        ServiceLocator.Timer = this;
    }
    
    private void Start()
    {
        EventBus.GameEvents.OnGameStarted += Reset;
    }

    private void OnDestroy()
    {
        EventBus.GameEvents.OnGameStarted -= Reset;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        EventBus.UIEvents.OnTimerChanged?.Invoke(_currentTime);
    }

    private void Reset()
    {
        _currentTime = 0;
    }
}