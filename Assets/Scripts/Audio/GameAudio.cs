
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    [SerializeField] private AudioClip _spawnClip;
    [SerializeField] private AudioClip _moveClip;
    [SerializeField] private AudioClip _mergeClip;
    
    private void Awake()
    {
        EventBus.GameEvents.OnTileSpawned += OnSpawned;
        EventBus.GameEvents.OnTileMerged += OnMerge;
        EventBus.GameEvents.OnTileMoved += OnMove;
    }

    private void OnDestroy()
    {
        EventBus.GameEvents.OnTileSpawned -= OnSpawned;
        EventBus.GameEvents.OnTileMerged -= OnMerge;
        EventBus.GameEvents.OnTileMoved -= OnMove;
    }
    
    private void OnSpawned()
    {
        ServiceLocator.AudioManager.PlaySound(_spawnClip);
    }

    private void OnMove()
    {
        ServiceLocator.AudioManager.PlaySound(_moveClip);
    }
    
    private void OnMerge(int index)
    {
        ServiceLocator.AudioManager.PlaySound(_mergeClip);
    }
}
