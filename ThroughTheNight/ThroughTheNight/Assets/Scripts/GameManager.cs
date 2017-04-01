﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Game Manager Singleton
/// 
/// Allows the access of variables at all times no matter the current scene
/// 
/// 
/// </summary>
public class GameManager : MonoBehaviour {

    public enum State { Day, Buy, Night } 

    public static GameManager GM;

    public Image[] hearts;

    public Text health;
    public int healthNum;

    public Text time;
    public double timeNum;

    public Text remaining;
    public int remainNum;

    public Text objective;

    public State currentState;

    public Text[] textElements;
    public Font[] fonts;

    private List<string> objectives = new List<string>();
    private List<string> currentObjectives;
    private int objectivesTotal;
    void Awake()
    {
        CreateGameManager();
    }


    void CreateGameManager()
    {
        if (GM == null)
        {
            DontDestroyOnLoad(gameObject);
            GM = this;
        }
        else
        {
            if (GM != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // Use this for initialization
    void Start () {
        objective = GameObject.Find("Objectives").GetComponent<Text>();
        objectives = new List<string>();
        objectives.Add("Cook");
        objectives.Add("Clean");
        objectives.Add("Shower");
        
        
        health = GameObject.Find("Health").GetComponent<Text>();
        health.text = "Health: " + healthNum;

        time = GameObject.Find("Time").GetComponent<Text>();
        time.text = "Time: " + timeNum;

        remaining = GameObject.Find("Remaining").GetComponent<Text>();
        remaining.text = remainNum.ToString();

        for(int i = 0; i < objectives.Count; i++)
        {
            objective.text += "     " + objectives[i] + "\n";
        }
        objectivesTotal = objectives.Count;

        currentObjectives = new List<string>(objectives);

        RemoveObjective("Cook");

        for(int i = 0; i <hearts.Length; i++)
        {
            hearts[i].enabled = false;
        }
        for (int i = 0; i < healthNum; i++)
        {
            hearts[i].enabled = true;
        }
        //Starting at Day
        ChangeState(State.Day);
        
    }
	
	// Update is called once per frame
	void Update () {
        if(currentState == State.Night)
        {
            timeNum -= Time.deltaTime;
            time.text = "Time: " + timeNum.ToString("F2");
        }
       
        if (Input.GetKeyDown(KeyCode.L))
        {
            if(currentState == State.Night)
            {
                ChangeState(State.Day);
            }else if(currentState == State.Day)
            {
                ChangeState(State.Night);
            }
            
        }
    }

    /// <summary>
    /// Change the health of the player. Use negatives to decrease health
    /// </summary>
    /// <param name="health">Amount of health the player loses/gains </param>
    public void ChangeHealth(int health)
    {
        if(health == 0)//if input is zero set health to zero instead of changing it by zero
        {
            GameManager.GM.healthNum = health;
        }
        else
        {
            GameManager.GM.healthNum += health;
        }
        if(healthNum > 16)
        {
            healthNum = 16;
        }
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = false;
        }
        for (int i = 0; i < healthNum; i++)
        {
            hearts[i].enabled = true;
        }
        GameManager.GM.health.text = "Health: " + GameManager.GM.healthNum;
    }
    
    /// <summary>
    /// Called to decrease remaining enemy count once and enemy has been destroyed
    /// </summary>
    public void DefeatEnemy()
    {
        remainNum--;
        remaining.text = remainNum.ToString();
    }


    /// <summary>
    /// Change the game between it's seperate states
    /// </summary>
    /// <param name="change">The state to change the game to</param>
    void ChangeState(State change)
    {
        currentState = change;
        switch (currentState)
        {
            case State.Day:
                { 
                    for(int i = 0; i < textElements.Length; i++)
                    {
                        textElements[i].GetComponent<Text>().font = fonts[1];
                    }                   
                    Camera.main.backgroundColor = new Color(155, 155, 155);
                    remaining.text = "0";
                    return;
                }
            case State.Night:
                {
                    for (int i = 0; i < textElements.Length; i++)
                    {
                        textElements[i].GetComponent<Text>().font = fonts[0];                        
                    }
                    Camera.main.backgroundColor = new Color(125, 0, 0);
                    remaining.text = remainNum.ToString();
                    return;
                }
            case State.Buy:
                {
                    return;
                }
        }

    }

    /// <summary>
    /// Removes a completed objective from the current list of objectives
    /// </summary>
    /// <param name="obj"></param>
    void RemoveObjective(string obj)
    {
        if (currentObjectives.Contains(obj))
        {
            currentObjectives.Remove(obj);
            objective.text = "Objectives: \n";
            for (int i = 0; i < currentObjectives.Count; i++)
            {
                objective.text += "     " + objectives[i] + "\n";
            }
            Debug.Log(objectives.Count);
        }
    }

}
