using UnityEngine;

public class TileEvents : MonoBehaviour
{
    Tile attachedTile;

    public bool triggerOnEnter, triggerOnExit;
    public bool OneTimeEffect;
    bool triggered;

    protected virtual void OnEnable()
    {
        attachedTile = GetComponent<Tile>();

        if (triggerOnEnter)
            attachedTile.OnEnterTile += TriggerEffect;
        if (triggerOnExit)
            attachedTile.OnExitTile += TriggerEffect;
    }

    protected virtual void OnDisable()
    {
        if (triggerOnEnter)
            attachedTile.OnEnterTile -= TriggerEffect;
        if (triggerOnExit)
            attachedTile.OnExitTile -= TriggerEffect;
    }

    public void TriggerEffect()
    {
        if (!triggered)
        {
            Effect();

            if (OneTimeEffect)
                triggered = true;
        }
    }

    protected virtual void Effect()
    {

    }
}
