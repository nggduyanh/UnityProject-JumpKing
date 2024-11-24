using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicManagement : MonoBehaviour
{
    public AudioClip openingTheme;
    public AudioClip menuIntro;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            // Play the opening theme first
            audioSource.clip = openingTheme;
            audioSource.Play();
            // Start a coroutine to check when the opening theme finishes
            StartCoroutine(PlayMenuIntroAfterOpening());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private System.Collections.IEnumerator PlayMenuIntroAfterOpening()
    {
        // Wait for the opening theme to finish
        yield return new WaitForSeconds(openingTheme.length);

        // Play the menu intro and set it to loop
        audioSource.clip = menuIntro;
        audioSource.loop = true;
        audioSource.Play();
    }
}
