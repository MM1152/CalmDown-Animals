using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TileUndo : MonoBehaviour
{
    private TileManager tileManager;
    private GameManager gameManager;

    private List<PathTile> undoTileList = new List<PathTile>();
    private int gold;
    [Header("BUTTONS")]
    public Button undoTileButton;
    private void Awake()
    {
        tileManager = GetComponent<TileManager>();
    }

    private void Start()
    {
        gameManager = GameObject.FindWithTag(TagIds.GameManagerTag)?.GetComponent<GameManager>();

        undoTileButton.onClick.AddListener(() =>
        {
            tileManager.ClearAllTiles();

            foreach(var tile in undoTileList)
            {
                tile.Type = TileType.Path;
            }

            tileManager.ChangeToColorPathTiles();
            tileManager.undoAble = false;
            UpdateButton();

            gameManager.Gold = gold;
        });
    }

    public void SaveUndoList(List<PathTile> undoList , int saveGold)
    {
        undoTileList = undoList;
        gold = saveGold;
    }

    public void UpdateButton()
    {
        undoTileButton.GetComponent<ButtonUI>().IsOn = !tileManager.undoAble;
    }
}
