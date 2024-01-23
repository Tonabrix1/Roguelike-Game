using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public float timer = 10f, cooldown = 10f, min_spawn_radius = 10f, max_spawn_radius = 15f;
    public int wave_count = 0;
    bool spawning = false, game_over = false;
    public List<Wave> waves = new List<Wave>();

    void Update()
    {
        if (timer <= 0f && !spawning && !game_over) StartCoroutine(SpawnWave(cooldown));
        else timer -= Time.deltaTime;
    }
    
    IEnumerator SpawnWave(float wave_time) {
        spawning = true; //spawning : flag to avoid race conditions
        if (wave_count >= waves.Count) {
            game_over = true;
            print("GAME OVER!!");
            yield break;
        }
        Wave curr_wave = waves[wave_count]; 
        List<int> curr_num_waves = curr_wave.num_waves, curr_num_spawns = curr_wave.num_spawns;

        int num_tides = 0, tides_left = 0, mob_wave = 0, num_categories = curr_wave.num_waves.Count;
        //num_tides : number of total times monsters will be spawned across all waves
        //tides_left : number of tides left in this set of tides (each mob type has its own set of tides)
        //mob_wave : index of the mobs that are being spawned in the current set of tides 
        //num_categories : the number of mob types that will be spawned, (EX: a slime and a goat both spawn 3 enemies in 5 tides per wave, num_categories is 2 len([slime, goat]))

        for (int i = 0; i < num_categories; i++) num_tides += curr_num_waves[i]; // num_tides used to calculate timer offset

        float timer_offset = wave_time / num_tides;

        for (int i = 0; i < num_tides; i++,tides_left--) {
            if (mob_wave < num_categories - 1 && tides_left <= 0) {
                tides_left = curr_num_waves[mob_wave];
                mob_wave++;
            }
            for (int mob = 0; mob < curr_num_spawns[mob_wave]; mob++) {
                Vector2 spawn = SpawnRange(Vector2.zero, min_spawn_radius, max_spawn_radius);
                Instantiate(curr_wave.mobs[mob_wave], spawn, Quaternion.identity);
                yield return new WaitForSecondsRealtime(timer_offset);    
            }
        }
        
        spawning = false;
        timer = cooldown;
        wave_count++;
    }

    Vector2 SpawnRange(Vector2 center, float min, float max) {
        float offset_x = Random.Range(-max, max), offset_y = Random.Range(-max, max);
        offset_x = offset_x >= 0 ? offset_x + min : offset_x - min; // if a positive position is generated add min, if a negative one is genered subtract gen to push the spawn away from the player.
        offset_y = offset_y >= 0 ? offset_y + min : offset_y - min;
        return new Vector2(offset_x, offset_y);
    } 
}
