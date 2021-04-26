using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class PlatformerPlayer : MonoBehaviour
{
    private Rigidbody2D _body;
    private Animator _anim;
    private BoxCollider2D _box;
    
    private Vector3 respawnposition = new Vector3(-10, 0, 0);

    public float jumpForce = 12.0f;
    public float speed = 250.0f;

    private bool isDead = false;
    bool grounded = false;

    bool XBOXController = false;
    bool PS4Controller = false;
    bool SwitchController = false;
    bool NOController = true;

    //Setup of comonents
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _box = GetComponent<BoxCollider2D>();

        setupJoystickFlags();
        respawnPlayer();
    }

    //Reset animation, parent, scale, and spawn position
    public void respawnPlayer()
    {
        isDead = false;
        transform.parent = null;
        _anim.SetFloat("speed", 0);
        Vector3 pScale = Vector3.one;
        transform.localScale = new Vector3(1 / pScale.x, 1 / pScale.y, 1);
        transform.position = respawnposition;
    }

    void Update()
    {
        //Check is player is dead
        if (isDead == true)
            return;

        //Disable jump
        grounded = false;

        //Move forward (Left)
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector2 movement = new Vector2(deltaX, _body.velocity.y);
        _body.velocity = movement;

        //Sets colliders
        Vector3 max = _box.bounds.max;
        Vector3 min = _box.bounds.min;

        //Bottom detector
        Vector2 corner1 = new Vector2(max.x - 0.1f, min.y - 0.1f);
        Vector2 corner2 = new Vector2(min.x + 0.1f, min.y - 0.2f);

        //Right detector
        Vector2 corner3 = new Vector2(max.x + 0.1f, min.y + 0.3f);
        Vector2 corner4 = new Vector2(max.x + 0.2f, max.y - 0.3f);
        
        //Left detector
        Vector2 corner5 = new Vector2(min.x - 0.1f, min.y + 0.3f);
        Vector2 corner6 = new Vector2(min.x - 0.2f, max.y - 0.3f);

        //Colliders check surroundings
        Collider2D Bothit = Physics2D.OverlapArea(corner1, corner2);
        Collider2D Righthit = Physics2D.OverlapArea(corner3, corner4);
        Collider2D Lefthit = Physics2D.OverlapArea(corner5, corner6);

        //Enables jumps and wall jumps
        //If player stands on a platform call refrence
        MovingPlatform platform = null;
        if (Bothit != null)
        {  
            grounded = true;
            platform = Bothit.GetComponent<MovingPlatform>();
        }

        if (Righthit != null || Lefthit != null)
        {
            movement = new Vector2(0, _body.velocity.y);
            grounded = true;
        }

        //Makes the player affected by the gravity scale of the world
        _body.gravityScale = grounded & deltaX == 0 ? 0 : 1;
        if (grounded & deltaX == 0)
        {
            _body.gravityScale = 0;
        } else
        {
            _body.gravityScale = 1;
        }

        //Jump
        if (grounded && isJumpTriggered())
        {
            Debug.Log("Doing Jump");
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //Sets player to become child of a platform
        if (platform != null)
        {
            transform.parent = platform.transform;
        }
        else
        {
            transform.parent = null;
        }

        //Transition animations based on player's displacement
        _anim.SetFloat("speed", Mathf.Abs(deltaX));
        if (!Mathf.Approximately(deltaX, 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
        }

        //If player stands on platform, change the scale to the platform's scale
        Vector3 pScale = Vector3.one;
        if (platform != null)
        {
            pScale = platform.transform.localScale;
        }

        if (deltaX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX) / pScale.x, 1 / pScale.y, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //sets variable to player collider and calls death script
        GameObject checkCollision = collision.gameObject;
        TriggerDeath canKill = checkCollision.GetComponent<TriggerDeath>();

        //Remember location of each savepole upon collision
        if (collision.gameObject.tag == "save pole 1")
        {
            respawnposition = new Vector3(-5, 0, 0);
        }
        if (collision.gameObject.tag == "save pole 2")
        {
            respawnposition = new Vector3(5, 6, 0);
        }
        if (collision.gameObject.tag == "save pole 3")
        {
            respawnposition = new Vector3(-3, 7, 0);
        }

        //If player dies, disable animation, and respawn
        if (canKill != null)
        {
            isDead = true;
            _anim.SetFloat("speed", 0);
            StartCoroutine(Respawn());
        }
    }

    //Respawn time is 1 second
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.0f);
        respawnPlayer();
    }

   
    
    
    
    
    
    /*I did not touch this code...
    The only controller that worked for me was a PS4 controller
    I know that we are supposed to use xbox1 controllers but I don't own one */
    void setupJoystickFlags()
    {
        XBOXController = false;
        PS4Controller = false;
        SwitchController = false;
        NOController = true;

        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            // Uncomment if you want to see the names of your controllers.
            //Debug.Log("Joystick Name: " + Input.GetJoystickNames()[i]);

            if (Input.GetJoystickNames()[i].ToLower().IndexOf("xbox") >= 0)
                XBOXController = true;

            if (Input.GetJoystickNames()[i].ToLower().IndexOf("play") >= 0)
                PS4Controller = true;

            if (Input.GetJoystickNames()[i].ToLower().IndexOf("wireless") >= 0)
                SwitchController = true;
        }
    }

    private void doJumpDebug()
    {
        for (int i = 0; i < 10; i++)
        {
            string strButtonName = "joystick button " + i.ToString();

            if (Input.GetKeyDown(strButtonName))
                Debug.Log(strButtonName + " pushed");
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Debug.Log("Space pushed");
    }

    bool isJumpTriggered()
    {
        // Debug Joystick button pushes.
        //doJumpDebug();

        if (XBOXController)
            return Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space);

        if (PS4Controller)
            return Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Space);

        if (SwitchController)
            return Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space);

        return Input.GetKeyDown(KeyCode.Space);
    }
}
