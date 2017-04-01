﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Beau Marwaha
/// Handles checking for collisions.
/// </summary>
public class CollisionHandler : MonoBehaviour {

    //attributes
    private GameObject player;
    private GameObject[] enemies;
    private GameObject[] pBullets; //player bullets
    private GameObject[] eBullets; //enemy bullets

    // Use this for initialization
    void Start()
    {
        //initialize attributes
        player = GameObject.Find("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        pBullets = GameObject.FindGameObjectsWithTag("pBullet");
        eBullets = GameObject.FindGameObjectsWithTag("eBullet");
    }

    // Update is called once per frame
    void Update()
    {
        //Check for collisions between the player and all enemies
        PlayerEnemyCollisionCheck();

        //Check for collisions between the player and all enemy bullets
        PlayerEBulletCollisionCheck();

        //Check for collisions between all enemies and all player bullets
        EnemyPBulletCollisionCheck();
    }

    /// <summary>
	/// Checks if two game objects are colliding using AABB collision.
	/// </summary>
	/// <returns><c>true</c>, if collision was AABBed, <c>false</c> otherwise.</returns>
	/// <param name="obj1">Obj1.</param>
	/// <param name="obj2">Obj2.</param>
	public bool AABBCollision(GameObject obj1, GameObject obj2)
    {
        //get the sprtie info scripts from each game object which hold corrected bounds of the sprite renderers
        SpriteInfo info1 = obj1.GetComponent<SpriteInfo>();
        SpriteInfo info2 = obj2.GetComponent<SpriteInfo>();

        //check for AABB collision
        if (info1.GetMinX() < info2.GetMaxX() &&
            info1.GetMaxX() > info2.GetMinX() &&
            info1.GetMinY() < info2.GetMaxY() &&
            info1.GetMaxY() > info2.GetMinY())
        {
            return true;
        }

        //if they are not colliding return false
        return false;
    }

    /// <summary>
    /// Check for AABB collisions between the player and all enemies
    /// </summary>
    private void PlayerEnemyCollisionCheck()
    {
        //for each enemy
        for (int i = 0; i < enemies.Length; i++)
        {
            //check for collision
            if (enemies[i].activeSelf && AABBCollision(player, enemies[i]))
            {
                //if colliding have the player take damage
                player.TakeDamage(enemies[i].Attack);
            }
        }
    }

    /// <summary>
    /// Check for AABB collisions between all enemies and all player bullets
    /// </summary>
    private void EnemyPBulletCollisionCheck()
    {
        //update bullet list
        pBullets = GameObject.FindGameObjectsWithTag("pBullet");

        //create a list to put bullets to be removed in
        List<GameObject> oldBullets = new List<GameObject>();

        //check all bullets
        foreach (GameObject bullet in pBullets)
        {
            //check all enemies
            for (int i = 0; i < enemies.Length; i++)
            {
                //check for collision
                if (enemies[i].activeSelf && AABBCollision(bullet, enemies[i]))
                {
                    enemies.TakeDamage(player.Attack);
                    bullet.SetActive(false);
                    oldBullets.Add(bullet);
                }
            }
        }

        //remove old bullets from screen
        for(int i = 0; i < oldBullets.Count; i++)
        {
            Destroy(oldBullets[i]);
        }
    }

    /// <summary>
    /// Check for AABB collisions between the player and all enemy bullets
    /// </summary>
    private void PlayerEBulletCollisionCheck()
    {
        //update bullet list
        eBullets = GameObject.FindGameObjectsWithTag("eBullet");

        //create a list to put bullets to be removed in
        List<GameObject> oldBullets = new List<GameObject>();

        //check all bullets
        foreach (GameObject bullet in eBullets)
        {
            if (AABBCollision(player, bullet))
            {
                //if colliding have the player take damage
                player.TakeDamage(enemies[0].Attack);
                bullet.SetActive(false);
                oldBullets.Add(bullet);
            }
        }

        //remove old bullets from screen
        for (int i = 0; i < oldBullets.Count; i++)
        {
            Destroy(oldBullets[i]);
        }
    }
}