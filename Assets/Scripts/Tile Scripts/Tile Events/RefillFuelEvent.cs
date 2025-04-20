using UnityEngine;

public class RefillFuelEvent : TileEvents
{
    public float refillAmount;

    protected override void Effect()
    {
        FuelSystem.Instance.RefillFuel(refillAmount);
    }
}
