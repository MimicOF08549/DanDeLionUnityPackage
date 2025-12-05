using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Events;


[RequireComponent(typeof(PlayerGroundPro), typeof(Rigidbody2D))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField] PlayerGroundPro playerGround;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerMovementStats playerMovementStats;

    public float Velocity
    {
        get => m_velocity;
    }

    public bool IsJumped
    {
        get => m_isJumping;
    }
    public bool EnableToJump
    {
        get => _enableJump;
        set => _enableJump = value;
    }

    [SerializeField] UnityEvent _OnJumpEvent = new();

    #region JumpVAR

    [SerializeField] private bool _enableJump = true;
    [SerializeField] private int _airJumpCount = 1;
    [SerializeField] private float _jumpHeight = 1;
    [SerializeField] private float _gravityForce = 9.8f;
    [SerializeField] private float _coyoteTimeLimit = 1f;
    [SerializeField] private float _jumpBufferTimeLimit = 0.25f;
    [SerializeField] private float _jumpDynamicTimeLimit = 0.1f;
    [SerializeField] private float _jumpDynamicScale = 1f;
    [SerializeField] private float _airborneScale = 1f;

    #endregion


    #region Private Var

    [SerializeField] private int m_curAirJumpCount;
    [SerializeField] private float m_velocity;
    [SerializeField] private bool m_isJumping;
    [SerializeField] private float m_lastVelocity;
    [SerializeField] private bool m_isPerfromJump;
    [SerializeField] private bool m_canPerformJump = true;
    [SerializeField] private float m_jumpTiming;
    [SerializeField] private float m_jumpDistanceAgo;
    [SerializeField] private bool m_isCoyoteTime;
    [SerializeField] private float m_falldownTime;
    [SerializeField] private bool m_isPauseBreak;
    [SerializeField] private int m_selfPauseCount;
    [SerializeField] private bool m_HoldingJump = false;

    Coroutine m_bufferCoroutine = null;

    #endregion


    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!playerGround) playerGround = GetComponent<PlayerGroundPro>();
        InitialMovementStats();
        m_curAirJumpCount = _airJumpCount;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_isPerfromJump = true;
            if (!m_isJumping && (playerGround.GetOnGround() || m_isCoyoteTime) && m_canPerformJump && _enableJump)
            {
                Action(true);
                if (m_bufferCoroutine != null)
                {
                    StopCoroutine(m_bufferCoroutine);
                    m_bufferCoroutine = null;
                }
            }
            else if (m_curAirJumpCount > 0 && !m_HoldingJump && _enableJump)
            {
                Action(true);
                if (m_bufferCoroutine != null)
                {
                    StopCoroutine(m_bufferCoroutine);
                    m_bufferCoroutine = null;
                }
            }
            else
            {
                m_bufferCoroutine = StartCoroutine(nameof(BufferJump));
            }
        }

        if (context.canceled)
        {
            m_isPerfromJump = false;
            m_HoldingJump = false;
            Action(false);
        }
    }



    private void Update()
    {

        float vertiVelo = 0;


        vertiVelo = m_velocity;
        if (!m_isJumping)
        {
            m_canPerformJump = JumpChecker();

        }



        if (m_isJumping && playerGround.GetOnGround() && vertiVelo >= 0)
        {
            vertiVelo = AirborneCalculate();
            m_canPerformJump = false;
            m_isCoyoteTime = false;
        }
        else if (m_isPerfromJump && m_isJumping && m_jumpTiming < _jumpDynamicTimeLimit && m_HoldingJump)
        {
            m_jumpTiming += Mathf.Min(Time.deltaTime, Time.fixedDeltaTime);
            m_jumpTiming = Mathf.Clamp(m_jumpTiming, 0, _jumpDynamicTimeLimit);
            vertiVelo = AirborneCalculate();
            m_jumpDistanceAgo +=
                (vertiVelo - _gravityForce * (1 / _airborneScale) * Mathf.Min(Time.deltaTime, Time.fixedDeltaTime)) *
                Mathf.Min(Time.deltaTime, Time.fixedDeltaTime);
        }
        else if (m_isJumping && playerGround.GetOnGround() && vertiVelo < 0)
        {
            m_isJumping = false;
            m_canPerformJump = true;
            m_curAirJumpCount = _airJumpCount;

        }


        if (!playerGround.GetOnGround() || (playerGround.GetOnGround() && m_isCoyoteTime))
        {
            if (vertiVelo <= 0) vertiVelo -= _gravityForce * Mathf.Min(Time.deltaTime, Time.fixedDeltaTime);
            if (vertiVelo > 0) vertiVelo -= _gravityForce * (1f / _airborneScale) * Mathf.Min(Time.deltaTime, Time.fixedDeltaTime);
        }


        if ((playerGround.GetOnGround() || (m_canPerformJump && !m_isCoyoteTime)) && vertiVelo < 0)
        {
            vertiVelo = 0;
        }


        m_velocity = vertiVelo;

        if (!m_isPauseBreak) rb.linearVelocity = new Vector3(rb.linearVelocity.x, Mathf.Max(-30f, m_velocity) /** Mathf.Min(Time.deltaTime, Time.fixedDeltaTime)*/);

    }


    public void Action(bool isPress)
    {

        if (isPress && !m_isJumping && (playerGround.GetOnGround() || m_isCoyoteTime) && m_canPerformJump && _enableJump)
        {
            _OnJumpEvent?.Invoke();
            m_isJumping = true;
            m_canPerformJump = false;
            m_isCoyoteTime = false;
            m_jumpTiming = 0;
            m_jumpDistanceAgo = 0;
            m_HoldingJump = true;
        }
        else if (isPress && !m_HoldingJump && m_curAirJumpCount > 0 && _enableJump)
        {
            _OnJumpEvent?.Invoke();
            m_isJumping = true;
            m_canPerformJump = false;
            m_isCoyoteTime = false;
            m_jumpTiming = 0;
            m_jumpDistanceAgo = 0;
            m_HoldingJump = true;
            m_curAirJumpCount--;

        }


        if (!isPress)
        {
            m_HoldingJump = false;
        }
    }


    private bool JumpChecker()
    {


        if (playerGround.GetOnGround())
        {
            m_falldownTime = _coyoteTimeLimit;
            m_isCoyoteTime = false;
            return true;
        }
        else if (m_falldownTime > 0)
        {
            m_falldownTime -= Mathf.Min(Time.deltaTime, Time.fixedDeltaTime);
            m_isCoyoteTime = true;
            return true;
        }
        else if (m_isCoyoteTime)
        {
            m_isCoyoteTime = false;
            return false;
        }

        return false;
    }


    private float AirborneCalculate()
    {
        float minHeight = _jumpDynamicScale * _jumpHeight;
        float divideResult = _jumpDynamicTimeLimit == 0 ? 1 : m_jumpTiming / _jumpDynamicTimeLimit;
        float dynamicHeight = Mathf.Lerp(minHeight, _jumpHeight, divideResult) - m_jumpDistanceAgo;
        float velocityReturn = Mathf.Sqrt(2f * _gravityForce * (1f / _airborneScale) * dynamicHeight);

        return velocityReturn;
    }


    public void AddForceOneFrame(float YForce)
    {
        if (YForce > 0) ResetVelocity();
        StartCoroutine(AddForce(YForce, Mathf.Min(Time.deltaTime, Time.fixedDeltaTime)));
    }

    public void AddForceContinuously(float YForce, float duration)
    {
        if (YForce > 0) ResetVelocity();
        StartCoroutine(AddForce(YForce, duration));
    }

    public void AddForceImpulse(float YForce, bool forceTo)
    {
        m_velocity = forceTo ? YForce : m_velocity + YForce;
    }

    public void PauseBreak(float breakTime)
    {
        StartCoroutine(RoutineBreak(breakTime));
    }

    private IEnumerator AddForce(float force, float time)
    {
        float countdown = 0;
        while (countdown < time)
        {
            if (!m_isPauseBreak) rb.linearVelocity = new Vector3(rb.linearVelocity.x, force /** Mathf.Min(Time.deltaTime, Time.fixedDeltaTime)*/);
            countdown += Mathf.Min(Time.deltaTime, Time.fixedDeltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator RoutineBreak(float breakTime)
    {
        m_isPauseBreak = true;
        m_velocity = 0;
        m_selfPauseCount++;
        yield return new WaitForSeconds(breakTime);
        m_selfPauseCount = Mathf.Max(0, --m_selfPauseCount);
        m_isPauseBreak = m_selfPauseCount > 0;
    }

    private IEnumerator BufferJump()
    {
        float timeBuffLimit = _jumpBufferTimeLimit;
        while (timeBuffLimit > 0)
        {
            yield return new WaitForEndOfFrame();
            timeBuffLimit -= Time.deltaTime;
            if (!m_isJumping && (playerGround.GetOnGround() || m_isCoyoteTime) && m_canPerformJump && _enableJump)
            {
                Action(true);
                if (m_bufferCoroutine != null)
                {
                    StopCoroutine(m_bufferCoroutine);
                    m_bufferCoroutine = null;
                }
                if (!m_isPerfromJump)
                {
                    m_HoldingJump = false;
                }

                break;
            }
        }

        yield return null;
    }

    public void ResetVelocity()
    {
        m_velocity = 0;
    }

    private void InitialMovementStats()
    {
        if (!playerMovementStats)
        {
            Debug.LogWarning($"Please assign playerMovementStats on {this.name}. disable components");
            this.enabled = false;
            return;
        }

        _airJumpCount = playerMovementStats.airJumpCount;
        _jumpHeight = playerMovementStats.maxjumpHeight;
        _gravityForce = playerMovementStats.gravityForce;
        _coyoteTimeLimit = playerMovementStats.maxCoyoteTimeLimit;
        _jumpBufferTimeLimit = playerMovementStats.jumpBufferTimeLimit;
        _jumpDynamicTimeLimit = playerMovementStats.jumpDynamicTimeLimit;
        _jumpDynamicScale = playerMovementStats.jumpDynamicScale;
        _airborneScale = playerMovementStats.airborneScale;
    }
}
