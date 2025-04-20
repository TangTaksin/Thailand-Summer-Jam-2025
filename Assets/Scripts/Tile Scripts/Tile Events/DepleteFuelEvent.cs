using UnityEngine;

public class DepleteFuelEvent : TileEvents
{
    public float depleteAmount;

    protected override void Effect()
    {
        FuelSystem.Instance.UseFuel(depleteAmount);
    }
}
