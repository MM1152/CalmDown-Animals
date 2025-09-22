using System.Collections.Generic;

public class Weapon
{
    private List<EquipmentInfo.Data> weaponDatas;
    private int curEquipmentId = -1;

    public Weapon(int rankId)
    {
        weaponDatas = DataTableManager.equipmentInfoTable.GetRankIdToEquipment(rankId);
    }

    public EquipmentInfo.Data Equip(int equipmentId)
    {
        if (equipmentId >= weaponDatas.Count) return null;
        curEquipmentId = equipmentId;
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