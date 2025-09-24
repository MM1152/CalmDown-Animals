using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    public Toggle[] toggles;
    public ToggleGroup toggleGroup;
    public GameObject[] groups;

    public AnimalImage prefabs;
    private void Start()
    {
        for(int i  = 0; i< toggles.Length; i++)
        {
            int idx = i;
            toggles[i].onValueChanged.AddListener((value) =>
            {
                if(value)
                {
                    groups[idx].SetActive(true);
                }
                else
                {
                    groups[idx].SetActive(false);
                }
            });
        }
        toggleGroup.NotifyToggleOn(toggles[0]);
        toggles[0].Select();
    }
}
