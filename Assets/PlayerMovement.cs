using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed, runMultiplier;
    private Rigidbody2D rb2d;
    private Vector2 input;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float currSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currSpeed *= runMultiplier;
        }
        else
        {

        }
        input.x = Input.GetAxisRaw("Horizontal") * currSpeed;
        input.y = Input.GetAxisRaw("Vertical") * currSpeed;
    }

    private void FixedUpdate()
    {
        //rb2d.velocity = new Vector2(input.x, input.y);
        rb2d.velocity = input;

    }
}
