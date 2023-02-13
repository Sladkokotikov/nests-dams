using Enums;

public enum BytecodeBasis : byte
{
    [Command(CommandType.Application)] /*[Application(ApplicationType.Confirm)]*/
    Confirm = 0,

    [Command(CommandType.Application)] /*[Application(ApplicationType.ConfirmRandom)]*/
    ConfirmRandom = 1,

    [Command(CommandType.Application)] /*[Application(ApplicationType.ConfirmAuto)]*/
    ConfirmAuto = 2,

    [Command(CommandType.Declaration)] [Declaration(DeclarationType.Spawn)]
    Spawn = 3,

    [Command(CommandType.Declaration)] [Declaration(DeclarationType.Kill)]
    Kill = 4,

    [Command(CommandType.Declaration)] [Declaration(DeclarationType.Push)]
    Push = 5,

    [Command(CommandType.Declaration)] [Declaration(DeclarationType.Pull)]
    Pull = 6,

    [Command(CommandType.Declaration)] [Declaration(DeclarationType.Draw)]
    Draw = 7,

    [Command(CommandType.Declaration)] [Declaration(DeclarationType.Lock)]
    Lock = 8,

    [Command(CommandType.Declaration)] [Declaration(DeclarationType.Unlock)]
    Unlock = 9,

    [Command(CommandType.Declaration)] [Declaration(DeclarationType.Break)]
    Break = 10,

    [Command(CommandType.Declaration)] [Declaration(DeclarationType.Build)]
    Build = 11,

    [Command(CommandType.Specification)] [Specification(SpecificationType.FieldRule)]
    OuterEdge = 12,

    [Command(CommandType.Specification)] [Specification(SpecificationType.FieldRule)]
    Adjacent = 13,

    [Command(CommandType.Specification)] [Specification(SpecificationType.FieldRule)]
    Surrounding = 14,

    [Command(CommandType.Specification)] [Specification(SpecificationType.FieldRule)]
    Plus = 15,

    [Command(CommandType.Specification)] [Specification(SpecificationType.FieldRule)]
    Edge = 16,

    [Command(CommandType.Specification)] [Specification(SpecificationType.Tribe)]
    Beaver = 19,

    [Command(CommandType.Specification)] [Specification(SpecificationType.Tribe)]
    Magpie = 20,

    [Command(CommandType.Specification)] [Specification(SpecificationType.Tribe)]
    Obstacle = 21,

    [Command(CommandType.Specification)] [Specification(SpecificationType.Tribe)]
    Playable = 22,

    [Command(CommandType.Specification)] [Specification(SpecificationType.Tribe)]
    None = 23,

    [Command(CommandType.Specification)] [Specification(SpecificationType.CardSource)]
    PlayerDeck = 24,

    [Command(CommandType.Specification)] [Specification(SpecificationType.CardSource)]
    PlayerGraveyard = 25,

    [Command(CommandType.Specification)] [Specification(SpecificationType.CardSource)]
    OpponentHand = 26,

    [Command(CommandType.Specification)] [Specification(SpecificationType.CardSource)]
    OpponentDeck = 27,

    [Command(CommandType.Specification)] [Specification(SpecificationType.CardSource)]
    OpponentGraveyard = 28,

    [Command(CommandType.Specification)] [Specification(SpecificationType.ConcreteCard)]
    BeaverCub = 29,
}