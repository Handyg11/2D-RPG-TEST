using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed, runMultiplier;
    private Rigidbody2D rb2d;
    private Vector2 input;
    private Vector2 mouseDirection; // Stores direction toward the mouse

    // Melee Attack
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int damage = 1;
    public LayerMask enemyLayers;
    public float attackCooldown = 0.5f;
    private float nextAttackTime = 0f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float currSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currSpeed *= runMultiplier;
        }

        // Player movement
        input.x = Input.GetAxisRaw("Horizontal") * currSpeed;
        input.y = Input.GetAxisRaw("Vertical") * currSpeed;

        // Rotate player toward the mouse
        RotateTowardsMouse();

        // Attack
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Change to your attack button
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void RotateTowardsMouse()
    {
        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure it's in 2D space

        // Get direction from player to mouse
        mouseDirection = (mousePosition - transform.position).normalized;

        // Calculate angle and apply rotation
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Attack()
    {
        // Detect enemies in attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Vector2 directionToEnemy = (enemy.transform.position - transform.position).normalized;

            // Player's current forward direction is now based on mouse rotation
            Vector2 playerForward = mouseDirection;

            float angle = Vector2.SignedAngle(playerForward, directionToEnemy);

            if (angle >= -45 && angle <= 45)
            {
                Debug.Log("Hit FRONT of " + enemy.name);
                //enemy.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            }
            else if (angle > 45 && angle < 135)
            {
                Debug.Log("Hit RIGHT SIDE of " + enemy.name);
                //enemy.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            }
            else if (angle < -45 && angle > -135)
            {
                Debug.Log("Hit LEFT SIDE of " + enemy.name);
                //enemy.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            }
            else
            {
                Debug.Log("Hit BACK of " + enemy.name);
                //enemy.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            }
        }
    }

    private void FixedUpdate()
    {
        rb2d.velocity = input;
    }
}
