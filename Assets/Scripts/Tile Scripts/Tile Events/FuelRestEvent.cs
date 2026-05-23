using UnityEngine;

public class FuelRestEvent : TileEvents
{
    bool inEffect;

    public override void Effect()
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
