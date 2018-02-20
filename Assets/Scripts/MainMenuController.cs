using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public void OpenLevel(int i) {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

	public void QuitGame() {
        Application.Quit();
    }
}
