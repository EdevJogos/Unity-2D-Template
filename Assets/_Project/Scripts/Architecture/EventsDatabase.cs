using System.Collections.Generic;

public enum GameEvents
{
    START_GAME,
}

public static class EventsDatabase
{
    public static Dictionary<GameEvents, TrackedEvent> EventsList = new Dictionary<GameEvents, TrackedEvent>(1)
    {
        { GameEvents.START_GAME, new TrackedEvent(GameEvents.START_GAME) }
    };

    public static TrackedEvent GetEvent(GameEvents p_id)
    {
        return EventsList[p_id];
    }
}