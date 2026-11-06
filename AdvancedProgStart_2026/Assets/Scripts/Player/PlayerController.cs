using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //send data ONE DIRECTIONAL to the player
    private PlayerBody body;

    [Range(150f, 500f), Tooltip("The sens for the camera")]
    [SerializeField] float sens = 200f;

    //the x and y rotation values
    private float xRotation;
    private float yRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponentInChildren<PlayerBody>();
        Cursor.lockState = CursorLockMode.Locked;
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

    /// <summary>
    /// handles the mouse inputs based on the input manager mouse value
    /// </summary>
    private void HandleMouse()
    {
        //get the mouse value reference
        Vector2 mouse = InputManager.Instance.MousePos;

        //set the amount of rotation for the mouse
        float mouseX = mouse.x * Time.deltaTime * sens;
        float mouseY = mouse.y * Time.deltaTime * sens;

        //sets the rotation for the y-rotation
        yRotation += mouseX;

        //sets the rotation for the x rotation between -90 and 90
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotates the body with the y rotation and the x rotation clamped again between 20 and 20
        body.transform.rotation = Quaternion.Euler(Mathf.Clamp(xRotation, -20f, 20f), yRotation, 0);

        //rotates the turret to aim properly
        body.Turret.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        body.UpdateTurret(body.Turret.forward);
    }
}
