﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Kat Weis
/// Parent Class for the different characters in the game--abstract
/// </summary>
public abstract class Entity : MonoBehaviour
{
    //fields
    protected float speed;
    public int attack;
    protected float health;
    protected Vector3 velocity;
    protected Vector3 direction;

    // Use this for initialization
    protected abstract void Start();

    // Update is called once per frame
	protected abstract void Update();
	
    //method to move the entity
	protected abstract void Move();
	
    //method to handle when the entity dies
	protected abstract void Death();
	
    //method to handle when the entity is attacked
	public abstract void TakeDamage(int damageTaken);
	
    //method to handle when the entity attacks
	protected abstract void Attack();
}
