using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Slider stam_slider, hp_slider;
    public static HUDManager HUDMANAGER;
    void Start() {
        if (HUDMANAGER == null) HUDMANAGER = this;
    }

    public void UpdateStam(float curr, float max) {
        stam_slider.maxValue = max;
        stam_slider.value = curr;
    }

    public void UpdateHP(float curr, float max) {
        hp_slider.maxValue = max;
        hp_slider.value = curr;
    }
}
