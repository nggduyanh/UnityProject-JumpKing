using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue()
    {


        // Đặt lại trạng thái trò chơi từ PlayerPrefs
        float playerPosX = PlayerPrefs.GetFloat("PlayerPosX", 0f);
        float playerPosY = PlayerPrefs.GetFloat("PlayerPosY", 0f);
        float playerPosZ = PlayerPrefs.GetFloat("PlayerPosZ", 0f);

        Vector3 playerPosition = new Vector3((float)playerPosX, (float)playerPosY, (float)playerPosZ);
        Debug.Log("Player position" + playerPosition);

        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;

        // Cần sử dụng coroutine hoặc phương thức khác để đảm bảo dữ liệu được khôi phục sau khi tải scene
        Debug.Log("Starting Coroutine to restore game state...");
        StartCoroutine(RestoreGameState(playerPosX, playerPosY, playerPosZ));


    }
    private IEnumerator RestoreGameState(float x, float y, float z)
    {
        Debug.Log("RestoreGameState Coroutine started...");

        // Chờ cho đến khi cảnh được tải xong
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "SampleScene");

        // Khôi phục vị trí của người chơi
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(x, y, z);
            Debug.Log("Player position restored to: " + player.transform.position);
        }
        else
        {
            Debug.LogError("Player object could not be found!");
            yield break;
        }

        // Khôi phục điểm số
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            if (PlayerPrefs.HasKey("FallScore")) scoreManager.setFallCount(PlayerPrefs.GetInt("FallScore"));
            if (PlayerPrefs.HasKey("JumpScore")) scoreManager.setJumpCount(PlayerPrefs.GetInt("JumpScore"));
            if (PlayerPrefs.HasKey("Hour")) scoreManager.setHoursCount(PlayerPrefs.GetInt("Hour"));
            if (PlayerPrefs.HasKey("Minus")) scoreManager.setMinusCount(PlayerPrefs.GetInt("Minus"));

            Debug.Log("Scores restored: JumpCount=" + scoreManager.getJumpCount());
        }
        else
        {
            Debug.LogError("ScoreManager component not found!");
            yield break;
        }

        Debug.Log("Game state restoration complete.");
    }
}
