using UnityEngine;
using TMPro;

public class TileDescription : MonoBehaviour
{
    public GameObject infoPanel;
    public TextMeshProUGUI descTxt;

    private void OnEnable()
    {
        infoPanel.SetActive(false);

        Tile.DescriptionEvent += CallDescInfo;
    }

    private void OnDisable()
    {
        Tile.DescriptionEvent -= CallDescInfo;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void CallDescInfo(Tile tile)
    {
        var hasDesc = tile.infoDescription != string.Empty;
        infoPanel.SetActive(hasDesc);

        if (hasDesc)
        {
            descTxt.text = tile.infoDescription;
        }
    }

    void CloseDescInfo(Tile tile)
    {
        infoPanel.SetActive(false);
    }
}
