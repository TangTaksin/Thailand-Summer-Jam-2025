using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("Clicked tile: " + gameObject.name);
        Destroy(gameObject);
    }
}
