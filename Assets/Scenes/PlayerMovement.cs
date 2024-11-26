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

    private bool isSquatting, isGrounded,isProned,isFalling,isFlying;   
    private float jumpForce, moveInput;
    private int currentLevel;
    public float walkSpeed, minJumpForce, maxJumpForce, increasingForceSpeed;

    void Start()
    {
        Application.targetFrameRate = 60;
        Debug.Log("hello");
        rb =GetComponent<Rigidbody2D>();
        Player = GetComponent<Animator>();
        audioSource=GetComponent<AudioSource>();
        scoreManager= GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        if (PlayerPrefs.GetInt("HasSavedGame") == 1)
        {
            transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"), PlayerPrefs.GetFloat("PlayerPosY"), PlayerPrefs.GetFloat("PlayerPosX"));
            scoreManager.setFallCount(PlayerPrefs.GetInt("FallScore"));
            scoreManager.setJumpCount(PlayerPrefs.GetInt("JumpScore"));
            scoreManager.setTimeCount(PlayerPrefs.GetInt("Time"));

            //Player.SetBool("isIdle", PlayerPrefs.GetInt("isIdleAnim") ==1 ? true:false);
            //Player.SetBool("isRunning", PlayerPrefs.GetInt("isRunningAnim") == 1 ? true : false);
            //Player.SetBool("isSquatting", PlayerPrefs.GetInt("isSquattingAnim") == 1 ? true : false);
            //Player.SetBool("isJumping", PlayerPrefs.GetInt("isJumpingAnim") == 1 ? true : false);
            //Player.SetBool("isFalling", PlayerPrefs.GetInt("isFallingAnim") == 1 ? true : false);
            //Player.SetBool("isProned", PlayerPrefs.GetInt("isPronedAnim") == 1 ? true : false);
            //Player.SetBool("isFlying", PlayerPrefs.GetInt("isFlyingAnim") == 1 ? true : false);

            //isSquatting = PlayerPrefs.GetInt("isSquatting") == 1 ? true : false;
            //isGrounded = PlayerPrefs.GetInt("isGrounded") == 1 ? true : false;
            //isProned = PlayerPrefs.GetInt("isProned") == 1 ? true : false;
            //isFalling = PlayerPrefs.GetInt("isFalling") == 1 ? true : false;
            //isFlying = PlayerPrefs.GetInt("isFlying") == 1 ? true : false;
            //rb.velocity =new Vector2 (PlayerPrefs.GetFloat("velocityX"), PlayerPrefs.GetFloat("velocityY"));
            scoreManager.ReDrawText();
        }
        else
        {
            Player.SetBool("isIdle", true);
            Player.SetBool("isRunning", false);
            Player.SetBool("isSquatting", false);
            Player.SetBool("isJumping", false);
            Player.SetBool("isFalling", false);
            Player.SetBool("isProned", false);
            Player.SetBool("isFlying", false);
            isSquatting = false;
            isGrounded = false;
            isProned = false;
            isFalling = false;
            isFlying = false;
        }
        currentLevel = 1;

        InvokeRepeating("PlayerTime", 0, 1);


    }
    public void setIsSquatting(bool isSquatting)
    {
        this.isSquatting = isSquatting;
    }
    public void setIsGrounded(bool isGrounded)
    {
        this.isGrounded = isGrounded;
    }
    public void setIsProned(bool isProned)
    {
        this.isProned = isProned;
    }
    public void setIsFalling(bool isFalling)
    {
        this.isFalling = isFalling;
    }
    public void setIsFlying(bool isFlying)
    {
        this.isFlying = isFlying;
    }
    public void setVelocity(Vector2 velocity)
    {
        this.rb.velocity = velocity;
    }

    public Vector2 getVelocity()
    {
        return rb.velocity;
    }
    public bool getIsSquatting()
    {
        return isSquatting;
    }
    public bool getIsGrounded()
    {
        return isGrounded;
    }
    public bool getIsProned()
    {
        return isProned;
    }
    public bool getIsFalling()
    {
        return isFalling;
    }
    public bool getIsFlying()
    {
        return isFlying;
    }
    //Luong
    private void OnTriggerEnter2D(Collider2D collision)
    {
        for(int i = 1; i < 24; i++)
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
        if (!isGrounded)
        {
            PlaySound(0);
            //Debug.Log("Sound0");
        }
        if(collision.collider.tag == "FlyingBuff")
        {
            isFlying = true;
            rb.sharedMaterial = normalMaterial;
            Player.SetBool("isFlying", true);
            Invoke("StopFlying", 5);
            //Destroy(collision.gameObject);
            Debug.Log("im flying");
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
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.7f, 0.3f));
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

    private void StopFlying()
    {
        isFlying = false;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        Player.SetBool("isFlying", false);
    }
    void Update()
    {
        if (!PauseMenu.isPause)
        {
            if(Input.GetKey(KeyCode.F))
            {
                transform.position = new Vector2(-4.176444f, 166.1912f);
            }
            if (Input.GetKey(KeyCode.G))
            {
                transform.position = new Vector2(-4.176444f, -63.44709f);
            }

            if (isFlying)
            {
                rb.velocity = new Vector2(rb.velocity.x, 3);
            }
            //if(rb.velocity.x!=0)    Debug.Log("velocity x:" + rb.velocity.x);
            //if (rb.velocity.y != 0) Debug.Log("velocity y:" + rb.velocity.y);
            moveInput = Input.GetAxisRaw("Horizontal");
            //isGrounded = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(1.5f, 0.3f), 0f, groundMask);
            isGrounded = Physics2D.BoxCast(transform.position, new Vector2(0.7f, 0.3f), 0, -transform.up, 0.5f, groundMask);
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
            if ((isGrounded && !isSquatting)|| isFlying)
            {
                //gameObject.transform.Translate(Vector2.left * speedX * Time.deltaTime);
                rb.velocity = new Vector2(walkSpeed * moveInput, rb.velocity.y);
                if (moveInput != 0)
                {
                    transform.localScale = new Vector2(moveInput, transform.localScale.y);
                    Player.SetBool("isRunning", true);
                    Player.SetBool("isIdle", false);
                    Player.SetBool("isProned", false);
                    isSquatting = false;
                }
            }
            //Player squat
            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                jumpForce += increasingForceSpeed;
                rb.velocity = new Vector2(0, rb.velocity.y);
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
                if (!isFlying)
                {
                    rb.sharedMaterial = bouncyMaterial;
                    //Debug.Log("isJumping");

                    Player.SetBool("isJumping", true);
                    Player.SetBool("isIdle", false);
                    Player.SetBool("isRunning", false);
                }
                else 
                {
                    Player.SetBool("isFlying", true);
                }
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
}
