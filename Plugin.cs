using System.Reflection;
using BepInEx;
using GorillaLocomotion;
using ShirtsPad.Components;
using UnityEngine;
using TMPro;
using GorillaShirts;
using GorillaShirts.Behaviours.Cosmetic;

namespace ShirtsPad
{

	[BepInDependency("dev.gorillashirts")]
	[BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
	public class Plugin : BaseUnityPlugin
	{
		public GameObject ShirtPad;
		
		//Buttons
		public GameObject back;
		public GameObject previous;
		public GameObject next;
		public GameObject equip;
		
		//Text
		public TextMeshPro shirtName;
		public TextMeshPro shirtCreator;
		public TextMeshPro equipText;
		
		
		private InputManager _inputManager;

		private void Awake() => Logger.LogInfo(Constants.Description);

		private void Start() => GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);

		private void OnPlayerSpawned()
		{
			if (ShirtPad == null)
			{
				Logger.LogInfo("Loading ShirtPad asset...");
				ShirtPad = Instantiate(InitialiseShirtPad("ShirtsPad.Assets.shirtpad")
					.LoadAsset<GameObject>("ShirtsPad"));
				ShirtPad.transform.position = Vector3.zero;
				ShirtPad.transform.rotation = Quaternion.identity;
				ShirtPad.transform.localScale = new Vector3(5f, 0.7f, 7f);
				ShirtPad.transform.parent = GTPlayer.Instance.leftControllerTransform;
				
				foreach (Collider col in ShirtPad.GetComponentsInChildren<Collider>())
					col.enabled = false;
			}
			
			back = ShirtPad.transform.Find("Back").gameObject;
			previous = ShirtPad.transform.Find("Previous").gameObject;
			next = ShirtPad.transform.Find("Next").gameObject;
			equip = ShirtPad.transform.Find("Equip").gameObject;
			shirtName = ShirtPad.transform.Find("ShirtName").GetComponent<TextMeshPro>();
			shirtCreator  = ShirtPad.transform.Find("ShirtCreator").GetComponent<TextMeshPro>();
			equipText  = ShirtPad.transform.Find("Equip/Text").GetComponent<TextMeshPro>();
			
			back.AddComponent<GorillaButton>().onPressed = () => { Logger.LogInfo("Back Button Pressed"); };
			previous.AddComponent<GorillaButton>().onPressed = () => { };
			next.AddComponent<GorillaButton>().onPressed = () => { };
			equip.AddComponent<GorillaButton>().onPressed = () => { };

			
			ShirtPad.AddComponent<InputManager>();
			_inputManager = GetComponent<InputManager>();
		}

		private void Update()
		{
			if (ShirtPad == null)
				return;
			
			if (_inputManager.LeftSecondary.WasPressed)
				ShirtPad.SetActive(!ShirtPad.activeSelf);

			if (ShirtPad.activeSelf)
			{
				var descriptor = FindObjectOfType<GorillaShirts.Behaviours.Cosmetic.ShirtDescriptor>();
				if (descriptor != null)
				{
					string shirtName = descriptor.ShirtName;
					string author = descriptor.Author;
				}
				
				var shirtStandStatus = FindObjectOfType<GorillaShirts.Behaviours.UI.Stand>();
				if (shirtStandStatus != null)
				{
					string equipText = shirtStandStatus.shirtStatusText.ToString();
				}

			}

		}

		public AssetBundle InitialiseShirtPad(string path)
		{
			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
			return AssetBundle.LoadFromStream(stream);
		}
	}
}
