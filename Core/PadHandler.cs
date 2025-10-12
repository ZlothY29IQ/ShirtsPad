using GorillaShirts.Models.UI;
using ShirtsPad.Components;
using TMPro;
using UnityEngine;

namespace ShirtsPad.Core;

public class PadHandler : MonoBehaviour
{
    public static    GameObject ShirtPad;
    private readonly Vector3    shirtsViewBackDisabled = new(0f, -0.0141f, 0.00518f);

    private readonly Vector3 shirtsViewBackEnabled = new(-0.00122f, -0.0141f, 0.00518f);

    private GameObject  back;
    private TextMeshPro equipText;
    private TextMeshPro shirtCreator;
    private TextMeshPro shirtName;
    private GameObject  shirtsView;

    private GameObject      standBack;
    private TextMeshProUGUI standEquipText;
    private TextMeshProUGUI standShirtCreator;
    private TextMeshProUGUI standShirtName;

    private void Start()
    {
        shirtName    = ShirtPad.transform.Find("ShirtName").GetComponent<TextMeshPro>();
        shirtCreator = ShirtPad.transform.Find("ShirtCreator").GetComponent<TextMeshPro>();
        equipText    = ShirtPad.transform.Find("Equip/Text").GetComponent<TextMeshPro>();

        back = ShirtPad.transform.Find("Back").gameObject;
        back.AddComponent<PressableButton>().ButtonType = EButtonType.Return;
        ShirtPad.transform.Find("Previous").AddComponent<PressableButton>().ButtonType = EButtonType.NavigateDecrease;
        ShirtPad.transform.Find("Next").AddComponent<PressableButton>().ButtonType = EButtonType.NavigateIncrease;
        ShirtPad.transform.Find("Equip").AddComponent<PressableButton>().ButtonType = EButtonType.NavigateSelect;
        ShirtPad.transform.Find("Credits/Version").GetComponent<TextMeshPro>().text = $"Version: {Constants.Version}";

        ShirtPad.transform.Find("ShirtsView").AddComponent<StandCameraHandler>();
    }

    private void Update()
    {
        if (!ShirtPad.activeSelf) return;

        if (standBack == null)
            standBack = GameObject.Find("GorillaShirts/Shirt Stand/UI/MainMenu/MainContainer/Text/Navigation/");

        if (standShirtName == null)
            standShirtName = GameObject.Find("GorillaShirts/Shirt Stand/UI/MainMenu/MainContainer/Text/Item/Main")
                                       .GetComponent<TextMeshProUGUI>();

        if (standShirtCreator == null)
            standShirtCreator = GameObject.Find("GorillaShirts/Shirt Stand/UI/MainMenu/MainContainer/Text/Item/Body")
                                          .GetComponent<TextMeshProUGUI>();

        if (standEquipText == null)
            standEquipText = GameObject
                            .Find("GorillaShirts/Shirt Stand/UI/MainMenu/MainContainer/Buttons/ShirtEquip/Equip")
                            .GetComponent<TextMeshProUGUI>();

        if (standBack != null && back.activeSelf != standBack.activeSelf)
        {
            back.SetActive(standBack.activeSelf);
            shirtsView.transform.localPosition = standBack.activeSelf ? shirtsViewBackEnabled : shirtsViewBackDisabled;
        }

        if (standShirtName != null && shirtName.text != standShirtName.text) shirtName.text = standShirtName.text;
        if (standShirtCreator != null && shirtCreator.text != standShirtCreator.text)
            shirtCreator.text = standShirtCreator.text;

        if (standEquipText != null && equipText.text != standEquipText.text) equipText.text = standEquipText.text;
    }
}