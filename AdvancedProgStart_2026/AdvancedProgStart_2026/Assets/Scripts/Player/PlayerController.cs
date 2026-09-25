using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //send data ONE DIRECTIONAL to the player
    private PlayerBody body;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponentInChildren<PlayerBody>();
    }

    // Update is called once per frame
    void Update()
    {
        // handle input from the input manager and send that down the line to the player body
        Vector2 move = InputManager.Instance.MoveVector;
        body.Direction = move.y;
        body.RotationDir = move.x;

        //Fire Attacks
        if (InputManager.Instance.AttackPresed) body.Fire();

        if (InputManager.Instance.AltAttack) body.Fire(Bulllet.BulletType.Grenade);
    }
}
