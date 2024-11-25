using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        if (playerMovement.getIsGrounded())
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
        playerMovement.transform.position = new Vector3(-4.176444f, -63.44709f, 0);
        //Camera.main.transform.position = new Vector3(-0.12f, -59.6f, -20f);
        scoreManager.setJumpCount(0);
        scoreManager.setFallCount(0);
        scoreManager.setTimeCount(0);
        pauseMenu.SetActive(false);
        isPause = false;
        Time.timeScale = 1f;
        scoreManager.jumpCountText.text = "Jump: " + scoreManager.getJumpCount().ToString();
        scoreManager.fallCountText.text = "Fall: " + scoreManager.getFallCount().ToString();
        scoreManager.ResetTime();
    }
    public void QuiteAnSave()
    {
        pauseMenu.SetActive(false);
        isPause = false;
   
        Vector3 playerPosition = playerMovement.transform.position;
        Debug.Log("Player position" + playerPosition);
        PlayerPrefs.SetFloat("PlayerPosX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPosY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPosZ", playerPosition.z);

        PlayerPrefs.SetInt("FallScore", scoreManager.getFallCount());
        PlayerPrefs.SetInt("JumpScore", scoreManager.getJumpCount());
        PlayerPrefs.SetInt("Time", scoreManager.getTimeCount());

        //PlayerPrefs.SetInt("isIdleAnim", playerMovement.GetComponent<Animator>().GetBool("isIdle") ? 1 : 0);
        //PlayerPrefs.SetInt("isRunningAnim", playerMovement.GetComponent<Animator>().GetBool("isRunning") ? 1 : 0);
        //PlayerPrefs.SetInt("isSquattingAnim", playerMovement.GetComponent<Animator>().GetBool("isSquatting") ? 1 : 0);
        //PlayerPrefs.SetInt("isJumpingAnim", playerMovement.GetComponent<Animator>().GetBool("isJumping") ? 1 : 0);
        //PlayerPrefs.SetInt("isFallingAnim", playerMovement.GetComponent<Animator>().GetBool("isFalling") ? 1 : 0);
        //PlayerPrefs.SetInt("isPronedAnim", playerMovement.GetComponent<Animator>().GetBool("isProned") ? 1 : 0);
        //PlayerPrefs.SetInt("isFlyingAnim", playerMovement.GetComponent<Animator>().GetBool("isFlying") ? 1 : 0);

        //PlayerPrefs.SetInt("isSquatting", playerMovement.getIsSquatting() ? 1 : 0);
        //PlayerPrefs.SetInt("isGrounded", playerMovement.getIsGrounded() ? 1 : 0);
        //PlayerPrefs.SetInt("isFalling", playerMovement.getIsFalling() ? 1 : 0);
        //PlayerPrefs.SetInt("isProned", playerMovement.getIsProned() ? 1 : 0);
        //PlayerPrefs.SetInt("isFlying", playerMovement.getIsFlying() ? 1 : 0);

        //PlayerPrefs.SetFloat("velocityX", playerMovement.getVelocity().x);
        //PlayerPrefs.SetFloat("velocityY", playerMovement.getVelocity().y);
        PlayerPrefs.SetInt("HasSavedGame", 1);

        PlayerPrefs.Save();

        SceneManager.LoadScene("MenuScene");
        Debug.Log("Player position" + playerPosition);
    }
}
