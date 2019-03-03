using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPDisplay : MonoBehaviour
{
    TracksHP player;

    Image sr;

    public Sprite hp1, hp2, hp3, hp4;

    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<Image>();
        player = GameObject.Find("Player(Clone)").GetComponent<TracksHP>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null || player.gameObject == null){
            player = GameObject.Find("Player(Clone)").GetComponent<TracksHP>();
        }
        if(player.hp == 4){
            sr.sprite = hp4;
        } else if(player.hp == 3){
            sr.sprite = hp3;
        } else if(player.hp == 2){
            sr.sprite = hp2;
        } else if(player.hp == 1){
            sr.sprite = hp1;
        }
    }
}
