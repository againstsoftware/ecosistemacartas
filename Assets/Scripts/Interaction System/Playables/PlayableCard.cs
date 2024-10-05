
using UnityEngine;
using UnityEngine.Serialization;

public class PlayableCard : APlayableItem, IActionReceiver, IRulebookEntry
{
    public override bool OnlyVisibleOnOverview => false;
    public override bool CanInteractWithoutOwnership => _canInteractWithoutOwnership;

    public override IActionItem ActionItem => Card;
    public bool IsDropEnabled { get; private set; } = false;

    [field:SerializeField] public ACard Card { get; private set; }
    [field:SerializeField] public Transform SnapTransform { get; private set; }

    [SerializeField] private float _closestCardZ;
    private bool _canInteractWithoutOwnership = false;


    public string GetName() => Card.Name;

    public string GetDescription() => Card.Description;


    public int IndexOnSlot { get; set; } = -1;

    public SlotReceiver SlotWherePlaced { get; private set; }
    public PlayableCard CardWherePlaced { get; private set; }
    
    
    public void PlayCard(IActionReceiver playLocation)
    {
        IsPlayed = true;
        IsDraggable = false;
        IsDropEnabled = true;
        _canInteractWithoutOwnership = true;

        SlotWherePlaced = playLocation as SlotReceiver;
        CardWherePlaced = playLocation as PlayableCard;

        //moverla a la playLocation
        if (SlotWherePlaced is not null) SlotWherePlaced.AddCardOnTop(this);
        
        
    }

    public override void OnSelect()
    {
        transform.localPosition = new(transform.localPosition.x, transform.localPosition.y, _closestCardZ);
        base.OnSelect();
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
        transform.position = _defaultPosition;
    }

    public void OnDraggingSelect()
    {
        OnSelect();
    }

    public void OnDraggingDeselect()
    {
        OnDeselect();
    }

    public void OnChoosingSelect()
    {
        OnSelect();
    }

    public void OnChoosingDeselect()
    {
        OnDeselect();
    }


    public override void OnDrop(IActionReceiver dropLocation)
    {
        base.OnDrop(dropLocation);
        transform.rotation = dropLocation.SnapTransform.rotation;
    }
    
    
    public Receiver GetReceiverStruct(ValidDropLocation actionDropLocation) => 
        new (actionDropLocation, Owner, 
            CardWherePlaced is not null ? CardWherePlaced.SlotWherePlaced.IndexOnTerritory : SlotWherePlaced.IndexOnTerritory,
            CardWherePlaced is not null ? CardWherePlaced.IndexOnSlot : IndexOnSlot);

    
    
}