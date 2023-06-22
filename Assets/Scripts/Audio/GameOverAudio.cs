using UnityEngine;

public class GameOverAudio : MonoBehaviour
{
    [SerializeField] private AudioClip _gameOverClip;
    
    private void Awake()
    {
        EventBus.GameEvents.OnGameEnded += OnEndGame;
    }

    private void OnDestroy()
    {
        EventBus.GameEvents.OnGameEnded -= OnEndGame;
    }

    private void OnEndGame(bool obj)
    {
        ServiceLocator.AudioManager.PlaySound(_gameOverClip);
    }
}
