using GorillaShirts.Behaviours;
using GorillaShirts.Models;
using GorillaShirts.Models.UI;
using UnityEngine;

namespace ShirtsPad.Components;

public class PressableButton : MonoBehaviour
{
    private const float       DebounceTime = 0.2f;
    public        EButtonType ButtonType;
    private       float       touchTime;

    private void Awake() => gameObject.SetLayer(UnityLayer.GorillaInteractable);

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - touchTime < DebounceTime)
            return;

        GorillaTriggerColliderHandIndicator hand = other.GetComponentInParent<GorillaTriggerColliderHandIndicator>();
        if (hand != null && !hand.isLeftHand)
        {
            touchTime = Time.time;

            ShirtManager.Instance.MenuStateMachine.CurrentState.OnButtonPress(ButtonType);
            GorillaTagger.Instance.offlineVRRig.rightHandPlayer.GTPlayOneShot(ShirtManager.Instance.Audio[EAudioType.ButtonPress], 0.35f);
            GorillaTagger.Instance.StartVibration(hand.isLeftHand, 0.2f, 0.2f);
        }
    }
}