using UnityEngine;
using TMPro;


public class TileCounter : MonoBehaviour
{
    public GridManager gridManager;
    public TextMeshProUGUI numberDisplay;

    public enum Type
    {
        countup,
        countdown
    }
    public Type displayType;

    private void OnEnable()
    {
        Tile.OnChecked += UpdateCounter;
        GridManager.OnGenerated += UpdateCounter;
    }

    private void OnDisable()
    {
        Tile.OnChecked -= UpdateCounter;
        GridManager.OnGenerated -= UpdateCounter;
    }

    void UpdateCounter()
    {
        var checkedTile = gridManager.TileStatusCounter().Item3;
        var neededTile = gridManager.tileNeeded;
        string display = string.Empty;

        switch(displayType)
        {
            case Type.countup:
                display = string.Format("{0}/{1}", checkedTile, neededTile);
                break;

            case Type.countdown:
                display = string.Format("{0}", neededTile - checkedTile);
                break;
        }

        numberDisplay.text = display;

    }
}
