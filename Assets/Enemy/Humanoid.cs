using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    [SerializeField]
    protected CharacterController controller;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    protected float speed = 1f;
    [SerializeField]
    private float rotSmooth = 0.2f;
    protected float jumpVelocity = 0f;
    [SerializeField]
    private float g = 1f;

    private Vector3 directionMov;
    private float smooth;

    protected float rl = 0f;
    protected float fb = 0f;
    private bool _isAttacking = false;
    private bool _isJumping = false;
    private bool _isDefending = false;
    private bool _isAlive = true;

    private (float, float) xzVelocity = (0f,0f);
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
            if (fb > 1.5f) { direction = new Vector3(-fb * Mathf.Sin(angle), 0f, fb * Mathf.Cos(angle)); }
            else { direction = new Vector3(rl * Mathf.Cos(angle) - fb * Mathf.Sin(angle), 0f, rl * Mathf.Sin(angle) + fb * Mathf.Cos(angle)).normalized; }
        }
        else { direction = directionMov; }

        if (transform.position.y < Map.instance.waterLevel) { direction /= 2f; }
        if (IsDefending) { direction /= 2f; }

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
}
