//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine.UIElements;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;

    public Camera MainCamera;
    public Animator Player;
    public PhysicsMaterial2D bouncyMaterial,normalMaterial;
    public LayerMask groundMask;

    private bool isSquatting, isGrounded;   
    private float jumpForce, moveInput;

    public float walkSpeed, minJumpForce, maxJumpForce, increasingForceSpeed;

    void Start()
    {

        rb =GetComponent<Rigidbody2D>();
        Player = GetComponent<Animator>();

        Player.SetBool("isIdle", true);
        Player.SetBool("isRunning", false);
        Player.SetBool("isSquatting", false);
        Player.SetBool("isJumping", false);
        Player.SetBool("isFalling", false);
        Player.SetBool("isPronning", false);
        isSquatting = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        for(int i = 1; i < 10; i++)
        {
            string levelTag = "Level" + i;
            if(collision.tag == levelTag)
            {
                Debug.Log("test1 Moving to Level : " + i);
                moveCameraToLevel(levelTag);
                Debug.Log("test 2Moving to Level: " + i);
            }   
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
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y-0.5f), new Vector2(0.75f, 0.3f), 0f, groundMask);
        
        //if(isGrounded) Debug.Log("isGrounded");
        //Debug.Log("velocity: " + rb.velocity.y);

        if (isGrounded)
        {
            rb.sharedMaterial = normalMaterial;
            //Debug.Log("normal");
            if (rb.velocity.x == 0)
            {
                Player.SetBool("isIdle", true);
                Player.SetBool("isFalling", false);
                Player.SetBool("isRunning", false);
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
            isSquatting = false;
        }
        //Player squat
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            jumpForce += increasingForceSpeed;
            //Debug.Log("isSquatting");
            Debug.Log("squatforce" + jumpForce);

            Player.SetBool("isSquatting", true);
            Player.SetBool("isRunning", false);
            Player.SetBool("isFalling", false);
            Player.SetBool("isIdle", false);
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
            //Debug.Log("isFalling");
        }
    }
}
