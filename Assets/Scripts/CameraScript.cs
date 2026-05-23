using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Camera cam;

    public GridManager gridManager;
    public Vector2 targetToFollow;
    //public float damping; 



    private void OnEnable()
    {
        cam = Camera.main;
        GridManager.OnGenerated += UpdateTarget;
    }

    private void OnDisable()
    {
        GridManager.OnGenerated -= UpdateTarget;
    }

    private void Update()
    {

    }

    void UpdateTarget()
    {
        var leftBottom = gridManager.GetTile(0, 0).transform.position;
        var topRight = gridManager.GetTile(gridManager.width - 1, gridManager.height - 1).transform.position;

        targetToFollow = (leftBottom + topRight) / 2;
        cam.transform.position = new Vector3(targetToFollow.x, targetToFollow.y, cam.transform.position.z);
    }

    void FollowTarget()
    {
    }
}
