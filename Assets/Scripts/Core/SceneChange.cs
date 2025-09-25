using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneChange : MonoBehaviour
{
    public string changeSceneId;

    private Button button;

    public GameObject viewer;
    public Button loadFromFile;
    public Button startNew;
    private void Start()
    {
        SaveLoadManager.Load();

        loadFromFile.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(changeSceneId);
        });
        startNew.onClick.AddListener(() =>
        {
            SaveLoadManager.Data.canLoadSaveData = false;
            SceneManager.LoadScene(changeSceneId);
        });

        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if(!SaveLoadManager.Data.canLoadSaveData)
            {
                SceneManager.LoadScene(changeSceneId);
            }
            else
            {
                viewer.SetActive(true);
            }
        });       
    }
}
