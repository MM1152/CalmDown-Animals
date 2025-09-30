using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalDes : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI animalname;
    public TextMeshProUGUI cr_IdText;
    public TextMeshProUGUI captureText;
    public TextMeshProUGUI sizeText;
    public TextMeshProUGUI speedText;

    public Button button;

    private void Start()
    {
        button.onClick.AddListener(() => gameObject.SetActive(false));
        gameObject.SetActive(false);
    }

    public void UpdateDescription(AnimalInfoTable.Data data)
    {
        animalname.text = data.Kor_Name;
        cr_IdText.text = "등급 : " + DataTableManager.animalCRRankTable.Get(data.CR_ID).name;
        captureText.text = "포획도 : " + data.CaptureHP.ToString();
        sizeText.text = "사이즈 : " + DataTableManager.animalSizeTable.Get(data.Size_ID).name;
        speedText.text = "속도 : " + DataTableManager.animalSpeedTable.Get(data.Spd).name;

        image.sprite = data.fullImage;

        gameObject.SetActive(true);
    }
}
