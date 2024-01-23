using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeRenderer : MonoBehaviour
{
    public float offset = 0.15f;
    SpriteRenderer sr;

    void Start(){
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnTriggerStay2D(Collider2D other) {
        List<string> possible = new List<string>{"Enemy", "Player"};
        if (possible.Contains(other.tag) && sr != null) {
            SpriteRenderer other_sr = other.gameObject.GetComponent<SpriteRenderer>();

            float height = other.transform.position.y - transform.position.y;
            if (height > offset) {
                other_sr.sortingOrder = sr.sortingOrder + 1;
                print($"setting sprite renderer to layer: {sr.sortingOrder + 1}");
            } else {
                other_sr.sortingOrder = sr.sortingOrder - 1;
                print($"setting sprite renderer to layer: {sr.sortingOrder - 1}");
            }
        }
    }
}
