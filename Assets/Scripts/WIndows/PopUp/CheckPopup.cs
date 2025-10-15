using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckPopup : GenericPopup
{
    public Button backToGame;
    public Button backToTitle;

    private bool forcingClose;

    public override void Init(PopupManager manager)
    {
        base.Init(manager);
        backToGame.onClick.AddListener(() =>
        {
            forcingClose = true;
            Close();
        });
        backToTitle.onClick.AddListener(() =>
        {
            SaveLoadManager.Data.canLoadSaveData = false;
            SaveLoadManager.Save();
            SceneManager.LoadScene(0);
        });

        gameObject.SetActive(false);
    }

    public override void Open()
    {
        forcingClose = false;
        base.Open();
    }

    public override bool Close()
    {
        if(!TouchManager.TouchStartInUI() || forcingClose)
        {
            return base.Close();
        }

        return true;
    }
}
