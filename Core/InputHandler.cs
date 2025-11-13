using GorillaLocomotion;
using UnityEngine;

namespace ShirtsPad.Core;

public class InputHandler : MonoBehaviour
{
    public static GameObject ShirtPad;

    private bool
            wasLeftControllerPrimaryButtonPressed;

    private void Update()
    {
        bool isLeftControllerPrimaryButtonPressed = ControllerInputPoller.instance.leftControllerPrimaryButton;
        if (isLeftControllerPrimaryButtonPressed && !wasLeftControllerPrimaryButtonPressed)
            ShirtPad.SetActive(!ShirtPad.activeSelf);

        wasLeftControllerPrimaryButtonPressed = isLeftControllerPrimaryButtonPressed;
    }

    private void LateUpdate()
    {
        if (!ShirtPad.activeSelf) return;

        ShirtPad.transform.position = GTPlayer.Instance.LeftHand.controllerTransform.TransformPoint(0.015f, -0.05f, -0.025f);
        ShirtPad.transform.rotation =
                GTPlayer.Instance.LeftHand.controllerTransform.rotation * Quaternion.Euler(325f, 10f, 85f);
    }
}