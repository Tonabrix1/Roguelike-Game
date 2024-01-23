using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public float aggro_range = 15f, direct_aggro_range = 3f, speed = 125f, target_offset = .5f, damage = .5f, knockback = 1f;
    // Update is called once per frame
    float timer = 0f, random_x = 0f, random_y = 0f, random_speed_mod = 1f;
    void FixedUpdate()
    {
        if (!is_dead && Controller.GetPlayer()) FindTarget(Controller.GetPlayer().gameObject.transform.position);
    }

    void FindTarget(Vector3 target) {
        float distance = Vector3.Distance(target, transform.position);
        if (distance <= aggro_range) {
            if (distance > direct_aggro_range) MovementIndirect(target);
            else MovementDirect(target);
        }
    }

    public virtual void MovementDirect(Vector3 target) {
        body.velocity = (target - transform.position).normalized * speed * Mathf.Min(random_speed_mod, 1.3f) * Time.fixedDeltaTime;
    }

    public virtual void MovementIndirect(Vector3 target) {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0) {
            random_x = Random.Range(-target_offset, target_offset);
            random_y = Random.Range(-target_offset, target_offset);
            random_speed_mod = Random.Range(1f, 3f);
            timer = 2.5f;
        }
        
        target = new Vector3(target.x + random_x, 0f, target.y + random_y);
        body.velocity = (target - transform.position).normalized * speed * random_speed_mod * Time.fixedDeltaTime;
    }
}
