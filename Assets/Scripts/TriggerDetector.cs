using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour {

    private Color32 PLAYERMATCOLOR = new Color32(60, 191, 132, 255);

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            other.GetComponent<Renderer>().material.color = new Color32(180, 15, 15, 120);
            other.GetComponent<PlayerLifePoints>().GetHit();
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.tag == "Player") {
            other.GetComponent<Renderer>().material.color = PLAYERMATCOLOR;
        }
    }

}
