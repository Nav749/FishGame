using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private float LifeTime = 1f;

    private void Start()
    {
        Destroy(gameObject, LifeTime);
    }
}
