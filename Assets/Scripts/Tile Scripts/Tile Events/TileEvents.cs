using UnityEngine;

public class TileEvents : MonoBehaviour
{
    protected Tile attachedTile;

    public bool triggerOnEnter, triggerOnExit;
    public bool OneTimeEffect;
    protected bool triggered = false;

    protected virtual void OnEnable()
    {
        Init();
    }

    public virtual void Init()
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
        print(name + " triggered: " + triggered);

        if (!triggered)
        {
            Effect();

            if (OneTimeEffect)
                triggered = true;
        }
    }

    public virtual void Effect()
    {

    }

    public virtual void Effect(Tile tile)
    {

    }
}
