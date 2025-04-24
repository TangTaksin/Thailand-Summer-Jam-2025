using UnityEngine;

public class DepleteFuelEvent : TileEvents
{
    public float depleteAmount;

    public override void Effect()
    {
        FuelSystem.Instance.UseFuel(depleteAmount);
    }
}
