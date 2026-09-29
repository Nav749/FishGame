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

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 50f;

    [SerializeField] Bulllet[] bullet;
    [SerializeField] Sword sword;

    private float direction;
    public float Direction 
    {
        get { return direction; }
        set { direction = value; }
    }
    private float rotationDir;
    public float RotationDir
    {
        get { return rotationDir; }
        set { rotationDir = value; }
    }
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 moveDir = direction * moveSpeed * Time.fixedDeltaTime * transform.forward;

        rb.MovePosition(transform.position +  moveDir);

        Quaternion deltaRot = Quaternion.Euler(rotationDir * Time.fixedDeltaTime * new Vector3(0, rotationSpeed, 0));

        rb.MoveRotation(rb.rotation *  deltaRot);
    }

    public void Swipe()
    {
        Sword s = sword;

        if(s != null)
        {
            s = Instantiate(s.gameObject, fireLocation.position, Quaternion.identity).GetComponent<Sword>();
            s.gameObject.SetActive(true);
        }
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
