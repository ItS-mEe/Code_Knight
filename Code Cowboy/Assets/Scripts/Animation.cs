﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Sprite[] sprites;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
    }
    public void animate(){
        StartCoroutine(Animate());
    }

    private IEnumerator Animate(){
        for(int i = 0; i<sprites.Length; i++){
            sr.sprite = sprites[i];
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(this.gameObject);
    }
}
