using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public GameObject GameOverText;
    public Text LivesText;

    private PlayerLifePoints playerLifePoints;

	void Start () {
        GameOverText.SetActive(false);
        playerLifePoints = player.GetComponent<PlayerLifePoints>();
	}
	
	void Update () {
        if(playerLifePoints.lifePoint <= 0) {
            if(Input.GetKeyUp("r")) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            if(Input.GetKeyUp("q")) {
                SceneManager.LoadScene(0);
            }
        }
    }
    
    public void GameOver() {
        player.SetActive(false);
        GameOverText.SetActive(true);
    }

    public void UpdateLivesText() {
        LivesText.text = "Lives : " + playerLifePoints.lifePoint;
    }

}
