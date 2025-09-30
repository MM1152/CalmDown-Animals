using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CrewReadyTab : MonoBehaviour
{

    public TextMeshProUGUI rankText;
    public TextMeshProUGUI paymentText;
    public TextMeshProUGUI captureDamageText;
    public TextMeshProUGUI captureSpeedText;
    public Image equipmentImage;
    public TextMeshProUGUI equipmentNameText;
    public TextMeshProUGUI captureAnimalText;

    public GameObject equipmentListTab;
    public GameObject[] equipmentList;
    public GameObject[] equipmentTab;

    public Sprite[] weapons;

    public Image[] captureAble;

    private Crew currentCrew;

    private void Start()
    {
        
        var uiEvent = equipmentTab[0].GetOrAddComponent<UIEvent>();
        if (uiEvent != null)
        {
            uiEvent.PointerClick += (eventData) => OpenEquipmentList(true);
        }

        for(int i = 0; i < equipmentList.Length; i++)
        {
            uiEvent = equipmentList[i].GetComponent<UIEvent>();
            int idx = i;
            if(uiEvent != null)
            {
                // 장착시킬 데이터 넘김
                uiEvent.PointerClick += (eventData) =>
                {
                    var data = currentCrew.weapon.Equip(idx);
                    equipmentImage.sprite = weapons[currentCrew.weapon.GetWeaponId()];
                    OpenEquipmentList(false);
                    Open(currentCrew);
                };
            }
        }
    }

    private void OpenEquipmentList(bool active)
    {
        equipmentListTab.SetActive(active);
        foreach (var obj in equipmentTab)
        {
            obj.SetActive(!active);
        }
    }

    public void Close()
    {
        OpenEquipmentList(false);
        gameObject.SetActive(false);
        currentCrew = null;
    }
    public void Open(Crew crew)
    {
        currentCrew = crew;
        rankText.text = DataTableManager.crewTable.Get(crew.Rank).Crew_name;
        paymentText.text = crew.GetPayCheck() + " / 라운드";
        equipmentNameText.text = crew.weapon.GetName();
        captureDamageText.text = (int)(crew.weapon.GetCaptureDmg() + crew.GetCapture()) + "";
        captureSpeedText.text = (int)(crew.weapon.GetCaptureSpeed() + crew.GetCaptureSpeed()) + "";
        gameObject.SetActive(true);

        if(crew.Rank == CrewRank.Intern)
        {
            equipmentList[2].transform.GetChild(0).GetComponent<Image>().color = Color.black;
        }
        else
        {
            equipmentList[2].transform.GetChild(0).GetComponent<Image>().color = new Color(0.6901961f , 0.7098039f , 0.6509804f);
        }

            foreach (var image in captureAble)
            {
                image.color = Color.black;
            }

        for(int i = 0; i < 5; i++)
        {
            if(((int)crew.weapon.GetCaptureSize() & (1 << i)) > 0)
            {
                captureAble[i].color = Color.white;
            }
        }

        equipmentImage.sprite = weapons[currentCrew.weapon.GetWeaponId()];
    }
}
