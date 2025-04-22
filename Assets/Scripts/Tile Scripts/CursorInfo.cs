using UnityEngine;
using TMPro;

public class CursorInfo : MonoBehaviour
{
    public GameObject cursorInfoObj, reticleObj;
    public TextMeshProUGUI infoTxt;

    public Vector2 offsetFromCursor;
    bool isActive;

    private void OnEnable()
    {
        cursorInfoObj.SetActive(false);
        reticleObj.SetActive(false);

        Tile.CursorInEvent += CallCursorInfo;
        Tile.CursorOutEvent += CloseCursorInfo;
    }

    private void OnDisable()
    {
        Tile.CursorInEvent -= CallCursorInfo;
        Tile.CursorOutEvent -= CloseCursorInfo;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisual();
    }

    void CallCursorInfo(Tile tile)
    {
        if (tile.infoCursor != string.Empty && tile.state == Tile.TileState.Checked)
        {
            cursorInfoObj.SetActive(true);
            infoTxt.text = tile.infoCursor;
        }

        reticleObj.SetActive(true);
        reticleObj.transform.position = tile.gameObject.transform.position;
    }

    void CloseCursorInfo(Tile tile)
    {
        cursorInfoObj.SetActive(false);

        reticleObj.SetActive(false);
    }

    void UpdateVisual()
    {
        isActive = cursorInfoObj.activeSelf;

        if (isActive)
        {
            cursorInfoObj.transform.position = (Vector2)Input.mousePosition + offsetFromCursor;
        }


    }
}
