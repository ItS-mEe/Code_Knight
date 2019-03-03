using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int myx = 0, myy = 0;
    TileManager tileManager;
    SpriteRenderer sr;
    public Sprite idle, walking1, walking2, shooting1, shooting2, melee1, melee2, melee3;

    void Start()
    {
        tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        sr = this.GetComponent<SpriteRenderer>();
    }

    public void turnLeft(){
        tileManager.turnLeft();
        int temp = myy;
        myy = tileManager.mapH-1-myx;
        myx = temp;
        print("x " + myx + ", y " + myy);
    }

    public void turnRight(){
        tileManager.turnRight();
        int temp = myx;
        myx = tileManager.mapW-1-myy;
        myy = temp;
        print("x " + myx + ", y " + myy);
    }

    public void move(){
        if(myx == tileManager.mapW - 1){
            // couldn't move foreward
            return;
        }
        else if(tileManager.map[myx+1, myy].tag == "Mountain"){
            // couldn't move foreward
            return;
        }
        else if(tileManager.entities[myx+1, myy] != null){
            // couldn't move foreward
            return;
        }
        else{
            print(tileManager.entities[myx, myy]);
            tileManager.entities[myx+1, myy] = tileManager.entities[myx, myy];
            tileManager.entities[myx, myy] = null;
            myx ++;
        }
    }

    public void attack(){
        for(int i = myx; i<tileManager.mapW; i++){
            if(tileManager.entities[i, myy] != null && tileManager.entities[i, myy].tag == "Enemy"){
                if(i-this.myx < 3){
                    // melee attack animation
                    StartCoroutine(attackAnimation(tileManager.entities[myx+1, myy], tileManager.entities[myx+2, myy]));
                }
                else{
                    //ranged attack animation
                    StartCoroutine(rangedAttackAnimation(tileManager.entities[i, myy]));
                }
                return;
            }
            if(tileManager.map[i, myy].tag == "Mountain"){

                return; // can't attack through mountains
            }
        }
        // nothing to attack
        
    }

    public void die(){
        Application.LoadLevel("GameOver");
    }

    public IEnumerator rangedAttackAnimation(GameObject enemy){
        sr.sprite = shooting1;
        yield return new WaitForSeconds(0.4f);
        sr.sprite = shooting2;
        yield return new WaitForSeconds(0.4f);
        enemy.SendMessage("takeDamage", 1);
        sr.sprite = idle;
    }

    public IEnumerator attackAnimation(GameObject enemy, GameObject enemy1){
        sr.sprite = melee1;
        yield return new WaitForSeconds(0.3f);
        sr.sprite = melee2;
        if(enemy != null) enemy.SendMessage("takeDamage", 1);
        if(enemy1 != null) enemy1.SendMessage("takeDamage", 1);
        yield return new WaitForSeconds(0.3f);
        sr.sprite = melee3;
        yield return new WaitForSeconds(0.3f);
        sr.sprite = idle;
    }

}
