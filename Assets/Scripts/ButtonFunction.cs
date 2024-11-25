using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunction : MonoBehaviour
{
    public Button continueButton;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("HasSavedGame", 0) == 1)
        {
            continueButton.gameObject.SetActive(true); // Bật nút Continue nếu có trò chơi đã lưu
        }
        else
        {
            continueButton.gameObject.SetActive(false); // Tắt nút Continue nếu không có trò chơi đã lưu
        }
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        PlayerPrefs.DeleteKey("HasSavedGame");
        SceneManager.LoadScene("SampleScene");
    }
    public void Continue()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
}
