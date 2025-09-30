using UnityEngine;
using GorillaLocomotion;
using ShirtsPad.Components;

public class GorillaButton : MonoBehaviour
{
    public System.Action onPressed;

    private bool isInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other == GorillaTagger.Instance.rightHandTriggerCollider)
        {
            isInside = true;
            TryPress();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == GorillaTagger.Instance.rightHandTriggerCollider)
        {
            isInside = false;
        }
    }
    

    private void TryPress()
    {
        Debug.Log($"{gameObject.name} pressed!");
        onPressed?.Invoke();
    }
}