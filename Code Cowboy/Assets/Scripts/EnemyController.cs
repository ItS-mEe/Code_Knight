using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    PlayerController player;
    SpriteRenderer sr;
    public Sprite attack1, attack2;

    TileManager tileManager;
    public GameObject attackAnimation;


    public void Start(){
        tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        sr = this.GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player(Clone)").GetComponent<PlayerController>();
    }

    public void done(){
        TakeTurn(this.gameObject);
    }

    public void TakeTurn(GameObject toNotify){
        StartCoroutine(Turn(toNotify));
    }

    public IEnumerator Turn(GameObject toNotify){
        print(this);
        int playerx = player.myx;
        int playery = player.myy;

        Vector2Int mypos = tileManager.find(this.gameObject);
        int toMove;
        if(Mathf.Abs(mypos.x - playerx) > Mathf.Abs(mypos.y - playery)){
            // move in the y
            if(mypos.y == playery && Mathf.Abs(mypos.x - playerx) >= 5){
                toMove = 2;
                while(toMove > 0){
                    toMove --;
                    yield return new WaitForSeconds(0.1f);
                    if(playerx < mypos.x)
                        mypos = move(mypos.x, mypos.y, mypos.x-1, mypos.y);
                    else
                        mypos = move(mypos.x, mypos.y, mypos.x+1, mypos.y);
                }
            }
            else if (mypos.y - playery > 0){
                // go up
                if (mypos.y - playery > 2 ){
                    toMove = 2;
                }
                else {
                    toMove = mypos.y - playery;
                }
                while(toMove > 0){
                    toMove --;
                    yield return new WaitForSeconds(0.1f);
                    mypos = move(mypos.x, mypos.y, mypos.x, mypos.y - 1);
                }
            } else {
                // go down
                if (mypos.y - playery < -2 ){
                    toMove = 2;
                }
                else {
                    toMove = - mypos.y + playery;
                }
                while(toMove > 0){
                    toMove --;
                    yield return new WaitForSeconds(0.1f);
                    mypos = move(mypos.x, mypos.y, mypos.x, mypos.y + 1);
                }
            }
            print("made it here");
            if(playery == mypos.y && Mathf.Abs(mypos.x - playerx) < 5){
                                            print("made it here");

                yield return new WaitForSeconds(0.1f);
                            print("made it here");

                // attack player
                sr.sprite = attack2;
                yield return new WaitForSeconds(0.2f);
                            print("made it here");

                sr.sprite = attack1;
                //spawn projectile
                GameObject atkAnim = Instantiate(attackAnimation);
                atkAnim.transform.position = player.transform.position;
                yield return new WaitForSeconds(0.2f);
                            print("made it here");

                player.SendMessage("takeDamage", 1);
            }
        } else {
            //move in the x

            if(mypos.x == playerx && Mathf.Abs(mypos.y - playery) >= 5){
                toMove = 2;
                while(toMove > 0){
                    toMove --;
                    yield return new WaitForSeconds(0.1f);
                    if(playery < mypos.y)
                        mypos = move(mypos.x, mypos.y, mypos.x, mypos.y-1);
                    else
                        mypos = move(mypos.x, mypos.y, mypos.x, mypos.y+1);
                }
            }
            else if (mypos.x - playerx > 0){
                // go left
                if (mypos.x - playerx > 2 ){
                    toMove = 2;
                }
                else {
                    toMove = mypos.x - playerx;
                }
                while(toMove > 0){
                    toMove --;
                    yield return new WaitForSeconds(0.1f);
                    mypos = move(mypos.x, mypos.y, mypos.x - 1, mypos.y);
                }
            } else {
                // go right
                if (mypos.x - playerx < -2 ){
                    toMove = 2;
                }
                else {
                    toMove = -mypos.x + playerx;
                }
                while(toMove > 0){
                    toMove --;
                    yield return new WaitForSeconds(0.1f);
                    mypos = move(mypos.x, mypos.y, mypos.x + 1, mypos.y);
                }
            }

            if(playerx == mypos.x && Mathf.Abs(mypos.y - playery) < 5){
                yield return new WaitForSeconds(0.1f);
                // attack player
                sr.sprite = attack2;
                yield return new WaitForSeconds(0.2f);
                sr.sprite = attack1;
                //spawn projectile
                GameObject atkAnim = Instantiate(attackAnimation);
                atkAnim.transform.position = player.transform.position;
                yield return new WaitForSeconds(0.2f);
                player.SendMessage("takeDamage", 1);
            }
        }
        yield return new WaitForSeconds(0.1f);
        toNotify.SendMessage("done");
    }

    private Vector2Int move(int myx, int myy, int tx, int ty){
        if(ty >= tileManager.mapW || tx >= tileManager.mapW || ty < 0 || tx < 0){
            // couldn't move foreward
            return new Vector2Int(myx, myy);
        }
        else if(tileManager.map[tx, ty].tag == "Mountain"){
            // couldn't move foreward
            return new Vector2Int(myx, myy);
        }
        else if(tileManager.entities[tx, ty] != null){
            // couldn't move foreward
            return new Vector2Int(myx, myy);
        }
        else{
            tileManager.entities[tx, ty] = this.gameObject;
            tileManager.entities[myx, myy] = null;
        }
        return new Vector2Int(tx, ty);
    }

}
