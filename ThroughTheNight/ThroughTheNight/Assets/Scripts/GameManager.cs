﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Game Manager Singleton
/// Allows the access of variables at all times no matter the current scene
/// </summary>
public enum State { Day, Buy, Night, Message }
public class GameManager : MonoBehaviour {


    private List<string> messages = new List<string>();
    private int currentMessage = 0;

    public List<string> roomNames = new List<string>();

    public static GameManager GM;

    public Image[] hearts;

    //public Text health;
    public int healthNum;

    public Text time;
    public double timeNum;

    public Text remaining;
    public int remainNum;

    public Text objective;

    public State currentState;

    public Text[] textElements;
    public Font[] fonts;

    //Currently Unused
    private List<string> objectives = new List<string>();
    private List<string> currentObjectives;
    private int objectivesTotal;

    public Image background;
    public Image largeCat;
    public Text message;


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

        HideMessage();
        PopulatesMessages();

        objective = GameObject.Find("Objectives").GetComponent<Text>();
        objectives = new List<string>();
        objectives.Add("Cook");
        objectives.Add("Clean");
        objectives.Add("Shower");
        
        
        //health = GameObject.Find("Health").GetComponent<Text>();
        //health.text = "Health: " + healthNum;

        time = GameObject.Find("Time").GetComponent<Text>();
        time.text = "Time: " + timeNum.ToString("F2");

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
        ChangeState(State.Night);
        
    }
	
	// Update is called once per frame
	void Update () {
        if (currentState != State.Message)
        {
            ClearRoom();
            if (currentState == State.Night)
            {
                timeNum -= Time.deltaTime;
                time.text = "Time: " + timeNum.ToString("F2");

                //if time runs out end the game
                if(timeNum <= 0)
                {
                    //Destroy Player, UI, and GameManager
                    Destroy(GameObject.Find("Player"));
                    Destroy(GameObject.Find("Canvas"));
                    Destroy(GameObject.Find("GameManager"));

                    //Move to end screen
                    SceneManager.LoadScene(11);
                }
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (currentState == State.Night)
                {
                    ChangeState(State.Day);
                }
                else if (currentState == State.Day)
                {
                    ChangeState(State.Night);
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (currentState == State.Night)
                {
                    DisplayMessage();
                }
                else if (currentState == State.Message)
                {
                    HideMessage();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideMessage();
            }
            if (Input.GetMouseButtonDown(0) && messages.Count -1 != currentMessage)
            {
                currentMessage++;
                message.text = messages[currentMessage];
            }
            else if(Input.GetMouseButtonDown(0) && messages.Count - 1 == currentMessage)
            {
                HideMessage();
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
        //GameManager.GM.health.text = "Health: " + GameManager.GM.healthNum;
    }
    
    /// <summary>
    /// Called to decrease remaining enemy count once and enemy has been destroyed
    /// </summary>
    public void DefeatEnemy()
    {
        remainNum--;
        remaining.text = remainNum.ToString();

        //if enemy count runs out the player wins
        if (timeNum <= 0)
        {
            //Destroy Player, UI, and GameManager
            Destroy(GameObject.Find("Player"));
            Destroy(GameObject.Find("Canvas"));
            Destroy(GameObject.Find("GameManager"));

            //Move to win screen
            SceneManager.LoadScene(12);
        }
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
                    Camera.main.backgroundColor = new Color(0, 0, 0);
                    remaining.text = remainNum.ToString();
                    return;
                }
            case State.Message:
                {
                    message.text = messages[currentMessage];
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
           
        }
    }

    void HideMessage()
    {
        background.enabled = false;
        largeCat.enabled = false;
        message.enabled = false;
        ChangeState(State.Night);
    }

    void DisplayMessage()
    {
        ChangeState(State.Message);
        background.enabled = true;
        largeCat.enabled = true;
        message.enabled = true;
        
    }

    void PopulatesMessages()
    {
        messages.Add("Welcome to your dream *cough* nightmare *cough*. I recommend you don't try going back to sleep.");
        messages.Add("I'm Harenae, the Inanis have invaded your dream. You see that number in the bottom left corner?");
        messages.Add("It's how many still remain in your dream. If any are left by morning...");
        messages.Add("You aren't waking up.");
    }

    public void ClearRoom()
    {
        //Debug.Log(roomNames.Count);
        //for (int i = 0; i < roomNames.Count; i++)
        //{
        //Debug.Log(roomNames[i]);
        //}

        Debug.Log(SceneManager.GetActiveScene().name);
        //int j = 0;
        //while (j < 1000000000)
        //{
        //    j++;
        //}
        
        if (roomNames.Contains(SceneManager.GetActiveScene().name))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < enemies.Length;i++)
            {
                Destroy(enemies[i]);
            }
        }
        
    }
}
