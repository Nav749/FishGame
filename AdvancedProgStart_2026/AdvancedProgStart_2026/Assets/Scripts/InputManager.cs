using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //Quick and Dirty Singleton
    public static InputManager Instance { get; private set; }

    //Hold our input system asset
    [SerializeField] private InputActionAsset inputActions;

    private InputActionMap playerMap;

    //input actions themselves
    private InputAction move, attack, altAttack;

    //public properties: variables that do not store a value, they call a functionality to get the value when accessed.
    public Vector2 MoveVector {get { return move.ReadValue<Vector2>(); }}
    public bool AttackPresed {get { return attack.WasPressedThisFrame(); }}
    public bool AltAttack{get { return altAttack.WasPressedThisFrame(); }}

    private void Awake()
    {
        //setting our singleton instance to this object
        Instance = this;

        //find out player map and actions
        playerMap = inputActions.FindActionMap("Player");

        move = playerMap.FindAction("Move");
        attack = playerMap.FindAction("Attack");
        altAttack = playerMap.FindAction("Alt Attack");
    }

    
}
