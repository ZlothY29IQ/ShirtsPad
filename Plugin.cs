using System.Collections;
using System.IO;
using System.Reflection;
using BepInEx;
using GorillaShirts.Behaviours;
using GorillaShirts.Models.StateMachine;
using ShirtsPad.Core;
using UnityEngine;

namespace ShirtsPad;

[BepInDependency("dev.gorillashirts", "2.4.1")]
[BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
public class Plugin : BaseUnityPlugin
{
    private GameObject shirtPad;

    private void Awake() =>
            Logger.LogInfo(Constants
                   .Description);

    private void Start() => GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);

    private void OnPlayerSpawned()
    {
        Stream bundleStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShirtsPad.Assets.shirtpad");
        AssetBundle bundle = AssetBundle.LoadFromStream(bundleStream);
        // ReSharper disable once PossibleNullReferenceException
        bundleStream.Close();

        shirtPad                      = Instantiate(bundle.LoadAsset<GameObject>("ShirtsPad"));
        shirtPad.transform.localScale = new Vector3(4.8f, 0.7f, 7f);
        shirtPad.SetActive(false);

        gameObject.AddComponent<InputHandler>();
        InputHandler.ShirtPad = shirtPad;

        StartCoroutine(WaitForShirtsToLoad());
    }

    private IEnumerator WaitForShirtsToLoad()
    {
        while (ShirtManager.Instance == null || ShirtManager.Instance.MenuStateMachine is null ||
               ShirtManager.Instance.MenuStateMachine.CurrentState is Menu_Welcome or Menu_Loading or Menu_Info)
            yield return null;

        gameObject.AddComponent<PadHandler>();
        PadHandler.ShirtPad = shirtPad;
    }
}