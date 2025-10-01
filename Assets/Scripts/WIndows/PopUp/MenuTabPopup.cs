using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuTabPopup : GenericPopup
{
    public Button optionButton;
    public Button backToTileButton;

    public GameObject target;

    private Action action;

    public override void Init(PopupManager manager)
    {
        base.Init(manager);
        optionButton.onClick.AddListener(() =>
        {
            manager.Open(Popup.OptionPopup);
        });
        backToTileButton.onClick.AddListener(() => {
            SaveLoadManager.Data.canLoadSaveData = false;
            SaveLoadManager.Save();
            SceneManager.LoadScene(0);
        });
    }
    public override void Open()
    {
        Time.timeScale = 0f;
        base.Open();
    }

    public override bool Close()
    {   
        if(TouchManager.TouchStartInUI())
        {
            var hit = TouchManager.GetTouchPositionUI(TouchManager.GetStartPosition());
            if (hit.Count > 0)
            {
                foreach(var obj in hit)
                {
                    if(obj.gameObject == target)
                    {
                        return false;
                    }
                }
            }
        }


        Time.timeScale = 1f;
        return base.Close();
    }
}
    