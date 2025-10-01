using System;
using UnityEngine;

namespace ShirtsPad.Components
{
    public class GorillaButton : MonoBehaviour
    {
        public Action onPressed;
        public string button;
        private float touchTime = 0f;
        private const float debounceTime = 0.25f;
        private float lastPressTime;

        public ButtonPresser buttonPresser;

        void OnTriggerEnter(Collider other)
        {
            if (touchTime + debounceTime >= Time.time) return;
            if (other.TryGetComponent(out GorillaTriggerColliderHandIndicator component))
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(211, component.isLeftHand, 0.12f);
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                TryPress();
            }
        }

        private void TryPress()
        {
            if (Time.time - lastPressTime < 0.3f)
                return;

            lastPressTime = Time.time;

            Debug.Log($"{gameObject.name} pressed!");
            onPressed?.Invoke();
        }
    }
}