using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    public Toggle[] toggles;
    private Image[] togglesBackGroundImage = new Image[5];
    public ToggleGroup toggleGroup;
    public GameObject[] groups;
    public Transform[] parents;

    public Sprite initSprite;
    public Sprite pressedSprite;

    public Button button;
    public AnimalDes animalDes;
    public AnimalImage prefabs;

    private void Start()
    {
        for(int i = 0; i < toggles.Length; i++)
        {
            togglesBackGroundImage[i] = toggles[i].transform.GetComponentInChildren<Image>();
        }
        for(int i  = 0; i< toggles.Length; i++)
        {
            int idx = i;
            toggles[i].onValueChanged.AddListener((value) =>
            {
                if(value)
                {
                    togglesBackGroundImage[idx].sprite = pressedSprite;
                    groups[idx].SetActive(true);
                }
                else
                {
                    togglesBackGroundImage[idx].sprite = initSprite;
                    groups[idx].SetActive(false);
                }
            });
        }


        for(int i = 0; i < 5; i++)
        {
            var datas = DataTableManager.animalInfoTable.GetToCR_ID(i + 1);
            foreach(var data in datas)
            {
                var animalImage = Instantiate(prefabs, parents[i].transform);
                animalImage.UpdateSlot(data , animalDes);
                if(data.Icon == null)   
                {
                    Debug.Log(data.Animal_name);
                }
            }   
        }

        toggles[0].isOn = true;
        toggleGroup.NotifyToggleOn(toggles[0]);
        EventSystem.current.firstSelectedGameObject = toggles[0].gameObject;

        button.onClick.AddListener(() => gameObject.SetActive(false));
        gameObject.SetActive(false);
    }
}
