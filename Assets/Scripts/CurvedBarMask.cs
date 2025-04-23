using UnityEngine;

public class CurvedBarMask : MonoBehaviour
{
    public RectTransform fillMask;
    [Range(0, 1)] public float fillAmount = 0.5f;
    public float maxWidth = 200f; // width when full

    void Update()
    {
        Vector2 size = fillMask.sizeDelta;
        size.x = maxWidth * fillAmount;
        fillMask.sizeDelta = size;
    }
}
