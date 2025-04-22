using UnityEngine;

public class FuelRestEvent : TileEvents
{
    bool inEffect;

    protected override void Effect()
    {
        inEffect = !inEffect;

        if (inEffect)
        {
            GameManager.Instance.RestartDecisionTimer();
            GameManager.Instance.StopDecisionTimer();
        }
        else
        {
            GameManager.Instance.RestartDecisionTimer();
        }

    }
}
