using UnityEngine;
using UnityEngine.EventSystems;

public class OnPointerAudio : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private bool _playUpClip;
    [SerializeField] private bool _playDownClip;
    [Space]
    [SerializeField] private AudioClip _pointerUpClip;
    [SerializeField] private AudioClip _pointerDownClip;

    public void OnPointerUp(PointerEventData eventData)
    {
        if(_playUpClip)
            ServiceLocator.AudioManager.PlaySound(_pointerUpClip);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_playDownClip)
            ServiceLocator.AudioManager.PlaySound(_pointerDownClip);
    }
}