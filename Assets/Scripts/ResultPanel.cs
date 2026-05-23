using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    public GameObject resultpanel;
    public GameObject uiPanel;

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
        GameManager.Instance.WinGame();
        resultpanel.SetActive(true);
        uiPanel.SetActive(false);
    }

    public void CloseResoutPanel()
    {
        resultpanel.SetActive(false);
        uiPanel.SetActive(true);
    }
}
