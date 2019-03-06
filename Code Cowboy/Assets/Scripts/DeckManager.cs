using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{

    public GameObject[] initialDeck;
    private IDictionary<GameObject, int> deck, discard;
    private int deckSize;
    public GameObject[] hand, prefabHand, active, prefabActive;
    public int handSize, activeSize;
    public RectTransform content, activeContent, transitionPanel;
    public int cardHeight, padding;
    public bool running = false, _done = true;
    public TileManager tileManager;
    public GameObject tileManagerPrefab;
    public Text text;
    public int currentlevel = 1;

    public void Start(){
        deck = new Dictionary<GameObject, int>();
        discard = new Dictionary<GameObject, int>();
        deckSize = initialDeck.Length;
        for(int i = 0; i<initialDeck.Length; i++){
            if(deck.ContainsKey(initialDeck[i])){
                deck[initialDeck[i]] = deck[initialDeck[i]]+1;
            }
            else{
                deck.Add(initialDeck[i], 1);
                discard.Add(initialDeck[i], 0);
            }
        }
        printDeck();
        hand = new GameObject[handSize];
        prefabHand = new GameObject[handSize];
        active = new GameObject[activeSize];
        prefabActive = new GameObject[activeSize];
        tileManager = Instantiate(tileManagerPrefab).GetComponent<TileManager>();
        tileManager.LoadLevel(currentlevel);
        DrawNewHand();
    }

    public void printDeck(){
        print("------------- deck");
        foreach( KeyValuePair<GameObject, int> kvp in deck )
        {
            print(kvp.Key + ": " + kvp.Value);
        }

        print("------------- discard");
        foreach( KeyValuePair<GameObject, int> kvp in discard )
        {
            print(kvp.Key + ": " + kvp.Value);
        }
    }

    private GameObject DrawNextCard(){
        int nextCard = ((int) (Random.value * deckSize));
        ICollection<GameObject> keys = deck.Keys;
        foreach(GameObject key in keys){
            nextCard -= deck[key];
            if(nextCard < 0){
                deck[key] -= 1;
                deckSize--;
                return key;
            }
        }
        return null;
    }

    public void DrawNewHand(){
        for(int i = 0; i<6; i++){
            if(hand[i] == null){
                prefabHand[i] = DrawNextCard();
                hand[i] = Instantiate(prefabHand[i]);
                hand[i].GetComponent<Card>().setup(this, tileManager.player.GetComponent<PlayerController>());
            }
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
                for(int j = 0; j<6; j++){
                    if(active[j] == null){
                        prefabActive[j] = prefabHand[i];
                        active[j] = other;
                        return;
                    }
                }
                hand[i] = null;
                prefabHand[i] = null;
            }

            if(active[i] == other){
                for(int j = 0; j<6; j++){
                    if(hand[j] == null){
                        prefabHand[j] = prefabActive[i];
                        hand[j] = other;
                        return;
                    }
                }
                active[i] = null;
                prefabActive[i] = null;
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
            currentlevel ++;
            text.text = "Level " + (currentlevel);
            transitionPanel.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            transitionPanel.gameObject.SetActive(false);
            Destroy(tileManager.gameObject);
            tileManager = Instantiate(tileManagerPrefab).GetComponent<TileManager>();
            tileManager.LoadLevel(currentlevel);
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
