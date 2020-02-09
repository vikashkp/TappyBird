using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour {

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float tapForce = 210;
    public float tiltSmooth = 1;
    public Vector3 startPos;

    public AudioSource tapSound;
    public AudioSource scoreSound;
    public AudioSource dieSound;

    Rigidbody2D rb;
    Quaternion downRoatation;
    Quaternion forwardRotation;

    GameManager game;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        downRoatation = Quaternion.Euler(0, 0, -70);
        forwardRotation = Quaternion.Euler(0, 0, 30);
        game = GameManager.Instance;
        rb.simulated = false;
    }

    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDiable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void OnGameStarted()
    {
        rb.velocity = Vector3.zero;
        rb.simulated = true;
    }
    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (game.GameOver) { return; }
        if (Input.GetMouseButtonDown(0))
        {
            tapSound.Play();
            rb.velocity = Vector2.zero;
            transform.rotation = forwardRotation;
            rb.AddForce(Vector2.up * tapForce, ForceMode2D.Force);

        }
        transform.rotation = Quaternion.Lerp(transform.rotation, downRoatation, tiltSmooth * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "ScoreZone")
        {
            //register a score event
            OnPlayerScored();//event sent to gameManager
            //play a sound
            scoreSound.Play();
        }
        if (col.gameObject.tag == "DeadZone")
        {
            rb.simulated = false;
            //register a dead event
            OnPlayerDied();//event sent to gameManager
            //play a sound
            dieSound.Play();
        }
    }
}


/*
 *  * 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_movement : MonoBehaviour {
    public float speed = 0f;
    public float angle= 0f;
    public float velocity_value = 1f;
    Vector3 m_EulerAngleVelocity;
    Vector2 forward_direction = new Vector2(0,1);

    private Rigidbody2D rb;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = forward_direction * velocity_value;
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("a"))
        {
            angle += 0.314f;
        }
        else if (Input.GetKeyDown("d"))
        {
            angle -= 0.314f;
        }
        float a = Mathf.Abs(angle - (rb.rotation/6.28f));
        if(a > 0.8f){
            angle = (rb.rotation / 6.28f) - (a/Mathf.Abs(a) * 0.8f);
        }
        Vector2 forward_direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        rb.velocity = forward_direction * velocity_value;
        angle = angle % (6.28f);
        
        m_EulerAngleVelocity = new Vector3(0,0, angle*(180/3.14f));
    }
    void FixedUpdate()
    {

        rb.rotation = Mathf.Lerp(rb.rotation, angle * (180 / 3.14f), 0.2f);

        // Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
        // rb.MoveRotation((Vector3)rb.rotation *deltaRotation);
        //var rot = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().rotation);
        //rb.rotation = Quaternion.Lerp(rb.rotation, angle % (3.14f * 2), Time.deltaTime * 10f);
    }
}
*/