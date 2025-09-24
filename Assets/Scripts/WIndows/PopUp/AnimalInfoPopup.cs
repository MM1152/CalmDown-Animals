using TMPro;
using UnityEngine;

public class AnimalInfoPopup : GenericPopup
{
    public TextMeshProUGUI animalName;
    public TextMeshProUGUI CR_ID;
    public TextMeshProUGUI capture;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI size;

    public AnimalInfoTable.Data AnimalInfomation
    {
        set
        {
            CR_ID.text = DataTableManager.animalCRRankTable.Get(value.CR_ID).name;
            capture.text = value.CaptureHP.ToString();
            speed.text = DataTableManager.animalSpeedTable.Get(value.Spd).name;
            size.text = DataTableManager.animalSizeTable.Get(value.Size_ID).name;
        }
    }
}