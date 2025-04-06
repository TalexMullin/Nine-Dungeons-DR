using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum MoveType
    {
        randomWander,
        homing
    }

    // movement
    public MoveType movementType = MoveType.randomWander;
    Rigidbody2D rigidbody2d;
    Rigidbody2D playerRigidbody2d;
    Vector2 playerPosition;
    GameObject target = null;
    public float moveSpeed = 2;
    bool moveVertical = true;
    public int direction = 1;
    public float timerMin = 1.0f;
    public float timerMax = 5.0f;
    float moveTimerIncrement = 0.0f;
    float moveTimer = 2.5f;
    public float antiJitterVar = 0.075f;

    [Header("Knockback")]
    public bool knockbackActive = false; // public so that HealthImpacterEnemy can use it
    float knockbackTimer;
    public float knockbackAmount = 0.2f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*
     * 
     * 
     * 
     */
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    /*
     * 
     * 
     * 
     */
    void Update()
    {
        if (!PlayerController.gamePaused)
        {
            /* random movement
             * 
             */
            if (movementType == MoveType.randomWander)
            {
                // increment moveTimerIncrement, then reset and decide new moveVertical, direction, and moveTimer
                moveTimerIncrement += Time.deltaTime;
                if (moveTimerIncrement >= moveTimer)
                {
                    // reset moveTimerIncrement
                    moveTimerIncrement = 0;
                    // determine new moveTimer
                    //TODO: consider making this float instead of int
                    moveTimer = Random.Range(timerMin, timerMax);
                    // determine new direction (positive or negative)
                    direction = Random.Range(0, 2);
                    if (direction == 0) { direction = -1; }
                    // determine if moving vertical or not (i.e. horizontal).
                    if (Random.Range(0, 2) == 1)
                    {
                        moveVertical = true;
                    }
                    else
                    {
                        moveVertical = false;
                    }
                }
                /*
                 * there is nothing here for homing movement
                 */
            }

            // knockback timer
            if (knockbackActive)
            {
                knockbackTimer += Time.deltaTime;
                if (knockbackTimer >= knockbackAmount)
                {
                    rigidbody2d.linearVelocity = Vector2.zero;
                    knockbackActive = false;
                    knockbackTimer = 0;
                }
            }
        }
    }


    // FixedUpdate
    /*
     * 
     * 
     * 
     */
    private void FixedUpdate()
    {
        if (!PlayerController.gamePaused)
        {
            if (!knockbackActive)
            {
                Vector2 position = rigidbody2d.position;
                /* random movement
                 * 
                 */
                if (movementType == MoveType.randomWander)
                {


                    // determine movement
                    if (moveVertical)
                    {
                        position.y = position.y + moveSpeed * direction * Time.deltaTime;
                    }
                    else
                    {
                        position.x = position.x + moveSpeed * direction * Time.deltaTime;
                    }

                    /* homing movement
                     * 
                     */
                }
                else if (movementType == MoveType.homing)
                {
                    if (target != null)
                    {   // if there is a target, move towards it
                        playerPosition = playerRigidbody2d.position;
                        // decide which direction to go
                        if (Mathf.Abs(playerPosition.x - position.x) > antiJitterVar)
                        {
                            if (playerPosition.x > position.x)
                            {
                                position.x = position.x + moveSpeed * Time.deltaTime;
                            }
                            else if (playerPosition.x < position.x)
                            {
                                position.x = position.x - moveSpeed * Time.deltaTime;
                            }
                        }
                        if (Mathf.Abs(playerPosition.y - position.y) > antiJitterVar)
                        {
                            if (playerPosition.y > position.y)
                            {
                                position.y = position.y + moveSpeed * Time.deltaTime;
                            }
                            else if (playerPosition.y < position.y)
                            {
                                position.y = position.y - moveSpeed * Time.deltaTime;
                            }
                        }
                    }
                    else
                    {   // otherwise, find the target
                        target = GameObject.FindWithTag("Player");
                        playerRigidbody2d = target.GetComponent<Rigidbody2D>();
                    }
                }
                rigidbody2d.MovePosition(position);
            } // end (!knockbackActive);
        }
    }
}
