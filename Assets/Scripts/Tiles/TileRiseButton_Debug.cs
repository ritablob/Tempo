using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileRiseButton_Debug : MonoBehaviour
{
    public int x, y;
    bool willRise = false;
    public Image buttImage;

    private void Start()
    {
        buttImage = GetComponent<Image>();
    }

    public void ChangeState()
    {
        willRise = !willRise;
        if (willRise)
        {
            Tile_Updater.Instance.ChangeTileState(new Vector2(x, y), "Rise");
            buttImage.color = new Color32(255, 255, 255, 255);
        }
        else 
        {
            Tile_Updater.Instance.ChangeTileState(new Vector2(x, y), "Fall");
            buttImage.color = new Color32(0, 0, 0, 255);
        }
    }

    public void UpdateAllTiles()
    {
        Tile_Updater.Instance.UpdateAllTiles(5, 5);
    }
}
