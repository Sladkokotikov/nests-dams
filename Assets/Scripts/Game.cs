using Configuration;
using UnityEngine;

public class Game : MonoBehaviour
{
    [field: SerializeField] public CardsConfiguration CardConfig { get; private set; }
    [field: SerializeField] public BotConfiguration BotConfig { get; private set; }

    [SerializeField] private MatchConfiguration matchConfig;

    [field: SerializeField] public GoalManager GoalManager { get; private set; }
    [field: SerializeField] public Messenger Messenger { get; private set; }
    [field: SerializeField] public Hand Hand { get; private set; }
    [field: SerializeField] public Field Field { get; private set; }

    private GameEngine _engine;
    public GameEngine Engine => _engine;

    private void Start()
    {
        Field.Tilt();
        _engine = new GameEngine(this, matchConfig.FieldSize);

        StartCoroutine(_engine.Play());
    }

    public void Win(Player player, Goal goal)
    {
        Messenger.Alert($"{player.Name} победил!");
        Field.Win(goal);
    }
}