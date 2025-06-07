using UnityEngine;

public class EnemyRandMovement : MonoBehaviour
{
    // movement
    Rigidbody2D rigidbody2d;
    public float moveSpeed = 2;
    //Vector2 move;
    public bool moveVertical = true;
    public int direction = 1;
    public float timerMin = 1.0f;
    public float timerMax = 5.0f;
    float moveTimerIncrement = 0.0f;
    float moveTimer = 2.5f;

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
    }


    // FixedUpdate
    /*
     * 
     * 
     * 
     */
    private void FixedUpdate()
    {

            Vector2 position = rigidbody2d.position;

            // determine movement
            if (moveVertical)
            {
                position.y = position.y + moveSpeed * direction * Time.deltaTime;
            }
            else
            {
                position.x = position.x + moveSpeed * direction * Time.deltaTime;
            }
            rigidbody2d.MovePosition(position);
    }
}
