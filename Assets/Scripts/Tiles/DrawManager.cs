using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawManager : MonoBehaviour
{
    public GameObject prefabs;
    public TMP_InputField widthInput;
    public TMP_InputField heightInput;
    public Button createButton;

    private int width;
    private int height;
    private Vector2 gridSize;

    private void Awake()
    {
        SpriteRenderer sp = prefabs.GetComponent<SpriteRenderer>();
        gridSize = new Vector2(sp.bounds.size.x, sp.bounds.size.z);
    }
    private void Start()
    {
        createButton.onClick.AddListener(() => Create());
    }
    public void Create()
    {
        if(string.IsNullOrEmpty(widthInput.text) || string.IsNullOrEmpty(heightInput.text)) 
            return;

        width = int.Parse(widthInput.text.ToString());
        height = int.Parse(heightInput.text.ToString());

        if (width == 0 || height == 0)
            return;

        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                GameObject tile = Instantiate(prefabs, transform);
                if (i % 2 != 0)
                {
                    tile.transform.position = new Vector3(i * gridSize.y, 0f, j * gridSize.x);
                }
                else
                {
                    tile.transform.position = new Vector3(i * gridSize.y, 0f, j * gridSize.x - gridSize.x * 0.5f);
                }
            }
        }
        
    }
}
