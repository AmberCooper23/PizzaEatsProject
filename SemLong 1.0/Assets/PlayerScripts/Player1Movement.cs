using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem; //Make sure to have this installed on Unity by going to Window >> Package Manager>>Choose Unity Registery>>
                               // Search for Input Systems then install
public class Player1Movement : MonoBehaviour //Attach to player 1 object
{
    private GameObject Player1;
    private Rigidbody2D rb1;

    public GameObject spatula; //Place the prefab for a spatula/projectile (NB ensure that the object has the SpatulaCol script attached to it before you make
    GameObject SpatulaObject; // it a Prefab.

    Keyboard key = Keyboard.current;
    [SerializeField]
    private InputAction playerOneMovement;

    [SerializeField]
    private TMP_Text aKeyOutput, dKeyOutput, aInput, dInput;

    public float JumpHeight = 2000f; //Change to alter the Jump Height (alter till it works for you)
    public float MovementSpeed = 10f; //Change for preferance
    public float SpatulaSpeed = 50f; //Recommended as spatula is a projectile

    private bool OnGround;
    private DirectionCheck direction;

   private Vector2 Input;

    // public float groundCheckRadius = 0.1f; Used to prevent player from jumping through shelves (not intended so all lines relating to this are commeneted out)
    // public LayerMask groundLayer;
    public bool isGrounded;
    // Start is called before the first frame update


    private void Awake()
    {
        playerOneMovement.performed += PlayerOneMovement_performed;
        playerOneMovement.canceled += PlayerOneMovement_canceled;
        //playerTwoMovement.performed += PlayerTwoMovement_performed  ;
    }

    private void PlayerOneMovement_canceled(InputAction.CallbackContext obj)
    {
        Input.x = 0;
    }

    private void PlayerOneMovement_performed(InputAction.CallbackContext obj)
    {
        var valueInput = obj.ReadValue<float>();
        Debug.Log($"PlayerOneMovement {valueInput}");
        aInput.text = dInput.text = valueInput.ToString();
        Input += Vector2.right * valueInput;
        Input.Normalize();
    }
    void Start()
    {
        Player1 = this.gameObject;
        rb1 = GetComponent<Rigidbody2D>();


        OnGround = true;

        direction = new DirectionCheck(false, false, false, false);
        Input = new Vector2();
    }

  //  public class PlayerController1
    //{
     //  private  Vector2 Vel = new Vector2();
       
        
        ///Players velocity
        ///
        
        

   // }

    private struct DirectionCheck //Gets direction player is moving in to adjust direction of projectile
    {
        public bool Up, Down, Left, Right;

        public DirectionCheck(bool up, bool down, bool left, bool right)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;

        }

    }



    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        Movement();
    }

    // void FixedUpdate()
    //{
    //  Vector2 position = rb1.position;
    //position.y -= groundCheckRadius;
    //Collider2D hit = Physics2D.OverlapPoint(position, groundLayer);
    //isGrounded = hit != null;
    //if (isGrounded)
    //{
    // Prevent the object from moving through the platform
    //  rb1.velocity = new Vector2(rb1.velocity.x, 0);
    //}
    // }

    private void Movement()
    {
        Vector2 Input = new Vector2();
        Debug.Log($"Keyboard: {key}");

        if (gameObject.tag == "Player1")
        {
            if (Keyboard.current.wKey.isPressed && OnGround == true)
            {
                rb1.AddForce(JumpHeight * Vector2.up);
                direction.Up = true;
                direction.Down = false;
                direction.Left = false;
                direction.Right = false;

            }

            if (Keyboard.current.aKey.isPressed)
            {
                Input += Vector2.left;
                direction.Up = false;
                direction.Down = false;
                direction.Left = true;
                direction.Right = false;
            }

            if (Keyboard.current.dKey.isPressed)
            {
                Input += Vector2.right;
                direction.Up = false;
                direction.Down = false;
                direction.Left = false;
                direction.Right = true;
            }

            if (Keyboard.current.sKey.isPressed)
            {
                direction.Up = false;
                direction.Down = true;
                direction.Left = false;
                direction.Right = false;

            }

            if (Keyboard.current.leftShiftKey.isPressed)
            {
                Destroy(SpatulaObject);
                SpatulaGenerator();
            }

            if (Keyboard.current.escapeKey.isPressed)
            {
                Application.Quit();
            }

            Input.Normalize();
            rb1.velocity = Input * MovementSpeed;
        }

        if (gameObject.tag == "Player")
        {
            if (Keyboard.current.upArrowKey.isPressed && OnGround == true)
            {
                rb1.AddForce(JumpHeight * Vector2.up);
                direction.Up = true;
                direction.Down = false;
                direction.Left = false;
                direction.Right = false;

            }

            if (Keyboard.current.leftArrowKey.isPressed)
            {
                Input += Vector2.left;
                direction.Up = false;
                direction.Down = false;
                direction.Left = true;
                direction.Right = false;
            }

            if (Keyboard.current.rightArrowKey.isPressed)
            {
                Input += Vector2.right;
                direction.Up = false;
                direction.Down = false;
                direction.Left = false;
                direction.Right = true;
            }

            if (Keyboard.current.downArrowKey.isPressed)
            {
                direction.Up = false;
                direction.Down = true;
                direction.Left = false;
                direction.Right = false;

            }

            if (Keyboard.current.rightShiftKey.isPressed)
            {
                Destroy(SpatulaObject);
                SpatulaGenerator();
            }

            if (Keyboard.current.escapeKey.isPressed)
            {
                Application.Quit();
            }

            Input.Normalize();
            rb1.velocity = Input * MovementSpeed;
        }
        
        


    }

    private Vector2 Spatula()
    {
        Vector2 SpatulaDirection = new Vector2();
        Vector2 SpatulaVelocity = new Vector2();
      //  float Fixed = 0.25f;


        if (direction.Up)
        {
            SpatulaDirection = Vector2.up;
        }

        if (direction.Down)
        {
            SpatulaDirection = Vector2.down;
        }

        if (direction.Right)
        {
            SpatulaDirection = Vector2.right;
        }

        if (direction.Left)
        {
            SpatulaDirection = Vector2.left;
        }

        SpatulaVelocity = SpatulaDirection * SpatulaSpeed ;

        return SpatulaVelocity;

    }

    private void SpatulaGenerator()
    {
        SpatulaObject = Instantiate(spatula, rb1.position, Quaternion.identity);
        //SpatulaObject.AddComponent<Rigidbody2D>();
        Rigidbody2D rbS = SpatulaObject.GetComponent<Rigidbody2D>();
        Transform transformS = SpatulaObject.transform;
        transformS.position = Player1.transform.position;


        rbS.velocity = Spatula();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {

        OnGround = true;


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnGround = false;
        if (collision.gameObject == gameObject.CompareTag("Enemy")) //Fixes a bug where player can no longer jump when enemy collides
        {
            OnGround = true;
        }
    }

    private void OnEnable()
    {
        playerOneMovement.Enable();
    }

    private void OnDisable()
    {
        playerOneMovement.Disable();
    }


}