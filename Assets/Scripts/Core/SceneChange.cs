using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneChange : MonoBehaviour
{
    public string changeSceneId;

    private Button button;
    private void Start()
    {
        if(DataTableManager.init)
        {
        }
        button = GetComponent<Button>();
        button.onClick.AddListener(() => SceneManager.LoadScene(changeSceneId));       
    }
}
