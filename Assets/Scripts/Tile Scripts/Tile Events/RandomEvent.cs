using UnityEngine;

public class RandomEvent : TileEvents
{
    [Header("Possible Events")]
    public GameObject[] possibleEvent;

    private Tile refTile;
    private TileEvents refEvent;

    private SpriteSwitchEvent refSpriteEvent;
    private SwitchInfoEvent refCurInfoEvent;
    private SwitchDescriptionEvent refDescEvent;

    private SpriteRenderer sRenderer;

    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        RandomizeEvent();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (refTile != null && refTile.descriptionVariants.Length > 0)
        {
            int index = Random.Range(0, refTile.descriptionVariants.Length);
            attachedTile.infoDescription = refTile.descriptionVariants[index];
        }
    }

    private void RandomizeEvent()
    {
        int index = Random.Range(0, possibleEvent.Length);
        GameObject chosenEvent = possibleEvent[index];

        // Cache references from chosen event
        refTile = chosenEvent.GetComponent<Tile>();
        refEvent = chosenEvent.GetComponent<TileEvents>();
        refSpriteEvent = chosenEvent.GetComponent<SpriteSwitchEvent>();
        refCurInfoEvent = chosenEvent.GetComponent<SwitchInfoEvent>();
        refDescEvent = chosenEvent.GetComponent<SwitchDescriptionEvent>();

        // Copy and initialize relevant components
        if (refSpriteEvent)
        {
            var spriteRef = gameObject.AddComponent<SpriteSwitchEvent>();
            CopyEventBase(spriteRef, refSpriteEvent);
            spriteRef.newSprite = refSpriteEvent.newSprite;
            spriteRef.Init();
        }

        if (refCurInfoEvent)
        {
            var infoRef = gameObject.AddComponent<SwitchInfoEvent>();
            CopyEventBase(infoRef, refCurInfoEvent);
            infoRef.newCursorInfo = refCurInfoEvent.newCursorInfo;
            infoRef.Init();
        }

        if (refDescEvent)
        {
            var descRef = gameObject.AddComponent<SwitchDescriptionEvent>();
            CopyEventBase(descRef, refDescEvent);
            descRef.newDescInfo = refDescEvent.newDescInfo;
            descRef.Init();
        }

        // Set base event properties
        if (refEvent != null)
        {
            triggerOnEnter = refEvent.triggerOnEnter;
            triggerOnExit = refEvent.triggerOnExit;
            OneTimeEffect = refEvent.OneTimeEffect;
        }
    }

    public override void Effect()
    {
        if (refEvent is SlideEvent || refEvent is BreakableEvent)
            refEvent.Effect(attachedTile);
        else
            refEvent?.Effect();

        if (!OneTimeEffect && refTile != null)
        {
            attachedTile.sprite_front = refTile.sprite_front;
            sRenderer.sprite = refTile.sprite_front;
            attachedTile.infoCursor = refTile.infoCursor;
        }
    }

    private void CopyEventBase(TileEvents target, TileEvents source)
    {
        target.OneTimeEffect = source.OneTimeEffect;
        target.triggerOnEnter = source.triggerOnEnter;
        target.triggerOnExit = source.triggerOnExit;
    }
}
