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

        if (InputManager.Instance.AltAttack) body.Fire(Bulllet.BulletType.Grenade);
    }

    private void HandleMouse()
    {
        Ray r = Camera.main.ScreenPointToRay(InputManager.Instance.MousePos);

        Plane plane = new Plane(Vector3.up, body.Turret.position);

        float distanceToPlane;

        if(plane.Raycast(r, out distanceToPlane))
        {
            //get the point along the ray that intersects with the plane
            Vector3 mouseWorldPos = r.GetPoint(distanceToPlane);

            //Calculate an aim dirction for turret
            Vector3 aimDir = mouseWorldPos - body.Turret.position;
            aimDir.y = 0f;

            //update turret pos
            body.UpdateTurret(aimDir);

            //draw debug lines
            Debug.DrawRay(body.transform.position, mouseWorldPos, Color.red);
            Debug.DrawRay(r.origin, mouseWorldPos, Color.green);

            Debug.DrawRay(mouseWorldPos, Vector3.back, Color.blue);
            Debug.DrawRay(mouseWorldPos, Vector3.left, Color.blue);
        }
    }
}
