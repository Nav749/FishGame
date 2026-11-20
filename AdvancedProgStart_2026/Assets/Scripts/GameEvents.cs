using UnityEditor;
using UnityEngine.Events;

public static class GameEvents
{
    public static event UnityAction OnGameReset;

    public static void RaiseGameReset()
    {
        OnGameReset?.Invoke();
    }
}
