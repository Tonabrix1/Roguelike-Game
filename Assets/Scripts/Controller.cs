using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    //mainstat upgrades based on character, hardcoded to be stamina regen for prototyping
    Rigidbody2D body;
    public static Controller player = null;
    bool charging = false, damaging = false, invulnerable = false;
    float vert = 0f, hori = 0f;
    public float speed = 250f, dampening = 1.1f, damage = 10f, knockback = 1f, curr_hp = 10f, 
                 max_hp = 10f, curr_stam = 10f, max_stam = 10f, base_stam_regen = 1f, curr_stam_regen = 1f, dash_cost = 2f,
                 exp = 0f;
    public int level;
    Vector2 dashDir;
    SpriteRenderer spriterend;
    Color col;

    public static Controller GetPlayer() {
        return player;
    }

    void Start()
    {
        player = this;
        body = gameObject.GetComponent<Rigidbody2D>();
        spriterend = gameObject.GetComponent<SpriteRenderer>();
        col = spriterend.color;
        HUDManager.HUDMANAGER.UpdateStam(curr_stam, max_stam);
        HUDManager.HUDMANAGER.UpdateHP(curr_hp, max_hp);
    }

    void Update() {
        vert = Input.GetAxisRaw("Vertical");
        hori = Input.GetAxisRaw("Horizontal");
        if (!charging) {
            if (Input.GetMouseButtonDown(0) && curr_stam > dash_cost) StartCoroutine(ChargeDash(vert, hori));
            else if (curr_stam < max_stam) {
                curr_stam = Mathf.Min(max_stam, curr_stam + curr_stam_regen * Time.deltaTime);
                HUDManager.HUDMANAGER.UpdateStam(curr_stam, max_stam);
            }
        }
    }

    void FixedUpdate()
    {
        body.velocity /= dampening;
        if (!charging) body.velocity += new Vector2(hori, vert) * Time.fixedDeltaTime * speed;
    }

    public IEnumerator ChargeDash(float vert, float hori) {
        charging = true;
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        body.velocity = Vector2.zero;
        yield return new WaitForSecondsRealtime(.5f);
        //check for race condition incase the player was damaged 
        if (charging) {
            damaging = true;
            curr_stam -= dash_cost;
            HUDManager.HUDMANAGER.UpdateStam(curr_stam, max_stam);
            dashDir = (mouse - transform.position).normalized * speed * 2;
            body.velocity = dashDir;
            yield return new WaitForSecondsRealtime(.35f);
        }
        charging = damaging = false;
    }

    public IEnumerator CancelDash(Vector3 pos) {
        body.velocity = (transform.position - pos) * speed;
        yield return new WaitForSecondsRealtime(.2f);
    }
    void OnTriggerEnter2D(Collider2D other) {
        Enemy e = other.gameObject.GetComponent<Enemy>();
        if (e != null) {
            if (damaging) {
                StartCoroutine(e.Damage(damage, dashDir, knockback, speed));
                StartCoroutine(CancelDash(other.transform.position));
            } else if (!invulnerable && !e.is_dead) {
                charging = false;
                StartCoroutine(Damage(e.damage, other.transform.position, e.knockback)); 
            }
        }
    }

    public IEnumerator Damage(float amount, Vector3 pos, float knockback) {
        invulnerable = true;
        curr_hp -= amount;
        HUDManager.HUDMANAGER.UpdateHP(curr_hp, max_hp);
        if (curr_hp <= 0) {
            Die();
            yield return true;
        }
        body.velocity = Vector2.zero;
        spriterend.color = Color.red;
        yield return new WaitForSecondsRealtime(.1f);
        body.velocity = (transform.position - pos).normalized * knockback;
        yield return new WaitForSecondsRealtime(.25f);
        spriterend.color = col;
        yield return new WaitForSecondsRealtime(.5f);
        invulnerable = false;
    }

    public void Die() {
        print("YOU DIED");
    }

    public float CalculateRequiredEXP(int level) {
       return Mathf.Pow(3*level, 1.5f);
    }

    public void CheckLevel() {
        float req_exp = CalculateRequiredEXP(level);
        while (exp >= req_exp) {
            exp -= req_exp;
            level++;
            print("YOU LEVELED UP!");
            req_exp = CalculateRequiredEXP(level);
        }
    }

    public void AwardExp(float amount) {
        exp += amount;
        print($"You got {amount} exp!!");
        CheckLevel();
    }

    public float RollStatIncrease(float min_roll, float max_roll) {
        return Random.Range(min_roll, max_roll);
    } 
}
