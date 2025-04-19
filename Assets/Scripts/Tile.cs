using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    bool inPlayerRange, revealed;

    void OnMouseDown()
    {
        if (inPlayerRange)
        {
            if (!revealed)
            {
                //Reveal
            }
        }
        else
        {
            //Player's not in range
        }
    }
}
