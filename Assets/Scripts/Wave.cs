using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Wave : ScriptableObject
{
    public List<GameObject> mobs;
    //mobs : A list of enemy prefabs that will be spawned (in order) during a wave
    public List<int> num_waves, num_spawns;
    //num_waves : A list of integers that details the number of times a group of enemies will be spawned before cycling to the next enemy type.
    //num_spawns : A list of integers that detaeils the number of enemies to spawn each time a mob group is spawned. 
    //The length of both must match the length of mobs
}