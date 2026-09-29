using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    #region Fields and Variables
    private Rigidbody rb;

    [SerializeField] private Transform fireLocation;
    [SerializeField] private GameObject turret;
    [HideInInspector] public Transform Turret { get { return turret.transform; } }

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] Bulllet[] bullet;
    [SerializeField] Sword sword;

    Vector2 moveInput;
    public Vector2 MoveInout
    {
        get
        {
            return moveInput;
        }
    }

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(transform.position +  moveDir);
    }

    public void Swipe()
    {
        Sword s = sword;

        if(s != null)
        {
            s = Instantiate(s.gameObject, fireLocation.position, Quaternion.identity).GetComponent<Sword>();
            s.transform.rotation = turret.transform.rotation;
            s.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// updates the turret's aim direction
    /// </summary>
    /// <param name="dir"> a direction to aim to</param>
    public void UpdateTurret(Vector3 dir)
    {
        dir.Normalize();
        turret.transform.forward = dir;
    }

    public void UpdateMove(Vector2 dir)
    {
        moveInput = dir;
    }

    public void Fire(Bulllet.BulletType type = Bulllet.BulletType.Default)
    {
        Vector3 dir = fireLocation.position - transform.position;
        
        dir.y = type == Bulllet.BulletType.Default ? 0f : 1.5f;
        dir.Normalize();

        Bulllet b = null;
        foreach(var blt  in bullet)
        {
            if(blt.type == type)
            {
                b = blt; 
                break;
            }
        }

        if(b != null)
        {
            b = Instantiate(b.gameObject, fireLocation.position, Quaternion.identity).GetComponent<Bulllet>();
            b.gameObject.SetActive(true);
            b.Fire(dir);
        }
    }
}
