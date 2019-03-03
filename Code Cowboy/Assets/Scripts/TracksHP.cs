using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksHP : MonoBehaviour
{
    public int MAXHP, hp;

    void Start(){
        hp = MAXHP;
    }

    public void takeDamage(int dmg){
        hp -= dmg;
        if(hp <= 0){
            SendMessage("die");
        }
    }
}
