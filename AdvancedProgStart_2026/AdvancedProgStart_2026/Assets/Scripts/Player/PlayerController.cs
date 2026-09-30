using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //send data ONE DIRECTIONAL to the player
    private PlayerBody body;

    [SerializeField] float sensX = 200f;
    [SerializeField] float sensY = 200f;

    private float xRotation;
    private float yRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponentInChildren<PlayerBody>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
        HandleMouse();
    }

    private void HandleInputs()
    {
        // handle input from the input manager and send that down the line to the player body
        Vector2 move = InputManager.Instance.MoveVector;
        body.UpdateMove(move);

        //Fire Attacks
        if (InputManager.Instance.AttackPresed) body.Swipe();

        if (InputManager.Instance.AltAttack) body.Fire();
    }

    private void HandleMouse()
    {
        Vector2 mouse = InputManager.Instance.MousePos;

        float mouseX = mouse.x * Time.deltaTime * sensX;
        float mouseY = mouse.y * Time.deltaTime * sensY;

        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        body.Turret.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        body.UpdateTurret(body.Turret.forward);
    }
}
