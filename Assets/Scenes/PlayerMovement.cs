using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private ScoreManager scoreManager;
    private AudioSource audioSource;

    public Camera MainCamera;
    public Animator Player;
    public PhysicsMaterial2D bouncyMaterial,normalMaterial;
    public AudioClip[] audioClips;
    public LayerMask groundMask;

    private bool isSquatting, isGrounded,isProned,isFalling;   
    private float jumpForce, moveInput;
    private int currentLevel;
    public float walkSpeed, minJumpForce, maxJumpForce, increasingForceSpeed;

    void Start()
    {

        rb =GetComponent<Rigidbody2D>();
        Player = GetComponent<Animator>();
        audioSource=GetComponent<AudioSource>();
        scoreManager= GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        currentLevel = 1;

        InvokeRepeating("PlayerTime", 0, 1);

        Player.SetBool("isIdle", true);
        Player.SetBool("isRunning", false);
        Player.SetBool("isSquatting", false);
        Player.SetBool("isJumping", false);
        Player.SetBool("isFalling", false);
        Player.SetBool("isProned", false);
        isSquatting = false;
        isProned = false;
        isFalling = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        for(int i = 1; i < 10; i++)
        {
            string levelTag = "Level" + i;
            if (collision.tag == levelTag)
            {
                if (i < currentLevel)
                {
                    scoreManager.OnFall();
                    isProned = true;
                }
                currentLevel = i;
                Debug.Log("test1 Moving to Level : " + i);
                moveCameraToLevel(levelTag);
                //Debug.Log("test 2Moving to Level: " + i);
            }   
        }   
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isGrounded)
        {
            PlaySound(0);
            //Debug.Log("Sound0");
        }
    }

    private void moveCameraToLevel(string levelTag)
    {
            string levelString = levelTag;
            string levelNumberString = levelString.Substring(5);

            GameObject level = GameObject.FindWithTag("Map" + levelNumberString);
            if (level != null)
            {
                Vector3 newCameraPosition = level.transform.position;
                MainCamera.transform.position = newCameraPosition + new Vector3(0, 0, -20);
            }
    }
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.75f, 0.3f) );
    }
    private void PlaySound(int clipNumber)
    {
        audioSource.clip = audioClips[clipNumber];
        audioSource.Play();
    }
    private void PlayerTime()
    {
        scoreManager.PlayerTime();
    }
    void Update()
    {
        //if(rb.velocity.x!=0)    Debug.Log("velocity x:" + rb.velocity.x);
        //if (rb.velocity.y != 0) Debug.Log("velocity y:" + rb.velocity.y);
        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y-0.5f), new Vector2(0.75f, 0.3f), 0f, groundMask);
        //if(isGrounded) Debug.Log("isGrounded")
        if (isGrounded) 
        {
            rb.sharedMaterial = normalMaterial;
            //Debug.Log("normal");  
            if (rb.velocity.x == 0)
            {
                if (isProned)
                {
                    Debug.Log("isProned");
                    Player.SetBool("isProned", true);
                    isProned = false;
                }
                else if (isFalling)
                {
                    //Debug.Log("falled");
                    PlaySound(3);
                }
                Player.SetBool("isIdle", true);
                Player.SetBool("isFalling", false);
                Player.SetBool("isRunning", false);
                isFalling = false;
            }

        }
        //Player walk
        if (moveInput!=0 && isGrounded && !isSquatting)
        {
            transform.localScale = new Vector2(moveInput, transform.localScale.y);
            //gameObject.transform.Translate(Vector2.left * speedX * Time.deltaTime);
            rb.velocity = new Vector2(walkSpeed * moveInput, rb.velocity.y);

            Player.SetBool("isRunning", true);
            Player.SetBool("isIdle", false);
            Player.SetBool("isProned", false);
            isSquatting = false;
        }
        //Player squat
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            jumpForce += increasingForceSpeed;
            //Debug.Log("isSquatting");
            Debug.Log("jumpForce" + jumpForce);

            Player.SetBool("isSquatting", true);
            Player.SetBool("isRunning", false);
            Player.SetBool("isFalling", false); 
            Player.SetBool("isIdle", false);
            Player.SetBool("isProned", false);
            isSquatting = true;
        }
        //Player Jump
        if (Input.GetKeyUp(KeyCode.Space) && isSquatting)
        {
            if (jumpForce < minJumpForce) jumpForce = minJumpForce;
            if (jumpForce > maxJumpForce) jumpForce = maxJumpForce;
            rb.velocity = new Vector2(moveInput * walkSpeed, jumpForce);
            jumpForce = 0;
            //Debug.Log("Bouncyyy");

            Player.SetBool("isRunning", false);
            Player.SetBool("isSquatting", false);
            isSquatting = false;
            scoreManager.OnJump();
        }
        //Player jumping
        if (rb.velocity.y > 0 && !isGrounded)
        {
            rb.sharedMaterial = bouncyMaterial;
            //Debug.Log("isJumping");

            Player.SetBool("isJumping", true);
            Player.SetBool("isIdle", false);
            Player.SetBool("isRunning", false);
        }
        //Player falling
        if (rb.velocity.y < 0 && !isGrounded)
        {
            Player.SetBool("isFalling", true);
            Player.SetBool("isIdle", false);
            Player.SetBool("isRunning", false);
            Player.SetBool("isJumping", false);
            isFalling = true;   
            //Debug.Log("isFalling");
        }
    }
}
