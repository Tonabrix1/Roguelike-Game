using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utils : MonoBehaviour
{
    public static IEnumerator LerpColor(SpriteRenderer sr, Color start, Color finish, float delay, float chunks) {
        Color new_col = new Color(start.r, start.g, start.b, start.a);
        float red_increment = (finish.r - start.r) / chunks, green_increment = (finish.g - start.g) / chunks, 
             blue_increment = (finish.b - start.b) / chunks, alpha_increment = (finish.a - start.a) / chunks;
        for (int i = 0; i < chunks; i++) {
            new_col = new Color(new_col.r + red_increment, new_col.g + green_increment, new_col.b + blue_increment, new_col.a + alpha_increment);
            sr.color = new_col;
            yield return new WaitForSecondsRealtime(delay / chunks);
        }
    }
}
