public enum BytecodeBasis: byte
{
    Spawn, Kill,
    Push, Pull,
    Convert, Invert,
    Draw, Discard,
    Lock, Unlock,
    Break, Build,
    
    Adjacent, Surrounding, Plus, Edge, Free, Occupied, 
    
    Confirm, ConfirmRandom, ConfirmAuto,
    
    Beaver, Magpie, Obstacle, Playable,
}