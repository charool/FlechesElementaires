using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Humanoid
{ 
    [SerializeField]
    private EnemyStat stat;

    public bool isTrigger = false;

    //patern

    private float lastPaternTime = 0f;
    private bool wantToAttack = false;
    private bool toFar = true;

    //

    private void Start()
    {
        speed = stat.speed;
    }

    public void Active()
    {
        gameObject.SetActive(true);
        lastPaternTime = Random.value * stat.changePaternTime;
    }

    private void Trigger()
    {
        isTrigger = true;
        ChangePatern((Player.instance.transform.position - transform.position).magnitude);
    }
    void Update()
    {
        Vector3 toPlayer = Player.instance.transform.position - transform.position;
        toPlayer.y = 0f;
        float distance = toPlayer.magnitude;
        toPlayer.Normalize();

        float angle = Vector3.Angle(Vector3.forward, toPlayer);
        if (toPlayer.x < 0f) { angle *= -1; }
        if(!isTrigger) { angle = 0f; }

        lastPaternTime += Time.deltaTime;
        if(lastPaternTime >= stat.changePaternTime && isDisponible)
        {
            ChangePatern(distance);
        }

        if (isTrigger)
        {
            if (distance > stat.stopDistanceMax && isDisponible) { ChangePatern(distance); }
            else if (distance < (stat.stopDistanceMax + stat.stopDistanceMin) / 2f && isDisponible && toFar)
            { ChangePatern(distance); }
            else if (distance < stat.stopDistanceMin && isDisponible)
            { ChangePatern(distance); }
        }
        else
        {
            if (distance < stat.triggerDistance) { Trigger(); }
        }

        UpdateHumanoid(angle);
    }

    private void ChangePatern(float distance)
    {
        print("changePatern");
        lastPaternTime = 0;
        if (isTrigger)
        {
            if(distance > stat.stopDistanceMax)
            {
                fb = 1f;
                //rl = 0f;
                wantToAttack = false;
                IsAttacking = false;
                IsDefending = false;
                toFar = true;
            }
            else if (distance > stat.stopDistanceMin)
            {
                if(Random.value < stat.probaAttack)
                {
                    wantToAttack = true;
                    lastPaternTime = float.MaxValue;
                    IsDefending = false;
                    fb = 1f;
                    rl = 0f;
                }
                else
                {
                    IsDefending = Random.value < stat.probaDefence;
                    rl = (float)Random.Range(-1, 2); // 2 exclude
                    fb = 0f;
                }
                toFar = false;
            }
            else
            {
                rl = 0f;
                fb = -1f;
            }
        }
        else
        {
            fb = Mathf.RoundToInt((float)Random.Range(-3, 4)/ 4.5f);
            rl = Mathf.RoundToInt((float)Random.Range(-3, 4) / 4.5f);
            print(fb + " " + rl);
        }
    }

    private bool isDisponible{ get{ return !wantToAttack && !IsAttacking; } }
}
