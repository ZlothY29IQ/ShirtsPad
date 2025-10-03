using System;
using UnityEngine;

namespace ShirtsPad.Components;

public class PressableButton : MonoBehaviour
{
    public Action OnPress;

    private const float DebounceTime = 0.2f;
    private float touchTime;

    private void Awake() => gameObject.SetLayer(UnityLayer.GorillaInteractable);

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - touchTime < DebounceTime)
            return;

        var hand = other.GetComponentInParent<GorillaTriggerColliderHandIndicator>();
        if (hand != null && !hand.isLeftHand)
        {
            touchTime = Time.time;
            OnPress?.Invoke();
            GorillaTagger.Instance.StartVibration(hand.isLeftHand, 0.2f, 0.2f);
        }
    }
}