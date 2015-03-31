using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuCharSelect : MonoBehaviour
{
    public GameObject[] players;
    public MenuCharSelectWindow[] windows;


    private int i;

    void Awake ()
    {
        if (GameManager.o == null)
        {
            GameObject g = new GameObject("Game Manager");
            g.AddComponent<GameManager>();
        }
    }
    void Start ()
    {
        windows = new MenuCharSelectWindow[4];
        for (i = 0; i<4; i++)
        {
            windows[i] = new MenuCharSelectWindow();
            windows[i].Set(players[i].gameObject, "Player " + (i + 1).ToString());
        }
        if (GameManager.o.numPlayers > 0)
        {
            for (i = 0; i<GameManager.o.playerData.Length; i++)
            {
                if (!GameManager.o.playerData[i].active)
                    continue;
                if (GameManager.o.playerData[i].control == "Keyboard")
                    windows[i].On("Keyboard", "The Destined Hero", "Press X/Escape to quit");
                else
                    windows[i].On(GameManager.o.playerData[i].control, "The Destined Hero", "Press B to quit");
            }
        }
    }
	
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetButtonDown("Menu Confirm"))
        {
            if (Input.GetButtonDown("Joy1 Confirm"))
            {
                if (GameManager.o.FindPlayer("Joy1") == -1)
                {
                    i = GameManager.o.AddPlayer("Joy1", 1);
                    windows[i].On("Joy1", "The Destined Hero", "Press B to quit");
                }
            }
            if (Input.GetButtonDown("Joy2 Confirm"))
            {
                if (GameManager.o.FindPlayer("Joy2") == -1)
                {
                    i = GameManager.o.AddPlayer("Joy2", 2);
                    windows[i].On("Joy2", "The Destined Hero", "Press B to quit");
                }
            }
            if (Input.GetButtonDown("Joy3 Confirm"))
            {
                if (GameManager.o.FindPlayer("Joy3") == -1)
                {
                    i = GameManager.o.AddPlayer("Joy3", 3);
                    windows[i].On("Joy3", "The Destined Hero", "Press B to quit");
                }
            }
            if (Input.GetButtonDown("Joy4 Confirm"))
            {
                if (GameManager.o.FindPlayer("Joy4") == -1)
                {
                    i = GameManager.o.AddPlayer("Joy4", 4);
                    windows[i].On("Joy4", "The Destined Hero", "Press B to quit");
                }
            }
            if (Input.GetButtonDown("KB Confirm"))
            {
                if (GameManager.o.FindPlayer("Keyboard") == -1)
                {
                    i = GameManager.o.AddPlayer("Keyboard", 5);
                    windows[i].On("Keyboard", "The Destined Hero", "Press X/Escape to quit");
                }
            }
        }
        else if (Input.GetButtonDown("Menu Cancel"))
        {
            if (Input.GetButtonDown("Joy1 Cancel"))
            {
                i = GameManager.o.FindPlayer("Joy1");
                if (i >= 0)
                {
                    GameManager.o.RemovePlayer(i);
                    windows[i].Off();
                }
            }
            if (Input.GetButtonDown("Joy2 Cancel"))
            {
                i = GameManager.o.FindPlayer("Joy2");
                if (i >= 0)
                {
                    GameManager.o.RemovePlayer(i);
                    windows[i].Off();
                }
            }
            if (Input.GetButtonDown("Joy3 Cancel"))
            {
                i = GameManager.o.FindPlayer("Joy3");
                if (i >= 0)
                {
                    GameManager.o.RemovePlayer(i);
                    windows[i].Off();
                }
            }
            if (Input.GetButtonDown("Joy4 Cancel"))
            {
                i = GameManager.o.FindPlayer("Joy4");
                if (i >= 0)
                {
                    GameManager.o.RemovePlayer(i);
                    windows[i].Off();
                }
            }
            if (Input.GetButtonDown("KB Cancel"))
            {
                i = GameManager.o.FindPlayer("Keyboard");
                if (i >= 0)
                {
                    GameManager.o.RemovePlayer(i);
                    windows[i].Off();
                }
            }

        }
        
        if (Input.GetButtonDown("KB Start") && GameManager.o.FindPlayer("Keyboard") >= 0 && GameManager.o.numPlayers >= 2)
        {
            GameManager.o.ChangeScene(1);
        }
        else if (Input.GetButtonDown("Joy1 Start") && GameManager.o.FindPlayer("Joy1") >= 0 && GameManager.o.numPlayers >= 2)
        {
            GameManager.o.ChangeScene(1);
        }
        else if (Input.GetButtonDown("Joy2 Start") && GameManager.o.FindPlayer("Joy2") >= 0 && GameManager.o.numPlayers >= 2)
        {
            GameManager.o.ChangeScene(1);
        }
        else if (Input.GetButtonDown("Joy3 Start") && GameManager.o.FindPlayer("Joy3") >= 0 && GameManager.o.numPlayers >= 2)
        {
            GameManager.o.ChangeScene(1);
        }
        else if (Input.GetButtonDown("Joy4 Start") && GameManager.o.FindPlayer("Joy4") >= 0 && GameManager.o.numPlayers >= 2)
        {
            GameManager.o.ChangeScene(1);
        }
    }
}

public class MenuCharSelectWindow
{
    public GameObject window;
    public Text textPlayer;
    public Text textControl;
    public Text textCharacter;
    public Text textQuit;
    public Text textJoin;
    public Image windowInside;
    public Color colorWindow;
    public Color colorPlayer;
    public Color colorText;

    public MenuCharSelectWindow ()
    {

    }

    public void Set (GameObject o, string player)
    {
        window = o;
        Text[] ta = o.GetComponentsInChildren<Text>();
        foreach (Text t in ta)
        {
            if (t.name == "Player")
                textPlayer = t;
            else if (t.name == "Control")
                textControl = t;
            else if (t.name == "Character")
                textCharacter = t;
            else if (t.name == "Quit")
                textQuit = t;
            else if (t.name == "Join")
                textJoin = t;
        }
        textPlayer.text = player;
        colorText = textPlayer.color;

        Image[] ia = o.GetComponentsInChildren<Image>();
        foreach (Image i in ia)
        {
            if (i.name == "Inside")
                windowInside = i;
        }
        colorPlayer = window.GetComponent<Image>().color;
        colorWindow = windowInside.color;

        Off();
    }

    public void On (string control, string character, string quit)
    {
        textControl.text = control;
        textCharacter.text = character;
        textQuit.text = quit;

        textPlayer.color = colorText;
        textControl.color = colorText;
        textCharacter.color = colorText;
        textQuit.color = colorText;
        textJoin.color = Color.clear;
        
        window.GetComponent<Image>().color = colorPlayer;
    }

    public void Off ()
    {
        textPlayer.color = Color.clear;
        textControl.color = Color.clear;
        textCharacter.color = Color.clear;
        textQuit.color = Color.clear;
        textJoin.color = colorText;

        window.GetComponent<Image>().color = colorWindow;
    }

}