using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Movement")]
public class PlayerMovementStats : ScriptableObject
{
    #region Walk

    [Header("Walk Variable")]
    [Tooltip("Max Speed character can move")][Range(0f, 20f)] public float maxSpeed = 10f;
    [Tooltip("Max Acceleration on ground")][Range(0f, 100f)] public float maxAcceleration = 50f;
    [Tooltip("Max Deceleration on ground")][Range(0f, 100f)] public float maxDeceleration = 50f;
    [Tooltip("Max Turn speed on ground")][Range(0f, 100f)] public float maxTurnSpeed = 50f;
    [Tooltip("Max Acceleration in air")][Range(0f, 100f)] public float maxAirAcceleration;
    [Tooltip("Max Deaceleration in air")][Range(0f, 100f)] public float maxAirDeceleration;
    [Tooltip("Max Turn speed in air")][Range(0f, 100f)] public float maxAirTurnSpeed = 50f;
    [Tooltip("Friction froce when character move")][Range(0f, 10f)] public float friction = 0f;
    public bool useAcceleration = true;
    #endregion

    [Space(20)]

    #region Jump

    [Header("Jump Variable")]
    [Tooltip("How Amount that player can Jump in midair")][Range(0, 2)] public int airJumpCount = 0;
    [Tooltip("How Maximum Height that character can jump")][Range(0.01f, 20f)] public float maxjumpHeight = 1;
    [Tooltip("Accelerate value that character drop or up")][Range(0f, 100f)] public float gravityForce = 9.8f;
    [Tooltip("Maximum Time that still can jump while not onGround")][Range(0f, 1f)] public float maxCoyoteTimeLimit = 1f;
    [Tooltip("Maximum Time that can press jump while unable to")][Range(0f, 1f)] public float jumpBufferTimeLimit = 0.25f;
    [Tooltip("How long for pressing jump to reach maximum height")][Range(0f, 1f)] public float jumpDynamicTimeLimit = 0.1f;
    [Tooltip("Scale Between press and hold jumping")][Range(0.01f, 1)] public float jumpDynamicScale = 1f;
    [Tooltip("Scale Between upPhase and downPhase (less for quick up, great for quick drop)")][Range(0.5f, 1.5f)] public float airborneScale = 1f;

    #endregion
}
