using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Controller : Humanoid
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float jumpSpeed = 1f;

    void Update()
    {
        rl = 0f;
        fb = 0f;
        bool isGrounded = IsGrounded;
        //print(isGrounded);

        Vector3 toMouse = Input.mousePosition - mainCamera.WorldToScreenPoint(this.transform.position);
        toMouse.z = 0f;
        toMouse.Normalize();

        if (IsJumping && isGrounded) { IsJumping = false; }

        if (isGrounded && Input.GetKey(Raccourcis.up)) 
        { 
            jumpVelocity = jumpSpeed; 
            IsJumping = true;
        }

        float angle = Vector3.Angle(Vector3.up, toMouse);
        if (toMouse.x < 0f)
        {
            angle *= -1;
        }
        if (isGrounded || (Mathf.Abs(jumpVelocity +0.1f) < 2f && !IsJumping))
        {
            if (Input.GetKey(Raccourcis.rigth)) { rl += 1f; }
            if (Input.GetKey(Raccourcis.left)) { rl -= 1f; }
            if (Input.GetKey(Raccourcis.back)) { fb -= 1f; }
            if (Input.GetKey(Raccourcis.forward)) 
            { 
                fb += 1f;
                if (Input.GetKey(Raccourcis.sprint)) { fb = 2f; }
            }
        }
        IsDefending = Input.GetKey(Raccourcis.defend);
        IsAttacking = Input.GetKey(Raccourcis.attack);
        //if (Input.GetKey(Raccourcis.attack)) { IsAttacking = true; }

        UpdateHumanoid(angle);
    }

    private void attack()
    {

    }
}
