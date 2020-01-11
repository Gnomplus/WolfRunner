using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject Menu;
    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float FallForce;
    [SerializeField] private float TimeToSpeedUp;
    [SerializeField] private int PregenerateDistance;
    private Animator Animator;
    private GridAnimation GridAnimator;
    private GameObject MainCamera;
    private Rigidbody2D Rb;
    private bool TimerCheck = true;
    private float StepPosition;
    private float groundPosY;
    private bool InputJump;
    private float CameraStartX;
    private bool ButtonHold = false;
    private float RunSpeed = 0.7f;
    private float JumpSpeed = 1.0f;
    private bool Alive = false;

    private void Start()
    {
        Animator = gameObject.GetComponent<Animator>();
        groundPosY = gameObject.transform.position.y;
        Rb = gameObject.GetComponent<Rigidbody2D>();
        GridAnimator = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GridAnimation>();
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraStartX = MainCamera.transform.position.x;
        StepPosition = gameObject.transform.position.x;       
    }

    public void Run()
    {
        Alive = true;
    }

    private void FixedUpdate()
    {
        if (Alive)
        {
            Rb.velocity = new Vector2(Speed, Rb.velocity.y);
            Animator.SetBool("Run", true);
        }
        MainCamera.transform.position = new Vector3(CameraStartX + gameObject.transform.position.x, MainCamera.transform.position.y, MainCamera.transform.position.z);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && transform.position.y <= groundPosY + 0.05f && Alive)
        {
            InputJump = true;
            Animator.SetTrigger("Jump");
            StartCoroutine("Jump");
        }
        if (Input.GetMouseButton(0))
        {
            ButtonHold = true;
        }
        else
        {
            ButtonHold = false;
        }
        if (gameObject.transform.position.x - StepPosition >= PregenerateDistance)
        {
            GridAnimator.GenerateBackground(75);
            GridAnimator.GenerateEnemies(75);
            GridAnimator.ClearUnusedBackground(Convert.ToInt32(transform.position.x * 3) - 6);
            StepPosition = gameObject.transform.position.x;
        }
        if (TimerCheck && Speed < 15f)
        {
            StartCoroutine(SpeedIncrease());
        }
    }

    IEnumerator SpeedIncrease()
    {
        TimerCheck = false;
        Speed += 0.1f;
        JumpForce += 0.1f;
        FallForce += 0.1f;
        RunSpeed += 0.01f;
        JumpSpeed += 0.1f;
        Animator.SetFloat("RunSpeed", RunSpeed);
        Animator.SetFloat("JumpSpeed", RunSpeed);
        yield return new WaitForSeconds(TimeToSpeedUp);
        TimerCheck = true;
    }

    IEnumerator Jump()
    {
        while (true)
        {           
            if (transform.position.y >= 0.45f)
            {
                InputJump = false;
            }               
            if (InputJump)
            {
                transform.Translate(Vector3.up * JumpForce * Time.smoothDeltaTime);
            }               
            else if (!InputJump)
            {
                transform.Translate(Vector3.down * (FallForce / (1f + (2.5f * Convert.ToSingle(ButtonHold)))) * Time.smoothDeltaTime);
                if (ButtonHold)
                {
                    Animator.SetFloat("JumpSpeed", JumpSpeed * 0.3f);
                }
                else
                {
                    Animator.SetFloat("JumpSpeed", JumpSpeed);
                }
                if (transform.position.y < groundPosY)
                {
                    transform.position = new Vector2(transform.position.x, groundPosY);                    
                    StopCoroutine("Jump");
                }
            }
            yield return new WaitForEndOfFrame();           
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && Alive)
        {
            Alive = false;
            Animator.SetBool("Run", false);
            Rb.velocity = new Vector2(0, Rb.velocity.y);
            Animator.SetTrigger("Death");
            gameObject.GetComponent<PlayerController>().enabled = false;
            Menu.GetComponent<GameMenuController>().DeathMenu();
        }
    }
}
