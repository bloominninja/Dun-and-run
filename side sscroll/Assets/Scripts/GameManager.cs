using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager o { get; private set; }

    public List<PlayerController> players;
    public PlayerData[] playerData;
    public List<Item> items;
    public Stage stage;
    public int numPlayers = 0;
    public bool pause = false;
    private float endTimer = 0f;
    public bool end = false;
	
	public bool fillAI = false;
	
	public AiTrainerManager aiBroodmother = new AiTrainerManager();//do not interfere!

    void Awake ()
    {
        if (GameManager.o != null)
            Destroy(gameObject);
        o = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        playerData = new PlayerData[4];
        for (int i = 0; i < 4; i++)
            playerData[i] = new PlayerData();
        players = new List<PlayerController>();
        items = new List<Item>();
		
		aiBroodmother.gameManager = this;
		aiBroodmother.Start();

        /*foreach (PlayerController p in (PlayerController[])FindObjectsOfType(typeof(PlayerController)))
        {
            players.Add(p);
        }

        foreach (Item i in (Item[])FindObjectsOfType(typeof(Item)))
        {
            items.Add(i);
        }*/
    }
	
	public void OnDestroy()
	{
		//destroy our aiBroodmother
		aiBroodmother.OnDestroy();
	}
	
    void Update ()
    {
        if (end)
        {
            endTimer -= Time.deltaTime;
            if (endTimer <= 0)
            {
                end = false;
                ChangeScene(0);
            }
        }
        else if (stage != null)
        {
            int i = 0;
            foreach (PlayerController p in players)
            {
                if (p.currentHealth > 0)
                    i += 1;
            }
            if (i <= 1)
            {
                end = true;
                endTimer = 2;
            }
        }
		
		//tick the ai broodmother
		aiBroodmother.Update();
    }

    public int AddPlayer (string control, int team)
    {
        int i;
        for (i=0; i<4 && playerData[i].active == true; i++)
            ;
        if (i < 4)
            playerData[i].Activate(control, team);
        numPlayers++;
        return i;
    }

    public void RemovePlayer (int num)
    {

        playerData[num].Deactivate();
        numPlayers--;
    }

    public int FindPlayer (string control)
    {
        for (int i = 0; i < playerData.Length; i++)
        {
            if (playerData[i].active && playerData[i].control == control)
                return i;
        }
        return -1;
    }

    public void ChangeScene (int scene)
    {
        stage = null;
        players.Clear();
        items.Clear();
        if (scene == 1)
            Application.LoadLevel("Hud" + Mathf.Floor(2).ToString());
        else if (scene == 0)
            Application.LoadLevel("Menu");
    }

    public void LoadStage ()
    {
        stage = FindObjectOfType<Stage>();
        PlayerBattleInterfaceScript[] hud = FindObjectsOfType<PlayerBattleInterfaceScript>();
		
		if(fillAI)
		{
			for (int i = 0; i < 4; i++)
			{
				if (playerData == null)
					continue;
				if (playerData[i] == null)
					continue;
				if (!playerData[i].active)
				{
					//if inactive, make them active and set their input to AI
					AddPlayer ("AI", i+6);
				}
			}
		}
		
        for (int i = 0; i < 4; i++)
        {
			if (playerData == null)
				continue;
            if (playerData[i] == null)
                continue;
			if(!playerData[i].active)
				continue;
			
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Characters/Player Hero");
            PlayerController p = Instantiate(prefab).GetComponent<PlayerController>();
            players.Add(p);
			
			//set the appropriate links for the broodmother
			if(playerData[i].control.Equals("AI"))
				//set the appropriate links for the broodmother
				aiBroodmother.linkAI(players[i].ai, i+1);
			else
				aiBroodmother.linkAI(null, i+1);//this will reset the ai if not in use
			
            p.transform.position = stage.playerSpawns[i].transform.position;
            p.team = playerData[i].team;
            p.inputType = playerData[i].control;
            foreach (PlayerBattleInterfaceScript h in hud)
            {
                if (h.name == "Player " + (i + 1).ToString() + " Battle UI")
                {
                    h.connectedPlayer = p;
                    break;
                }
            }
        }

        Item[] itemarr = Resources.LoadAll<Item>("Prefabs/Items");
        for (int i = 0; i < stage.chestSpawns.Length; i++)
        {
            Item itemtemp = Instantiate(itemarr[(int)Mathf.Floor(Random.Range(0, itemarr.Length))]).GetComponent<Item>();
            items.Add(itemtemp);
            itemtemp.transform.position = stage.chestSpawns[i].transform.position;
        }
    }
}

public class PlayerData
{
    public bool active = false;
    public string control;
    public int team;
    //character

    public PlayerData ()
    {

    }

    public void Activate (string c, int t)
    {
        active = true;
        control = c;
        team = t;
    }

    public void Deactivate ()
    {
        active = false;
    }
}