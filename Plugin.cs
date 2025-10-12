using System.Collections;
using System.IO;
using System.Reflection;
using BepInEx;
using ShirtsPad.Core;
using UnityEngine;
using GorillaShirts.Behaviours;


namespace ShirtsPad;

[BepInDependency("dev.gorillashirts")]
[BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
public class Plugin : BaseUnityPlugin
{
    private GameObject shirtPad;
    private void Awake() => Logger.LogInfo(Constants.Description); // zlothy no likey me remove this, in his own words "nooooo i need my description"
    private void Start() => GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);

    private void OnPlayerSpawned()
    {
        Stream bundleStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShirtsPad.Assets.shirtpad");
        AssetBundle bundle = AssetBundle.LoadFromStream(bundleStream);
        // ReSharper disable once PossibleNullReferenceException
        bundleStream.Close();
        
        shirtPad = Instantiate(bundle.LoadAsset<GameObject>("ShirtsPad"));
        shirtPad.transform.localScale = new Vector3(4.8f, 0.7f, 7f);
        shirtPad.SetActive(false);
        
        gameObject.AddComponent<InputHandler>();
        InputHandler.ShirtPad = shirtPad;

        StartCoroutine(WaitForShirtsToLoad());
    }
    
    private IEnumerator WaitForShirtsToLoad()
    {
        while (!ShirtManager.HasInstance || ShirtManager.Instance.MenuStateMachine.CurrentState is GorillaShirts.Models.StateMachine.Menu_Loading)
            yield return null;
        
        gameObject.AddComponent<PadHandler>();
        PadHandler.ShirtPad = shirtPad;
    }
}