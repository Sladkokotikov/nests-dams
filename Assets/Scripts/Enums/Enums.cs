namespace Enums
{
    public enum ApplicationType
    {
        Confirm,
        ConfirmRandom,
        ConfirmAuto,
    }

    public enum ArgumentType
    {
        Field,
        Card
    }

    public enum CardSourceType : byte
    {
        PlayerDeck,
        PlayerGraveyard,
        OpponentHand,
        OpponentDeck,
        OpponentGraveyard,
    }

    public enum CommandType
    {
        Declaration,
        Specification,
        Application
    }

    public enum DeclarationType
    {
        Spawn,
        Kill,
        Push,
        Pull,
        Draw,
        /*Discard,
        Steal,*/
        Lock,
        Unlock,
        Break,
        Build,
    }

    public enum CardOperationType
    {
        Draw,
        //Discard
    }

    public enum FieldOperationType
    {
        Spawn,
        Kill,
        Push,
        Pull,
        Lock,
        Unlock,
        Break,
        Build,
    }

    public enum FieldSpecificationType
    {
        OuterEdge,
        Adjacent,
        Surrounding,
        Plus,
        Edge,
    }

    public enum SpecificationType
    {
        CardSource,
        Tribe,
        ConcreteCard,
        FieldRule,
        AllControl
    }

    public enum ConcreteCard
    {
        BeaverCub,
        MagpieFledgling
    }
}