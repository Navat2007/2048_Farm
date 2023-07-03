using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public void SaveFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    
    public int LoadInt(string key)
    {
        return PlayerPrefs.GetInt(key, 0);
    }
    
    public float LoadFloat(string key)
    {
        return PlayerPrefs.GetFloat(key, 0);
    }
    
    private void Awake()
    {
        ServiceLocator.SaveLoadManager = this;
        
        EventBus.ScoreEvents.OnMaxScoreChanged += OnMaxScoreChanged;
    }

    private void OnDestroy()
    {
        EventBus.ScoreEvents.OnMaxScoreChanged -= OnMaxScoreChanged;
    }
    
    private void OnMaxScoreChanged(float value)
    {
        SaveFloat("score", value);
    }
}