using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Controller : Humanoid,IHeatable
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float jumpSpeed = 1f;
    [SerializeField]
    private Transform cameraPoint;
    [SerializeField]
    private float detectionTime;
    [SerializeField]
    private float portee;

    void Start()
    {
        health = 3;
    }

    void Update()
    {
        rl = 0f;
        fb = 0f;
        bool isGrounded = IsGrounded;
        //print(isGrounded);

        Vector3 toMouse = Input.mousePosition - mainCamera.WorldToScreenPoint(cameraPoint.position);
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
        bool attack = Input.GetKey(Raccourcis.attack) && !Archer;
        if (attack && !IsAttacking)
        {
            Attack(detectionTime,portee);
        }
        if(Archer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isReloading = true;
                isFireing = true;
            }
            if (isFireing && reloadTime < totalReloadTime)
            {
                reloadTime += Time.deltaTime;
            }
            else{ isReloading = false; }
            if (isFireing && Input.GetMouseButtonUp(0))
            {
                if (!isReloading) { bow.Fire(); }
                reloadTime = 0;
                isFireing = false;
            }

            IsAiming = isFireing && !isReloading;
            IsReloading = isFireing && isReloading;
        }
        else
        {
            IsAiming = false;
            IsReloading = false;
        }
        IsAttacking = attack;
        
        if(Input.GetKeyDown(Raccourcis.change)) 
        { 
            Archer = !Archer;
            if (Archer)
            {
                leftHand.GetComponent<LeftHand>().Bow.SetActive(true);
                leftHand.GetComponent<LeftHand>().Shield.SetActive(false);
                rightHand.GetComponent<RightHand>().sword.SetActive(false);
            }
            else
            {
                leftHand.GetComponent<LeftHand>().Bow.SetActive(false);
                leftHand.GetComponent<LeftHand>().Shield.SetActive(true);
                rightHand.GetComponent<RightHand>().sword.SetActive(true);
            }
        }
        //if (Input.GetKey(Raccourcis.attack)) { IsAttacking = true; }

        UpdateHumanoid(angle);
    }

    public void Heat(Vector3 direction, ArrowType type)
    {
        print("Im heat");
        if (IsDefending && Vector3.Dot(transform.forward, direction) < 0) { return; }
        health -= 1;
        if (health <= 0)
        {
            IsAlive = false;
            StartCoroutine(Respawn());
        }
    }
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(8f);
        UIFonctions.instance.Spawn();
        IsAlive = true;
        health = 3;
    }

    public Bow bow;

    [SerializeField] private float totalReloadTime;
    private float reloadTime = 0f;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxRotation;
    [SerializeField] private float minRotation;

    private bool isReloading = false;
    private bool isFireing = false;
}
