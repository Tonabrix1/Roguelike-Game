using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    public float curr_hp = 10f, max_hp = 10f, exp_reward = 1f;
    protected Rigidbody2D body;
    public bool is_dead = false;
    
    SpriteRenderer spriterend;
    Color col;
    
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        spriterend = gameObject.GetComponent<SpriteRenderer>();
        col = spriterend.color;
    }

    public IEnumerator Damage(float amount, Vector3 pos, float knockback, float speed) {
        if (!is_dead) {
            curr_hp -= amount;
            if (curr_hp <= 0) {
                Die();
                yield return true;
            }
            body.velocity = Vector2.zero;
            spriterend.color = Color.red;
            yield return new WaitForSecondsRealtime(.1f);
            body.velocity = pos * knockback / speed;
            yield return new WaitForSecondsRealtime(.25f);
            spriterend.color = col;
        }
    }

    public void Die() {
        is_dead = true;
        StartCoroutine(Utils.LerpColor(spriterend, spriterend.color, Color.black, 5f, 100));
        AwardExp();
        Destroy(gameObject, 5);
    }
    
    public void AwardExp() {
        Controller.player.AwardExp(exp_reward);
    }
}
