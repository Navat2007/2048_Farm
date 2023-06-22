using UnityEngine;
using UnityEngine.UI;

public class ButtonClickAudio : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private AudioClip _clickClip;

    private void Awake()
    {
        
        if(_button == null)
        {
            _button = GetComponent<Button>();
        }
        
        _button.onClick.AddListener(() =>
        {
            ServiceLocator.AudioManager.PlaySound(_clickClip);
        });
    }
}