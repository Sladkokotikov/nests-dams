public enum BytecodeBasis : byte
{
    Confirm = 0,
    ConfirmRandom= 1,
    ConfirmAuto = 2,

    Spawn = 3,
    Kill = 4,
    Push = 5,
    Pull = 6,
    Draw = 7,
    Lock = 8,
    Unlock = 9,
    Break = 10,
    Build = 11,

    OuterEdge = 12,
    Adjacent = 13,
    Surrounding = 14,
    Plus = 15,
    Edge = 16,
    
    Beaver = 19,
    Magpie = 20,
    Obstacle = 21,
    Playable = 22,
    None = 23,

    PlayerDeck = 24,
    PlayerGraveyard = 25,
    OpponentHand = 26,
    OpponentDeck = 27,
    OpponentGraveyard = 28,
    
    BeaverCub = 29,
}