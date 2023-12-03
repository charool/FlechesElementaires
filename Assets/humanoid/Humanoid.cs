using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    [SerializeField]
    protected CharacterController controller;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    protected Transform rightHand;
    [SerializeField]
    protected Transform leftHand;
    [SerializeField]
    protected Transform attackPoint;

    [SerializeField]
    public float stunStrength;
    [SerializeField]
    protected float speed = 1f;
    [SerializeField]
    private float rotSmooth = 0.2f;
    protected float jumpVelocity = 0f;
    [SerializeField]
    private float g = 1f;

    private Vector3 directionMov;
    protected Vector3 directionStun;
    private float smooth;
    [SerializeField]protected int health = 1;

    protected float rl = 0f;
    protected float fb = 0f;
    private bool _isAttacking = false;
    private bool _isJumping = false;
    private bool _isDefending = false;
    private bool _isAlive = true;
    private bool _archer = false;
    private bool _isAiming = false;
    private bool _isReloading = false;
    private bool _isStun = false;

    private (float, float) xzVelocity = (0f,0f);

    [SerializeField] protected float stunTime = 0.5f;

    protected void UpdateHumanoid(float angle)
    {
        bool isGrounded = IsGrounded;

        if (isGrounded && jumpVelocity < 0f) { jumpVelocity = -0.1f; } // need to be <
        else { jumpVelocity -= 9.81f * g * Time.deltaTime; }
        Vector3 jump = new Vector3(0f, jumpVelocity, 0f);

        if (transform.position.y < Map.instance.deepWaterLevel)
        {
            jump = new Vector3(0f, 0f, 0f);
            jumpVelocity = 0f;
            isGrounded = true;
        }
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref smooth, rotSmooth);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 direction;
        if (isGrounded)
        {
            angle /= -Mathf.Rad2Deg;
            if (fb > 1.5f && !IsAiming && !IsReloading) { direction = new Vector3(-fb * Mathf.Sin(angle), 0f, fb * Mathf.Cos(angle)); }
            else { direction = new Vector3(rl * Mathf.Cos(angle) - fb * Mathf.Sin(angle), 0f, rl * Mathf.Sin(angle) + fb * Mathf.Cos(angle)).normalized; }
        }
        else { direction = directionMov; }

        if (transform.position.y < Map.instance.waterLevel) 
        { 
            direction /= 2f;
        }
        if ((IsDefending)&& IsGrounded) { direction = Vector3.zero; }
        if (IsStun) { direction = directionStun * stunStrength; }

        if (direction.magnitude > 0.001f)
        {
            controller.Move((direction * speed + jump) * Time.deltaTime);
        }
        else
        {
            controller.Move((jump) * Time.deltaTime);
        }

        if((fb,rl) != xzVelocity) 
        {
            animator.SetFloat("x", fb);
            animator.SetFloat("z", rl);
            xzVelocity = (fb,rl);
        }

        directionMov = direction;
    }

    public void Attack(float detectionTime, float portee)
    {
        StartCoroutine(AttackDelay(detectionTime, portee));
    }

    protected void AttackDetection(float portee)
    {
        Collider[] hitObject = Physics.OverlapSphere(attackPoint.position, portee);
        foreach (Collider collider in hitObject)
        {
            IHitable target = collider.GetComponent<IHitable>();
            if(target != null && collider.gameObject != gameObject)
            {
                target.Hit(transform.forward, ArrowType.None);
            }
        }
    }

    private IEnumerator AttackDelay(float detectionTime,float portee)
    {
        yield return new WaitForSeconds(detectionTime);
        if(IsAttacking && !IsStun) { AttackDetection(portee); }
    }
    private IEnumerator Stun()
    {
        print("Stun");
        yield return new WaitForSeconds(stunTime);
        IsStun = false;
    }

    protected bool IsGrounded { get{ return controller.isGrounded || transform.position.y < Map.instance.deepWaterLevel; } }

    protected bool IsAttacking
    {
        get { return _isAttacking; }
        set
        {
            if (_isAttacking != value)
            {
                _isAttacking = value;
                animator.SetBool("isAttacking", value);
            }
        }
    }
    protected bool IsJumping
    {
        get { return _isJumping; }
        set
        {
            if (_isJumping != value)
            {
                _isJumping = value;
                //animator.SetBool("isJumping", value);
            }
        }
    }
    protected bool IsDefending
    {
        get { return _isDefending; }
        set
        {
            if (_isDefending != value)
            {
                _isDefending = value;
                animator.SetBool("isDefending", value);
            }
        }
    }
    protected bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            if (_isAlive != value)
            {
                _isAlive = value;
                animator.SetBool("isAlive", value);
            }
        }
    }
    protected bool Archer
    {
        get { return _archer; }
        set
        {
            if (_archer != value)
            {
                _archer = value;
                animator.SetBool("Archer", value);
            }
        }
    }
    protected bool IsReloading
    {
        get { return _isReloading; }
        set
        {
            if (_isReloading != value)
            {
                _isReloading = value;
                animator.SetBool("IsReloading", value);
            }
        }
    }
    protected bool IsAiming
    {
        get { return _isAiming; }
        set
        {
            if (_isAiming != value)
            {
                _isAiming = value;
                animator.SetBool("IsAiming", value);
            }
        }
    }
    protected bool IsStun
    {
        get { return _isStun; }
        set
        {
            if (_isStun != value)
            {
                _isStun = value;
                if(value)
                {
                    StartCoroutine(Stun());
                }
                //animator.SetBool("IsStun", value);
            }
        }
    }
}
