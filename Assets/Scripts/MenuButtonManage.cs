using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonManage : MonoBehaviour
{
    public Button[] menuButtons; // Mảng chứa các button
    public Image[] highlightedImages; // Các hình ảnh Highlighted tương ứng
    private int currentIndex = 0; // Chỉ số button hiện đang được chọn

    public AudioSource audioSource; // AudioSource để phát âm thanh
    public AudioClip navigateSound; // Âm thanh khi điều hướng
    public AudioClip selectSound; // Âm thanh khi chọn nút

    // Start is called before the first frame update
    void Start()
    {
        SelectButton(currentIndex);
    }

    // Update is called once per frame
    void Update()
    {
        // Điều hướng thông qua các button bằng phím mũi tên
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = menuButtons.Length - 1;
            PlayNavigateSound();
            SelectButton(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex++;
            if (currentIndex >= menuButtons.Length) currentIndex = 0;
            PlayNavigateSound();
            SelectButton(currentIndex);
        }

        // Nhấn phím "Enter" để kích hoạt button
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            PlaySelectSound();
            menuButtons[currentIndex].onClick.Invoke();
        }
    }

    void SelectButton(int index)
    {
        // Đặt tất cả hình ảnh về trạng thái bình thường
        for (int i = 0; i < menuButtons.Length; i++)
        {
            menuButtons[i].GetComponent<Image>().enabled = true; // Bật lại ảnh gốc
            highlightedImages[i].enabled = false; // Tắt ảnh Highlighted
        }

        // Bật hình ảnh trạng thái "Highlighted" của button hiện tại
        menuButtons[index].GetComponent<Image>().enabled = false; // Tắt ảnh gốc
        highlightedImages[index].enabled = true; // Bật ảnh Highlighted
    }

    // Phát âm thanh khi điều hướng
    void PlayNavigateSound()
    {
        if (audioSource != null && navigateSound != null)
        {
            audioSource.PlayOneShot(navigateSound);
        }
    }

    // Phát âm thanh khi chọn
    void PlaySelectSound()
    {
        if (audioSource != null && selectSound != null)
        {
            audioSource.PlayOneShot(selectSound);
        }
    }
}
