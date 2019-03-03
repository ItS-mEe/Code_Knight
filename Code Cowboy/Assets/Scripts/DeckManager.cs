using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{

    public GameObject[] deck;
    public GameObject[] hand, active;
    public RectTransform content, activeContent, transitionPanel;
    public int cardHeight, padding;
    public bool running = false, _done = true;
    TileManager tileManager;
    public Text text;

    public void Start(){
        hand = new GameObject[6];
        active = new GameObject[6];
        tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        DrawNewHand();
    }

    public void DrawNewHand(){
        for(int i = 0; i<6; i++){
            int nextCard = ((int) (Random.value * deck.Length));
            print(nextCard);
            if(hand[i] == null)
                hand[i] = Instantiate(deck[nextCard]);
        }
    }

    public void Update(){
        if(running) return;
        ///* 
        for(int i = 0; i<6; i++){
            if(hand[i] != null){
                hand[i].transform.SetParent(content.transform);
                (hand[i].transform as RectTransform).anchoredPosition = new Vector2(15, - padding - (padding + cardHeight) * i);
            }
            if(active[i] != null){
                active[i].transform.SetParent(activeContent.transform);
                (active[i].transform as RectTransform).anchoredPosition = new Vector2(15, - padding - (padding + cardHeight) * i);
            }
        }
        //*/
    }

    public void move(GameObject other){
        if(running) return;
        for(int i = 0; i<6; i++){
            if(hand[i] == other){
                hand[i] = null;
                for(int j = 0; j<6; j++){
                    if(active[j] == null){
                        active[j] = other;
                        print("moved to active");
                        return;
                    }
                }
            }

            if(active[i] == other){
                active[i] = null;
                for(int j = 0; j<6; j++){
                    if(hand[j] == null){
                        hand[j] = other;
                        return;
                    }
                }
            }
        }
    }

    public void run(){
        StartCoroutine(RunCode());
    }

    public IEnumerator RunCode(){
        running = true;
        for(int i = 0; i<6; i++){
            if(active[i] != null){
                (active[i].transform as RectTransform).anchoredPosition = new Vector2(30, - padding - (padding + cardHeight) * i);
                active[i].SendMessage("execute");
                yield return new WaitForSeconds(1f);
                (active[i].transform as RectTransform).anchoredPosition = new Vector2(15, - padding - (padding + cardHeight) * i);
            }
        }
        int numNotNull = 0;
        foreach(GameObject enemy in tileManager.enemies)
            if(enemy != null) numNotNull ++;

        if(numNotNull == 0){
            text.text = "Level " + (tileManager.numEnemies - 1);
            transitionPanel.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            transitionPanel.gameObject.SetActive(false);
            tileManager.cleanup();
            tileManager.numEnemies += 1;
            tileManager.Awake();
            
        }
        else{
            foreach(GameObject enemy in tileManager.enemies){
                if(enemy == null) continue;
                yield return new WaitForSeconds(0.1f);
                enemy.SendMessage("TakeTurn", this.gameObject);
                _done = false;
                while(!_done){
                    yield return null;
                }
            }
        }
        

        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i<6; i++){
            if(active[i] != null)
                Destroy(active[i]);
            
        }
        running = false;
        DrawNewHand();
    }

    public void done(){
        _done = true;
    }

}
