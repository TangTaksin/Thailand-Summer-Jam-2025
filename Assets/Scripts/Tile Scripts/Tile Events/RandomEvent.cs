using UnityEngine;

public class RandomEvent : TileEvents
{
    public GameObject[] possibleEvent;


    Tile refTile;
    TileEvents refEvent;

    SpriteSwitchEvent refSpriteEvent;
    SwitchInfoEvent refCurInfoEvent;
    SwitchDescriptionEvent refDescEvent;

    SpriteRenderer sRenderer;

    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        RandomizeEvent();

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (refTile.descriptionVariants.Length > 0)
        {
            var index = Random.Range(0, refTile.descriptionVariants.Length);
            attachedTile.infoDescription = refTile.descriptionVariants[index];
        }
    }

    void RandomizeEvent()
    {
        var index = UnityEngine.Random.Range(0, possibleEvent.Length);
        var chosenEvent = possibleEvent[index];

        chosenEvent.TryGetComponent<Tile>(out refTile);
        chosenEvent.TryGetComponent<TileEvents>(out refEvent);
        chosenEvent.TryGetComponent<SpriteSwitchEvent>(out refSpriteEvent);
        chosenEvent.TryGetComponent<SwitchInfoEvent>(out refCurInfoEvent);
        chosenEvent.TryGetComponent<SwitchDescriptionEvent>(out refDescEvent);

        if (refSpriteEvent)
        {
            var spriteRef = gameObject.AddComponent<SpriteSwitchEvent>();
            spriteRef.OneTimeEffect = refSpriteEvent.OneTimeEffect;
            spriteRef.triggerOnEnter = refSpriteEvent.triggerOnEnter;
            spriteRef.triggerOnExit = refSpriteEvent.triggerOnExit;

            spriteRef.newSprite = refSpriteEvent.newSprite;
            spriteRef.Init();
        }

        if (refCurInfoEvent)
        {
            var curRef = gameObject.AddComponent<SwitchInfoEvent>();
            curRef.OneTimeEffect = refCurInfoEvent.OneTimeEffect;
            curRef.triggerOnEnter = refCurInfoEvent.triggerOnEnter;
            curRef.triggerOnExit = refCurInfoEvent.triggerOnExit;

            curRef.newCursorInfo = refCurInfoEvent.newCursorInfo;
            curRef.Init();
        }

        if (refDescEvent)
        {
            var descRef = gameObject.AddComponent<SwitchDescriptionEvent>();
            descRef.OneTimeEffect = refDescEvent.OneTimeEffect;
            descRef.triggerOnEnter = refDescEvent.triggerOnEnter;
            descRef.triggerOnExit = refDescEvent.triggerOnExit;

            descRef.newDescInfo = refDescEvent.newDescInfo;
            descRef.Init();
        }

        triggerOnEnter = refEvent.triggerOnEnter;
        triggerOnExit = refEvent.triggerOnExit;
        OneTimeEffect = refEvent.OneTimeEffect;
    }

    public override void Effect()
    {
        if (refEvent is SlideEvent)
            refEvent.Effect(attachedTile);
        else
            refEvent.Effect();


        if (!OneTimeEffect)
        {
            attachedTile.sprite_front = refTile.sprite_front;
            sRenderer.sprite = refTile.sprite_front;
            attachedTile.infoCursor = refTile.infoCursor;
        }
    }
}
