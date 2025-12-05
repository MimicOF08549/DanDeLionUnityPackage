using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
public class PlayerWalk : MonoBehaviour
{
    public PlayerMovementStats playerStats;
    PlayerGroundPro ground;
    private Rigidbody2D _body;

    private float maxSpeed;
    private float maxAcceleration;
    private float maxDeceleration;
    private float maxTurnSpeed;
    private float maxAirAcceleration;
    private float maxAirDeceleration;
    private float maxAirTurnSpeed;
    private float friction;

    private bool useAcceleration;

    [HideInInspector]public Vector2 direction;
    private Vector2 velocity;
    private Vector2 desiredVelocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;

    private bool onGround;
    private bool pressingKey;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        ground = GetComponent<PlayerGroundPro>();
        InitialMovementStats();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    public void Update()
    {
        if(direction.x != 0)
        {
            transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
            pressingKey = true;
        }
        else
        {
            pressingKey = false;
        }

        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - friction, 0f);
    }

    public void FixedUpdate()
    {
        onGround = ground.GetOnGround();

        velocity = _body.linearVelocity;

        if (useAcceleration)
        {
            RunWithAcceleration();
        }
        else
        {
            if (onGround)
            {
                RunWithoutAcceleration();
            }
            else
            {
                RunWithAcceleration();
            }
        }
    }

    private void RunWithAcceleration()
    {
        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        deceleration = onGround ? maxDeceleration : maxAirDeceleration;
        turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;

        if (pressingKey) 
        {
            if (Mathf.Sign(direction.x) != Mathf.Sign(velocity.x))
            {
                maxSpeedChange = turnSpeed * Time.deltaTime;
            }
            else 
            {
                maxSpeedChange = acceleration * Time.deltaTime;
            }
        }
        else
        {
            maxSpeedChange = deceleration * Time.deltaTime;
        }

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        _body.linearVelocity = velocity;
    }

    private void RunWithoutAcceleration()
    {
        velocity.x = desiredVelocity.x;

        _body.linearVelocity = velocity;
    }

    private void InitialMovementStats()
    {
        maxSpeed = playerStats.maxSpeed;
        maxAcceleration = playerStats.maxAcceleration;
        maxDeceleration = playerStats.maxDeceleration;
        maxTurnSpeed = playerStats.maxTurnSpeed;
        maxAirAcceleration = playerStats.maxAirAcceleration;
        maxAirDeceleration = playerStats.maxAirDeceleration;
        maxAirTurnSpeed = playerStats.maxAirTurnSpeed;
        friction = playerStats.friction;

        useAcceleration = playerStats.useAcceleration;
    }
}
