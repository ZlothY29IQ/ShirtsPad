using System;
using UnityEngine;

namespace ShirtsPad.Components
{
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
        public static InputManager Instance { get; private set; }

        public class ControllerButton
        {
            public bool WasDown;
            public bool Held;
            public bool WasUp;

            private bool lastState;

            public void Update(Func<bool> getInput)
            {
                bool current = getInput();

                WasDown = !lastState && current;
                WasUp = lastState && !current;
                Held = current;

                lastState = current;
            }
        }

        public ControllerButton RightPrimary = new();
        public ControllerButton RightSecondary = new();
        public ControllerButton RightTrigger = new();
        public ControllerButton RightGrip = new();

        public ControllerButton LeftPrimary = new();
        public ControllerButton LeftSecondary = new();
        public ControllerButton LeftTrigger = new();
        public ControllerButton LeftGrip = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            if (ControllerInputPoller.instance == null) return;

            RightPrimary.Update(() => ControllerInputPoller.instance.rightControllerPrimaryButton);
            RightSecondary.Update(() => ControllerInputPoller.instance.rightControllerSecondaryButton);
            RightTrigger.Update(() => ControllerInputPoller.instance.rightControllerTriggerButton);
            RightGrip.Update(() => ControllerInputPoller.instance.rightGrab);

            LeftPrimary.Update(() => ControllerInputPoller.instance.leftControllerPrimaryButton);
            LeftSecondary.Update(() => ControllerInputPoller.instance.leftControllerSecondaryButton);
            LeftTrigger.Update(() => ControllerInputPoller.instance.leftControllerTriggerButton);
            LeftGrip.Update(() => ControllerInputPoller.instance.leftGrab);
        }

        public ControllerButton GetInput(InputType type) => type switch
        {
            InputType.RightPrimary => RightPrimary,
            InputType.RightSecondary => RightSecondary,
            InputType.RightTrigger => RightTrigger,
            InputType.RightGrip => RightGrip,
            InputType.LeftPrimary => LeftPrimary,
            InputType.LeftSecondary => LeftSecondary,
            InputType.LeftTrigger => LeftTrigger,
            InputType.LeftGrip => LeftGrip,
            _ => null
        };
    }
}
