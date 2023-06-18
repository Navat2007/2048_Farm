using System.Threading.Tasks;
using Bayat.SaveSystem;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    
    private Save _save;

    public async Task Init()
    {
        _save = await Load();
    }

    public void LoadCurrency()
    {
        Debug.Log($"{_save.GetMaxScore()} max score");
        ServiceLocator.CurrencyManager.SetCurrency(Currency.MAX_SCORE, _save.GetMaxScore());
    }
    
    public async Task AsyncSave()
    {
        _save.SetAllData();
        await SaveSystemAPI.SaveAsync("save.bin", _save);
    }

    public void Save(bool autoSave = false)
    {
        _save.SetAllData();
        SaveSystemAPI.SaveAsync("save.bin", _save);
    }
    
    public async void Reset()
    {
        await SaveSystemAPI.DeleteAsync("save.bin");
    }
    
    private void Awake()
    {
        ServiceLocator.SaveLoadManager = this;
    }
    
    private async Task<Save> Load()
    {
        if (!await SaveSystemAPI.ExistsAsync("save.bin")) 
            return new Save();
        
        return await SaveSystemAPI.LoadAsync<Save>("save.bin");
    }
}

public class Save
{
    [SerializeField] private float _maxScore;
    [SerializeField] private float _time;

    public void SetAllData()
    {
        _maxScore = ServiceLocator.CurrencyManager.GetCurrency(Currency.MAX_SCORE);
        _time = ServiceLocator.Timer.CurrentTime;
    }

    public float GetMaxScore() => _maxScore;
    public float GetTime() => _time;
}