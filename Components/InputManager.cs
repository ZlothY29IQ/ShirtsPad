using UnityEngine;

namespace ShirtsPad.Components;

public enum InputType
{
    RightPrimary,
    RightSecondary,
    RightTrigger,
    RightGrip,
    LeftPrimary,
    LeftSecondary,
    LeftTrigger,
    LeftGrip,
}

public class InputManager : MonoBehaviour
{
    public struct ControllerButton
    {
        public bool IsPressed;
        public bool WasPressed;

        public bool IsReleased;
        public bool WasReleased;
    }

    public ControllerButton RightPrimary, RightSecondary, RightTrigger, RightGrip;
    public ControllerButton LeftPrimary, LeftSecondary, LeftTrigger, LeftGrip;

    public ControllerButton GetInput(InputType inputType)
    {
        switch (inputType)
        {
            case InputType.RightPrimary:
                return RightPrimary;
            case InputType.RightSecondary:
                return RightSecondary;
            case InputType.RightTrigger:
                return RightTrigger;
            case InputType.RightGrip:
                return RightGrip;
            case InputType.LeftPrimary:
                return LeftPrimary;
            case InputType.LeftSecondary:
                return LeftSecondary;
            case InputType.LeftTrigger:
                return LeftTrigger;
            case InputType.LeftGrip:
                return LeftGrip;
            default:
                return default;
        }
    }

    private void HandleInput(ref ControllerButton button, bool isPressed)
    {
        bool wasPressed = button.IsPressed;
        button.IsPressed = isPressed;
        button.IsReleased = !isPressed;

        if (wasPressed && !isPressed)
            button.WasReleased = true;
        else
            button.WasReleased = false;

        if (!wasPressed && isPressed)
            button.WasPressed = true;
        else
            button.WasPressed = false;
    }

    private void Update()
    {
        HandleInput(ref RightPrimary, ControllerInputPoller.instance.rightControllerPrimaryButton);
        HandleInput(ref RightSecondary, ControllerInputPoller.instance.rightControllerSecondaryButton);
        HandleInput(ref RightTrigger, ControllerInputPoller.instance.rightControllerTriggerButton);
        HandleInput(ref RightGrip, ControllerInputPoller.instance.rightGrab);
        HandleInput(ref LeftPrimary, ControllerInputPoller.instance.leftControllerPrimaryButton);
        HandleInput(ref LeftSecondary, ControllerInputPoller.instance.leftControllerSecondaryButton);
        HandleInput(ref LeftTrigger, ControllerInputPoller.instance.leftControllerTriggerButton);
        HandleInput(ref LeftGrip, ControllerInputPoller.instance.leftGrab);
    }
}