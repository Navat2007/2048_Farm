using UnityEngine;
using UnityEngine.UI;

public class BasePanelsUI : MonoBehaviour
{
    [SerializeField] protected Transform _panel;
    [SerializeField] protected Button _closeButton;

    protected virtual void Awake()
    {
        if (_closeButton)
        {
            _closeButton.onClick.AddListener(() =>
            {
                _panel.gameObject.SetActive(false);
            });
        }
    }

    protected virtual void Start()
    {
        _panel.gameObject.SetActive(false);
    }
}
