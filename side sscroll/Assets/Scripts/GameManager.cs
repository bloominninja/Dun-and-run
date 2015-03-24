using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager o { get; private set; }

    public List<PlayerController> players;
    public List<Item> items;
    public Stage stage;

    void Awake ()
    {
        o = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Start ()
    {
        players = new List<PlayerController>();
        items = new List<Item>();


        foreach (PlayerController p in (PlayerController[])FindObjectsOfType(typeof(PlayerController)))
        {
            players.Add(p);
        }

        foreach (Item i in (Item[])FindObjectsOfType(typeof(Item)))
        {
            items.Add(i);
        }
    }
	
    // Update is called once per frame
    void Update ()
    {
	
    }
}
