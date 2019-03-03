using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFrameOnDamage : MonoBehaviour
{
    
    public Sprite normal, damage;

    bool dead;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
    }

    public void takeDamage(int n){
        StartCoroutine(ShowFrame());
    }

    IEnumerator ShowFrame(){
        sr.sprite = damage;
        yield return new WaitForSeconds(0.1f);
        sr.sprite = normal;
        if(dead){
            Destroy(this.gameObject);
        }
    }

    public void die(){
        dead = true;
    }

}
