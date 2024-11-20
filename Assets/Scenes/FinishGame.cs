using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    public GameObject finishGame;
    public PlayerMovement playerMovement;
    public ScoreManager scoreManager;
    public static bool isNewGame;
    private Vector3 startingPlayerPosition;
    // Start is called before the first frame update
    void Start()
    {
        finishGame.SetActive(false);
        startingPlayerPosition = playerMovement.transform.position;
        isNewGame = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game quitted");
    }
    public void NewGame()
    {
        playerMovement.transform.position = new Vector3(0.32f, -6.870397f, 0);
        Camera.main.transform.position = new Vector3(0.06122589f, -3.282597f, -20f);
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
