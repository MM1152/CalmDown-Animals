using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneChange : MonoBehaviour
{
    private readonly string tutorialId = "TutorialScene";
    public string changeSceneId;

    private Button button;

    public GameObject viewer;
    public Button loadFromFile;
    public Button startNew;

    public Button bookButton;
    public Button settingButton;

    public PopupManager popup;

    public void ChangeScene(string id)
    {
        if(id == tutorialId)
        {
            SaveLoadManager.Data.canLoadSaveData = false;
        }
        SceneManager.LoadScene(id);
    }

    private void Start()
    {
        SaveLoadManager.Load();
        Application.targetFrameRate = -1;
        SoundManager.Instance.PlayBackGround(BGM.TitleSceneBackGround);

        bookButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayOneShot(SFX.OnClickButtonInTitle);
        });

        loadFromFile.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(changeSceneId);
        });
        startNew.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayOneShot(SFX.OnClickStartbuttonInTitle);
            
            SaveLoadManager.Data.canLoadSaveData = false;
            SaveLoadManager.Save();
            SceneManager.LoadScene(changeSceneId);
        });
        settingButton.onClick.AddListener(() =>
        {
            popup.Open(Popup.OptionPopup);
        });


        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if(!SaveLoadManager.Data.canLoadSaveData)
            {
                if(!SaveLoadManager.Data.isClearTutorial)
                {
                    SceneManager.LoadScene(2);
                }else
                {
                    SceneManager.LoadScene(changeSceneId);
                }
            }
            else
            {
                viewer.SetActive(true);
            }
        });       
    }
}
