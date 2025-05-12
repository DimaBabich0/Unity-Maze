using System;
using System.Collections.Generic;

public class GameEventSystem
{
    private static readonly List<Action<GameEvent>>  subscribers =
        new List<Action<GameEvent>>();

    public static void Subscribe(Action<GameEvent> action)
    {
        subscribers.Add(action);
    }

    public static void Unsubscribe(Action<GameEvent> action)
    {
        subscribers.Remove(action);
    }

    public static void TriggerEvent(GameEvent gameEvent)
    {
        foreach (var action in subscribers)
        {
            action(gameEvent);
        }
    }
}
