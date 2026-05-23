using UnityEngine;

public class RefillFuelEvent : TileEvents
{
    public float refillAmount;

    public override void Effect()
    {
        print("Refilled for " + refillAmount);
        FuelSystem.Instance.RefillFuel(refillAmount);
    }
}
