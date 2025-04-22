using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    public GameObject resultpanel;

    private void OnEnable()
    {
        GoalEvent.OnGoal += CallResultPanel;
        CloseResoutPanel();
    }

    private void OnDisable()
    {
        GoalEvent.OnGoal -= CallResultPanel;
    }

    public void CallResultPanel()
    {
        resultpanel.SetActive(true);
    }

    public void CloseResoutPanel()
    {
        resultpanel.SetActive(false);
    }
}
