namespace GameTestApp.Models
{
    using Prism.Events;

    /// <summary>
    /// Game over event
    /// </summary>
    public class GameOverEvent : PubSubEvent
    {
    }

    /// <summary>
    /// Game started event
    /// </summary>
    public class GameStartedEvent : PubSubEvent
    {
    }

    /// <summary>
    /// Mine exploded event
    /// </summary>
    public class GameMineExplodedEvent : PubSubEvent
    {
    }

    /// <summary>
    /// Game score changed event
    /// </summary>
    public class GameScoreChangedEvent : PubSubEvent
    {
    }

    /// <summary>
    /// Game player won event
    /// </summary>
    public class GamePlayerWonEvent : PubSubEvent
    {
    }
}