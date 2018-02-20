using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifePoints : MonoBehaviour {

    public int lifePoint = 3;

    private GameObject gameManager;

    public void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    public void GetHit() {
        lifePoint--;
        gameManager.GetComponent<GameManager>().UpdateLivesText();
        if(lifePoint == 0) {
            gameManager.GetComponent<GameManager>().GameOver();
        }
    }

}
