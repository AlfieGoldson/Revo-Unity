using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField]
    KeyInput
        upKey = new KeyInput(KeyCode.Z),
        downKey = new KeyInput(KeyCode.S),
        leftKey = new KeyInput(KeyCode.Q),
        rightKey = new KeyInput(KeyCode.D),
        jumpKey = new KeyInput(KeyCode.Space);

    [SerializeField]
    float
        speed = 5f,
        jumpForce = 5f,
        defaultGravityMultiplier = 1f,
        fastFallGravityMultiplier = 6f,
        momentumGainSpeed = 6f,
        maxVelocity = 5f,
        maxTravelDistance = 50f;

    [SerializeField]
    int
        jumpFrames = 12,
        jumpCount = 0,
        maxAerialJumps = 2,
        doubleTapDelay = 12;

    public float gravityMultiplier = 1f;

    Vector2 movement = Vector2.zero;

    float lastJumpTime = 0;

    [SerializeField]
    bool
        isGrounded = false,
        isJumping = false,
        isFastFalling = false;

    [SerializeField] Transform sprite;

    [Header("Planet")]
    [SerializeField] GravityAttractor planet;
    [SerializeField] LayerMask planetMask;

    [Header("Trail")]
    [SerializeField] LineRenderer trail;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityMultiplier = defaultGravityMultiplier;
    }

    void Update()
    {
        Vector2 newMovement = new Vector2(
            (leftKey.CheckKey() ? -1 : 0) + (rightKey.CheckKey() ? 1 : 0),
            (downKey.CheckKey() ? -1 : 0) + (upKey.CheckKey() ? 1 : 0)
            );
        movement = Vector2.Lerp(movement, newMovement, Time.deltaTime * momentumGainSpeed);

        CheckGround();
        CheckDoubleTaps();

        isJumping = TimeFrames.ToFrames(Time.time - lastJumpTime) <= jumpFrames;

        if (isGrounded)
            jumpCount = 0;

        if (downKey.CheckKeyUp() || isGrounded)
            isFastFalling = false;

        if (!isJumping)
        {
            if (jumpKey.CheckKeyDown() && jumpCount < maxAerialJumps)
            {
                rb.velocity = transform.up * jumpForce;
                lastJumpTime = Time.time;
                if (!isGrounded)
                    jumpCount++;
            }
            else if (downKey.CheckKeyDown())
            {
                isFastFalling = true;
            }
        }

        isFastFalling = downKey.CheckKey() && isFastFalling;

        transform.Translate(Vector3.right * movement.x * speed * Time.deltaTime);

        gravityMultiplier = isFastFalling ? fastFallGravityMultiplier : defaultGravityMultiplier;

        MovementToAimAngle(newMovement);
    }

    void FixedUpdate()
    {
        planet.Attract(rb, gravityMultiplier);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }

    void CheckGround()
    {
        isGrounded = Vector3.Magnitude(planet.transform.position - transform.position) <= ((transform.localScale.x + planet.transform.localScale.x) / 2);
    }

    void CheckDoubleTaps()
    {
        if (leftKey.CheckDoubleTap(doubleTapDelay))
            Dash(Vector3.left);
        if (rightKey.CheckDoubleTap(doubleTapDelay))
            Dash(Vector3.right);
        if (upKey.CheckDoubleTap(doubleTapDelay))
            ChangePlanet();
        if (downKey.CheckDoubleTap(doubleTapDelay))
            TeleportDown();
    }

    void Dash(Vector2 direction)
    {
        Debug.Log("Dash!");
    }

    void TeleportDown()
    {
        Debug.Log("Teleport!");
        if (isGrounded)
        {
            Vector3 teleportationVector = 2 * (planet.transform.position - transform.position);
            transform.position += teleportationVector;
        }
        else
        {
            // Teleport To the Ground
        }
    }

    void ChangePlanet()
    {
        Debug.Log("Travel!");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 50f, planetMask);

        Debug.DrawRay(transform.position, transform.up * 1000, Color.red, 10f);

        if (hit.collider != null)
        {
            Debug.Log(hit.transform.tag);
            if (hit.transform.CompareTag("Planet"))
            {
                Debug.Log("Meteor");
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                distance -= transform.localScale.x + hit.transform.localScale.x;

                planet = hit.transform.GetComponent<GravityAttractor>();
            }
        }
    }

    void MovementToAimAngle(Vector2 _movement)
    {
        Quaternion rot = movement.magnitude >= 0.1f ? Quaternion.FromToRotation(sprite.up, movement.normalized) * sprite.localRotation * transform.rotation : Quaternion.Euler(Vector3.zero);
        sprite.localRotation = Quaternion.Lerp(sprite.localRotation, rot, Time.deltaTime * 16f);
    }
}

[System.Serializable]
public class KeyInput
{
    [SerializeField] KeyCode key;
    [SerializeField] float lastKeyStroke = 0;
    [SerializeField] bool keyStroked = false;

    public KeyInput(KeyCode _key)
    {
        key = _key;
    }

    public bool CheckKey()
    {
        return Input.GetKey(key);
    }

    public bool CheckKeyDown()
    {
        if (Input.GetKeyDown(key))
        {
            lastKeyStroke = Time.time;
            keyStroked = true;
            return true;
        }
        return false;
    }

    public bool CheckKeyUp()
    {
        return Input.GetKeyUp(key);
    }

    public bool CheckDoubleTap(int doubleTapDelay)
    {
        bool _keyStroked = keyStroked;
        float _lastKeyStroke = lastKeyStroke;
        bool keyDown = CheckKeyDown();
        if (_keyStroked && keyDown && (TimeFrames.ToFrames(Time.time - _lastKeyStroke) <= doubleTapDelay))
        {
            keyStroked = false;
            return true;
        }
        return false;
    }
}