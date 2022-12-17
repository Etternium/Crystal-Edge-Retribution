using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public int maxHealth;
    public int damage;
    public int startingPoise;
    public float attackSpeed;
    public float attackRange;
}
