using UnityEngine;
using TMPro;

public class CursorInfo : MonoBehaviour
{
    public GameObject cursorInfoObj, reticleObj;
    public TextMeshProUGUI infoTxt;

    public Vector2 offsetFromCursor;
    public Texture2D cursorDefault, cursorInavail, cursorCheck, cursorWalk;

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

        
        if (tile.state == Tile.TileState.Checked && GameManager.Instance.CanMoveTo(tile))
        {
            Cursor.SetCursor(cursorWalk, Vector2.zero, CursorMode.Auto);
        }
        else if (tile.state == Tile.TileState.Revealed && GameManager.Instance.CanMoveTo(tile))
        {
            Cursor.SetCursor(cursorCheck, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursorInavail, Vector2.zero, CursorMode.Auto);
        }
    }

    void CloseCursorInfo(Tile tile)
    {
        cursorInfoObj.SetActive(false);

        reticleObj.SetActive(false);

        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
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
