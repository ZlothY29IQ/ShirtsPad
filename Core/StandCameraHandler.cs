using UnityEngine;

namespace ShirtsPad.Core;

public class StandCameraHandler : MonoBehaviour
{
    private readonly Vector3 standCameraOffset = new(0f, 0f, 0.8f);

    private Camera    standCamera;
    private Transform target;

    private void Update()
    {
        if (!PadHandler.ShirtPad.activeSelf) return;

        if (standCamera == null)
        {
            GameObject shirtCharacterObj = GameObject.Find("GorillaShirts/Shirt Stand/Character");
            if (shirtCharacterObj != null) CreateStandCamera(shirtCharacterObj.transform);
        }

        if (standCamera != null && target != null)
        {
            standCamera.transform.position = Vector3.Lerp(standCamera.transform.position,
                    target.TransformPoint(standCameraOffset), Time.deltaTime * 5f);

            standCamera.transform.LookAt(target.position);
        }
    }

    private void CreateStandCamera(Transform targetObject)
    {
        target = targetObject;

        GameObject camObj = new("ShirtCam");
        standCamera               = camObj.AddComponent<Camera>();
        standCamera.nearClipPlane = 0.01f;

        RenderTexture rt = new(712, 512, 16);
        standCamera.targetTexture = rt;

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = rt;
        renderer.material.color       = Color.white;
    }
}