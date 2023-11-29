using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stat", menuName = "Stats/EnemyStat")]
public class EnemyStat : ScriptableObject
{
    public float changePaternTime;
    public float stopDistanceMax;
    public float stopDistanceMin;
    public float probaAttack;
    public float probaDefence;
    public float speed;
    public float triggerDistance;
    public float portee;
}
