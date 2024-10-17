using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] float steerSpped = 0.5f;
    //[SerializeField] float moveSpeed = 0.5f;
    public Camera MainCamera;
    public Animator Player;
    Vector2 move;

    public float speedX,speedY;
    public float directionSpeed;
    public float minSquatForce;
    public float maxSquatForce;

    private bool isSquatting = false, isFalling = false, isJumping = false,isRunning=false;
    private float squatForce;
    private float speed;
    void Start()
    {
        Player = GetComponent<Animator>();
        Player.SetBool("isIdle", true);
        Player.SetBool("isRunning", false);
        Player.SetBool("isSquatting", false);
        Player.SetBool("isJumping", false);
        Player.SetBool("isFalling", false);
        Player.SetBool("isPronning", false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        for(int i = 1; i < 10; i++)
        {
            string levelTag = "Level" + i;
            if(collision.tag == levelTag)
            {
                moveCameraToLevel(levelTag);
                Debug.Log("Moving to Level: " + i);
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
    void Update()
    {
        //move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //transform.Translate(move * speed * Time.deltaTime);
        ////if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        ////{
        ////    transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        ////}
        ////if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        ////{
        ////    transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        ////}
        ////if (Input.GetKeyDown(KeyCode.Space))
        ////{
        ////    transform.Translate(0, 2f, 0);
        if (Input.GetKey(KeyCode.LeftArrow) && !isSquatting && !isFalling && !isJumping)
        {
            Player.SetBool("isIdle", false);
            Player.SetBool("isRunning", true);

            gameObject.transform.Translate(Vector2.left * speedX * Time.deltaTime);
            if (gameObject.transform.localScale.x > 0)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
            }
            isRunning = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !isSquatting && !isFalling && !isJumping)
        {
            Player.SetBool("isIdle", false);
            Player.SetBool("isRunning", true);
 
            gameObject.transform.Translate(Vector2.right * speedX * Time.deltaTime);
            if (gameObject.transform.localScale.x < 0)
            {   
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
            }
            isRunning = true;
        }       
        else if (Input.GetKey(KeyCode.Space) && !isFalling && !isJumping && !isRunning)
        {
            Player.SetBool("isSquatting", true);
            isSquatting = true;

            gameObject.transform.Translate(new Vector2(0,0)); 
            squatForce += Time.deltaTime*3;
            Debug.Log("isSquatting");
        }
        else if(Input.GetKeyUp(KeyCode.Space) && isSquatting)
        {
            if (Input.GetKey(KeyCode.LeftArrow)) speed = -directionSpeed;
            else if (Input.GetKey(KeyCode.RightArrow)) speed = directionSpeed;
            else speed = 0;

            Debug.Log("squatforce"+squatForce);

            if(squatForce<minSquatForce) squatForce = minSquatForce;
            if(squatForce>maxSquatForce) squatForce = maxSquatForce;
           
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, squatForce * speedY);
            isSquatting = false;
            isJumping = true;
            squatForce = 0;
        }
        else if(gameObject.GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            Player.SetBool("isIdle", false);
            Player.SetBool("isJumping", true);

            Debug.Log("isJumping");
        }
        else if (gameObject.GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            Player.SetBool("isIdle", false);
            Player.SetBool("isFalling", true);
            isFalling = true;
            Debug.Log("isFalling");
        }
        //else if (gameObject.GetComponent<Rigidbody2D>().velocity.y==0)  
        //{
        //    Player.SetBool("isPronning", true);
        //    Debug.Log("isPronning");
        //}
        else
        {
            isFalling=false;
            isJumping = false;
            isRunning = false;

            Player.SetBool("isRunning", false);
            Player.SetBool("isSquatting", false);
            Player.SetBool("isIdle", true);
            Player.SetBool("isJumping", false);
            Player.SetBool("isFalling", false);
            Player.SetBool("isPronning", false);
            
            Debug.Log("after");
        }

    }
}
