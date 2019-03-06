using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private int _myx, _myy;
    public int myx{
        get { return _myx;}
        set{
            _myx = value;
        }
    }
    public int myy{
        get { return _myy;}
        set{
            _myy = value;
        }
    }
    TileManager _tileManager;
    TileManager tileManager{
        get {
            if(_tileManager == null){
                _tileManager = GameObject.Find("Tile Manager(Clone)").GetComponent<TileManager>();
            }
            return _tileManager;
        }
    }
    SpriteRenderer sr;
    public Sprite idle, walking1, walking2, shooting1, shooting2, melee1, melee2, melee3;

    public GameObject hpDisplayPrefab, hpDisplay;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        myx = 0;
        myy = 0;
        print("in start method");
        hpDisplay = Instantiate(hpDisplayPrefab);
        hpDisplay.transform.SetParent(GameObject.Find("Canvas").transform);
        (hpDisplay.transform as RectTransform).anchoredPosition = new Vector2(10, -10);
    }

    void OnDestroy(){
        Destroy(hpDisplay);
        myy = 0;
        myx = 0;
    }

    public void turnLeft(){
        tileManager.turnLeft();
        int temp = myy;
        myy = tileManager.mapH-1-myx;
        myx = temp;
    }

    public void turnRight(){
        tileManager.turnRight();
        int temp = myx;
        myx = tileManager.mapW-1-myy;
        myy = temp;
    }

    public void move(){
        print("----- move -----");
        print("myx " + myx + ", myy " + myy);
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
            tileManager.entities[myx+1, myy] = tileManager.entities[myx, myy];
            tileManager.entities[myx, myy] = null;
            myx ++;
        }
        print("myx " + myx + ", myy " + myy);
    }

    public void attack(){
        for(int i = myx; i<tileManager.mapW; i++){
            if(tileManager.entities[i, myy] != null && tileManager.entities[i, myy].tag == "Enemy"){
                if(i-this.myx < 3){
                    // melee attack animation
                    if(myx+2 < tileManager.mapW){
                    StartCoroutine(attackAnimation(tileManager.entities[myx+1, myy], tileManager.entities[myx+2, myy]));

                    } else                     
                    StartCoroutine(attackAnimation(tileManager.entities[myx+1, myy], null));

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
        SceneManager.LoadScene("GameOver");
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
