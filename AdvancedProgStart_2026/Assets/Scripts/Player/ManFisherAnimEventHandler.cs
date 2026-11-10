using UnityEngine;

public class ManFisherAnimEventHandler : MonoBehaviour
{
    [HideInInspector] public PlayerBody player;

    public void Swipe()
    {
        player.Swipe();
    }
}
