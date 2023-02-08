public class TileInfo
{
    public Tribe OccupantTribe { get; }
    public int OccupantId { get; }

    public bool Occupied => OccupantTribe != Tribe.None;
    public bool Free => !Occupied;

    public static TileInfo FreeTile => new TileInfo(Tribe.None, -1);
    public static TileInfo Create(Tribe tribe, int id) => new TileInfo(tribe, id);

    private TileInfo(Tribe tribe, int id)
    {
        OccupantTribe = tribe;
        OccupantId = id;
    }
}