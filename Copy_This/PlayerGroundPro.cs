using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundPro : MonoBehaviour
{

    private bool onGround;
    [SerializeField][Range(0f, 1f)] private float normalValue = 0.5f;
    [SerializeField] private Vector3 colliderOffset;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField][Range(2, 20)] private int checkStep = 2;
    [SerializeField] private AnimationCurve groundLengthCurve = AnimationCurve.Constant(0, 1, 1);

    public void Update()
    {
        RaycastHit2D castHit;
        Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 curOff = new Vector2(colliderOffset.x, colliderOffset.y);

        Vector2 curCastPos;
        float curIndex = 0f;
        float curLength = 0f;
        bool normalcheck = false;

        for (int i = 0; i < checkStep; i++)
        {

            curIndex = (float)i / (float)(checkStep - 1);

            curCastPos = Vector2.Lerp(curPos - curOff, curPos + curOff, curIndex);
            curLength = groundLengthCurve.Evaluate(curIndex);
            castHit = Physics2D.Raycast(curCastPos, Vector2.down, curLength, groundLayer);
            if (!castHit) continue;
            normalcheck = normalcheck || Mathf.Abs(Vector2.Dot((curCastPos - castHit.point).normalized, Vector2.up)) >= normalValue;
        }

        onGround = normalcheck;

    }
    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        if (onGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }

        Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 curOff = new Vector2(colliderOffset.x, colliderOffset.y);

        Vector2 curCastPos;
        float curIndex = 0f;
        float curLength = 0f;

        for (int i = 0; i < checkStep; i++)
        {

            curIndex = (float)i / (float)(checkStep - 1);

            curCastPos = Vector2.Lerp(curPos - curOff, curPos + curOff, curIndex);
            curLength = groundLengthCurve.Evaluate(curIndex);

            Gizmos.DrawLine(curCastPos, curCastPos + Vector2.down * curLength);
        }

    }

    public bool GetOnGround() { return onGround; }

}
