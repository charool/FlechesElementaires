using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : Humanoid,IHitable
{ 
    [SerializeField]
    private EnemyStat stat;

    public bool isTrigger = false;
    [SerializeField]
    public float destroyTime;

    //patern

    private float lastPaternTime = 0f;
    private bool wantToAttack = false;
    private bool toFar = true;
    private bool goBack = false;

    //

    private void Start()
    {
        speed = stat.speed;
        health = stat.maxHealth;
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
        if (!IsAlive)
        {
            fb = 0f;
            rl = 0f;
            return;
        }
        if (IsStun)
        {
            fb = -1f * stunStrength;
            rl = 0f;
            print("s");
            wantToAttack = false;
            UpdateHumanoid(0f);
            return;
        }

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
            else if (distance < stat.stopDistanceMin && isDisponible && !IsDefending)
            { ChangePatern(distance); }

            if(wantToAttack && distance < 0.9f * stat.portee + 0.25f)
            {
                wantToAttack = false;
                IsAttacking = true;
                fb = 0f;
                Attack(stat.detectionTime,stat.portee);
                StartCoroutine(AttackDelay());
            }
        }
        else
        {
            if (distance < stat.triggerDistance) { Trigger(); }
        }

        UpdateHumanoid(angle);
    }
    void LateUpdate()
    {
        if (Map.type == MapType.Spawn) { return; }
        Vector3 pos = transform.position;
        Chunk chunk = Map.instance.GetChunk(pos);
        if(chunk == null) { return; }

        if (!chunk.gameObject.activeSelf)
        {
            chunk.AddEnemy(this.gameObject);
            gameObject.SetActive(false);
        }
    }
    [SerializeField] Tower tower;
    public void SetTower(Tower t)
    {
        tower = t;
    }
    public void Hit(Vector3 direction, ArrowType type)
    {
        print("HeatEnemy");
        if (IsDefending && Vector3.Dot(transform.forward, direction) < 0) { return; }
        if(stat.asElem && type != stat.weakness) { return; }
        IsAttacking = false;
        wantToAttack = false;
        IsStun = true;
        directionStun = direction;
        health -= 1;
        if (health <= 0)
        {
            IsAlive = false;
            tower.Remove(this);
            StartCoroutine(DestroyAftertime());
        }
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
                if (Random.value < stat.probaAttack)
                {
                    wantToAttack = true;
                    lastPaternTime = float.MaxValue;
                    IsDefending = false;
                    fb = 1f;
                    rl = 0f;
                }
                else if (Random.value < stat.probaDefence)
                {
                    IsDefending = true;
                    fb = 0f;
                }
                toFar = false;
            }
        }
        else
        {
            fb = Mathf.RoundToInt((float)Random.Range(-3, 4)/ 4.5f);
            rl = Mathf.RoundToInt((float)Random.Range(-3, 4) / 4.5f);
        }
    }
    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(stat.attackDuration);
        IsAttacking = false;
    }

    private IEnumerator DestroyAftertime()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    private bool isDisponible{ get{ return !wantToAttack && !IsAttacking; } }

}
