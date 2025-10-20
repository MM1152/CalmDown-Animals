using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneButton : MonoBehaviour
{
    public string changeSceneId;

    private Button button;

    public GameObject viewer;
    public Button loadFromFile;
    public Button startNew;

    public Button bookButton;
    public Button settingButton;

    private void Awake()
    {
#if !DEBUG_MODE
        Debug.unityLogger.logEnabled = false;
#endif        
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
            SceneManager.LoadScene(changeSceneId);
        });

        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if (!SaveLoadManager.Data.canLoadSaveData)
            {
                if (!SaveLoadManager.Data.isClearTutorial)
                {
                    SceneManager.LoadScene(2);
                }
                else
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
