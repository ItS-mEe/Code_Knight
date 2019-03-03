using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public PlayerController player;
    public DeckManager deckManager;

    void Start(){
        player = GameObject.Find("Player(Clone)").GetComponent<PlayerController>();
        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
    }

    public void move(){
        print("got message");
        deckManager.move(this.gameObject);
    }

    public abstract void execute();
}
