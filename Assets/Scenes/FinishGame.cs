using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    public GameObject finishGame;
    public PlayerMovement playerMovement;
    public ScoreManager scoreManager;
    public static bool isNewGame;
    // Start is called before the first frame update
    void Start()
    {
        finishGame.SetActive(false);
        isNewGame = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game quitted");
    }
    public void NewGame()
    {
        playerMovement.transform.position = new Vector3(-3.31f, -5.05f, 0f);
        Camera.main.transform.position = new Vector3(-0.02877408f, -1.722597f, -20f);
        Time.timeScale = 1f;
        scoreManager.setJumpCount(0);
        scoreManager.setFallCount(-1);
        scoreManager.setHoursCount(0);
        scoreManager.setMinusCount(0);
        scoreManager.setSecondCount(0);
        scoreManager.jumpCountText.text = "Jump: " + scoreManager.getJumpCount().ToString();
        scoreManager.fallCountText.text = "Fall: " + scoreManager.getFallCount().ToString();
        scoreManager.ResetTime();
        
        finishGame.SetActive(false);

    }
    public void ShowFinish()
    {
        finishGame.SetActive(true);
    }
}
