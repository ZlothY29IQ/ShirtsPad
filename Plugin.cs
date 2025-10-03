using System;
using System.Reflection;
using BepInEx;
using GorillaLocomotion;
using ShirtsPad.Components;
using UnityEngine;
using TMPro;
using GorillaShirts;
using GorillaShirts.Behaviours;
using GorillaShirts.Behaviours.Cosmetic;
using GorillaShirts.Behaviours.UI;
using GorillaShirts.Models.StateMachine;
using GorillaShirts.Models.UI;
using HarmonyLib;

namespace ShirtsPad
{
    [BepInDependency("dev.gorillashirts")]
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public GameObject ShirtPad;
        public GameObject back;
        public GameObject previous;
        public GameObject next;
        public GameObject equip;
        public GameObject shirtsView;
        public TextMeshPro shirtName;
        public TextMeshPro shirtCreator;
        public TextMeshPro equipText;
        public TextMeshPro versionText;

        private TextMeshProUGUI shirtNameUI;
        private TextMeshProUGUI shirtCreatorUI;
        private TextMeshProUGUI equipTextUI;
        
        public Camera _shirtCamera;
        public Transform target;


        public static Plugin Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            Logger.LogInfo(Constants.Description);
        }

        private void Start() => GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);

        private void OnPlayerSpawned()
        {
            if (ShirtPad == null)
            {
                ShirtPad = Instantiate(InitialiseShirtPad("ShirtsPad.Assets.shirtpad")
                    .LoadAsset<GameObject>("ShirtsPad"));
                //ShirtPad.transform.SetParent(GTPlayer.Instance.leftControllerTransform, false);
                ShirtPad.transform.localScale = new Vector3(4.8f, 0.7f, 7f);
                ShirtPad.transform.localRotation = Quaternion.Euler(325f, 10f, 85f);
                ShirtPad.transform.localPosition = new Vector3(0.015f, -0.05f, -0.025f);
                ShirtPad.SetActive(false);
            }

            back = ShirtPad.transform.Find("Back").gameObject;
            previous = ShirtPad.transform.Find("Previous").gameObject;
            next = ShirtPad.transform.Find("Next").gameObject;
            equip = ShirtPad.transform.Find("Equip").gameObject;

            back.layer = 18;
            previous.layer = 18;
            next.layer = 18;
            equip.layer = 18;

            shirtsView = ShirtPad.transform.Find("ShirtsView").gameObject;
            shirtName = ShirtPad.transform.Find("ShirtName").GetComponent<TextMeshPro>();
            shirtCreator = ShirtPad.transform.Find("ShirtCreator").GetComponent<TextMeshPro>();
            equipText = ShirtPad.transform.Find("Equip/Text").GetComponent<TextMeshPro>();
            versionText = ShirtPad.transform.Find("Credits/Version").GetComponent<TextMeshPro>();
            versionText.text = $"Version: {Constants.Version}";
            

            back.AddComponent<PressableButton>().OnPress = () => { PressButtonHelper(EButtonType.Return); };
            previous.AddComponent<PressableButton>().OnPress = () => { PressButtonHelper(EButtonType.NavigateDecrease); };
            next.AddComponent<PressableButton>().OnPress = () => { PressButtonHelper(EButtonType.NavigateIncrease); };
            equip.AddComponent<PressableButton>().OnPress = () => { PressButtonHelper(EButtonType.NavigateSelect); };

            
            GameObject componentHolder = new GameObject("ComponentHolder");
            componentHolder.AddComponent<InputManager>();
        }

        private void LateUpdate()
        {
            if (ShirtPad.activeSelf)
            {
                if (GTPlayer.Instance.leftControllerTransform != null)
                {
                    ShirtPad.transform.position =
                        GTPlayer.Instance.leftControllerTransform.TransformPoint(0.015f, -0.05f, -0.025f);
                    ShirtPad.transform.rotation = GTPlayer.Instance.leftControllerTransform.rotation *
                                                  Quaternion.Euler(325f, 10f, 85f);
                }
            }
        }

        private void Update()
        {
            if (ShirtPad == null)
                return;

            if (_shirtCamera == null)
            {
                GameObject shirtCharacterObj = GameObject.Find("GorillaShirts/Shirt Stand/Character");
                if (shirtCharacterObj != null)
                    CreateShirtCam(shirtCharacterObj.transform);
            }

            if (InputManager.Instance.LeftPrimary.WasDown)
                ShirtPad.SetActive(!ShirtPad.activeSelf);

            if (ShirtPad.activeSelf)
            {
                
                if (_shirtCamera != null && target != null)
                {
                    Vector3 localOffset = new Vector3(0f, 0f, 0.8f);

                    Vector3 desiredPos = target.TransformPoint(localOffset);

                    _shirtCamera.transform.position =
                        Vector3.Lerp(_shirtCamera.transform.position, desiredPos, Time.deltaTime * 5f);

                    _shirtCamera.transform.LookAt(target.position);
                }
                
                if (shirtNameUI == null)
                {
                    GameObject nameObj =
                        GameObject.Find("GorillaShirts/Shirt Stand/UI/MainMenu/MainContainer/Text/Item/Main");
                    if (nameObj != null) shirtNameUI = nameObj.GetComponent<TextMeshProUGUI>();
                }

                if (shirtCreatorUI == null)
                {
                    GameObject bodyObj =
                        GameObject.Find("GorillaShirts/Shirt Stand/UI/MainMenu/MainContainer/Text/Item/Body");
                    if (bodyObj != null) shirtCreatorUI = bodyObj.GetComponent<TextMeshProUGUI>();
                }

                if (equipTextUI == null)
                {
                    GameObject equipObj =
                        GameObject.Find("GorillaShirts/Shirt Stand/UI/MainMenu/MainContainer/Buttons/ShirtEquip/Equip");
                    if (equipObj != null) equipTextUI = equipObj.GetComponent<TextMeshProUGUI>();
                }

                if (shirtNameUI != null) shirtName.text = shirtNameUI.text;
                if (shirtCreatorUI != null) shirtCreator.text = shirtCreatorUI.text;
                if (equipTextUI != null) equipText.text = equipTextUI.text;
            }
        }

        private void PressButtonHelper(EButtonType buttonType)
        {
            ShirtManager.Instance.MenuStateMachine.CurrentState.OnButtonPress(buttonType);
        }
        

        public void CreateShirtCam(Transform targetObject)
        {
            target = targetObject;
            GameObject camObj = new GameObject("ShirtCam");
            _shirtCamera = camObj.AddComponent<Camera>();
            RenderTexture rt = new RenderTexture(712, 512, 16);
            _shirtCamera.targetTexture = rt;
            Renderer rend = shirtsView.GetComponent<Renderer>();
            rend.material.mainTexture = rt;
            _shirtCamera.nearClipPlane = 0.01f;
        }
        
        public AssetBundle InitialiseShirtPad(string path)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            return AssetBundle.LoadFromStream(stream);
        }
    }
}