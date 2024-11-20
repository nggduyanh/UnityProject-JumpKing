using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPause;
    public PlayerMovement playerMovement;
    public ScoreManager scoreManager;
    public float defaultMusicVolume = 1f;
    private float musicVolume;
    private Vector3 startingPlayerPosition;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        startingPlayerPosition = playerMovement.transform.position;

        musicVolume = PlayerPrefs.GetFloat("musicVolume", defaultMusicVolume);
        AudioListener.volume = musicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
   public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
        Debug.Log("Paused: " + isPause);
        Debug.Log("Current Volume: " + AudioListener.volume);

        PlayerPrefs.SetFloat("musicVolume", AudioListener.volume);
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
        Debug.Log("Paused: " + isPause);
        Debug.Log("Current Volume: " + AudioListener.volume);


        musicVolume = PlayerPrefs.GetFloat("musicVolume", defaultMusicVolume);
        AudioListener.volume = musicVolume;
    }
    
    public void NewGame()
    {
        playerMovement.transform.position = new Vector3(0.32f, -6.870397f, 0);
        Camera.main.transform.position = new Vector3(0.06122589f, -3.282597f, -20f);
        scoreManager.setJumpCount(0);
        scoreManager.setFallCount(0);
        scoreManager.setHoursCount(0);
        scoreManager.setMinusCount(0);
        scoreManager.setSecondCount(0);
        pauseMenu.SetActive(false);
        isPause = false;
        Time.timeScale = 1f;
        scoreManager.jumpCountText.text = "Jump: " + scoreManager.getJumpCount().ToString();
        scoreManager.fallCountText.text = "Fall: " + scoreManager.getFallCount().ToString();
        scoreManager.ResetTime();
    }
    public void QuiteAnSave()
    {
        Debug.Log("Ground" + playerMovement.getIsGround());
        Vector3 playerPosition = playerMovement.transform.position;
        Debug.Log("Player position" + playerPosition);
        PlayerPrefs.SetFloat("PlayerPosX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPosY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPosZ", playerPosition.z);

        PlayerPrefs.SetInt("FallScore", scoreManager.getFallCount());
        PlayerPrefs.SetInt("JumpScore", scoreManager.getJumpCount());
        PlayerPrefs.SetInt("Hour", scoreManager.getHoursCount());
        PlayerPrefs.SetInt("Minus", scoreManager.getMinusCount());
        PlayerPrefs.SetInt("Second", scoreManager.getSecondCount());

        PlayerPrefs.SetInt("HasSavedGame", 1);

        PlayerPrefs.Save();

        SceneManager.LoadScene("MenuScene");
        Debug.Log("Player position" + playerPosition);
    }
}
