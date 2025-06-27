using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerScript : MonoBehaviour
{
    private PlayerInput playerInput;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector3 playerVelocity;
    [SerializeField, Tooltip("Player speed multiplier")]
    private float playerSpeed = 2.0f;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    void FixedUpdate()
    {

        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector2 move = new Vector3(input.x, input.y);
        move = Vector2.ClampMagnitude(move, 1f); 
        
        transform.localScale = float.IsNegative(input.x) ?
                                transform.localScale = new Vector3(-1, 1, 1) :
                                transform.localScale = new Vector3(1, 1, 1);

        if (move != Vector2.zero)
            animator.SetBool("IsWalking", true);
        else
            animator.SetBool("IsWalking", false);

        Vector3 newPosition = transform.position + new Vector3(move.x, move.y, 0) * playerSpeed * Time.fixedDeltaTime;
        transform.position = newPosition;
    }
}
