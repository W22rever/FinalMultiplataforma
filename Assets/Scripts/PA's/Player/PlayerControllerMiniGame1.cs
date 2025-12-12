using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D))]
public class PlayerControllerMiniGame1 : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float moveSpeed = 4f;
    
    private Vector2 _moveDirection;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    public void onFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Piu piu");
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _moveDirection * moveSpeed;
    }
}
