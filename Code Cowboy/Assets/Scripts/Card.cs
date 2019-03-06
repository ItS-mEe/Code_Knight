using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public PlayerController player{
        get {return deckManager.tileManager.player.GetComponent<PlayerController>(); }
    }
    public DeckManager deckManager;

    public void setup(DeckManager dm, PlayerController pc){
        deckManager = dm;
    }

    public void move(){
        deckManager.move(this.gameObject);
    }

    public abstract void execute();
}
