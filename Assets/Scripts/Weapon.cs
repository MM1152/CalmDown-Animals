using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    private List<EquipmentInfo.Data> weaponDatas;
    private int curEquipmentId = -1;
    private Crew crew;

    private List<GameObject> weapons = new List<GameObject>();

    public Weapon(int rankId , List<GameObject> weapons , Crew crew)
    {
        weaponDatas = DataTableManager.equipmentInfoTable.GetRankIdToEquipment(rankId);
        this.weapons = weapons;
        this.crew = crew;
        for(int i = 0; i < this.weapons.Count; i++)
        {
            weapons[i].SetActive(false);
        }
    }

    public EquipmentInfo.Data Equip(int equipmentId)
    {
        if (equipmentId >= weaponDatas.Count) return null;
        if(curEquipmentId != -1)
        {
            weapons[curEquipmentId].SetActive(false);
        }
        curEquipmentId = equipmentId;
        weapons[curEquipmentId].SetActive(true);
        crew.attackRadius = weaponDatas[curEquipmentId].atk_range;
        crew.FindAroundTiles();
        return weaponDatas[equipmentId];
    }

    public int GetWeaponId()
    {
        return curEquipmentId;
    }

    public int GetCaptureDmg()
    {
        return weaponDatas[curEquipmentId].Equ_capture;
    }
    public float GetCaptureSpeed()
    {
        return weaponDatas[curEquipmentId].Equ_atkspd;
    }
    public string GetName()
    {
        return weaponDatas[curEquipmentId].name;
    }
}