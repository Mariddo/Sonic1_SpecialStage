using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialStagePlayerController : MonoBehaviour
{
    [Header("Collectibles")]
    public int collectedItems;
    
    
    [Header("Checking for Grounding")]
    public bool isGrounded = true;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;
    public float lastTimeGrounded;
    
    [Header("Player Input")]
    private PlayerInputActions playerActionControls;
    public float moveInput;
    public float jumpInput;
        private void Awake() {

        playerActionControls = new PlayerInputActions();
    }


    [Header("Ending Conditions")] 
    public bool endingReached;
    public bool emeraldFound;
    public float endingRotateMin = 0.1f;
    public float endingRotateRate = 0.5f;
    public float endingRotatePerTick = 1f;

    private void OnEnable()
    {
        playerActionControls.Enable();
    }

    private void OnDisable()
    {
        playerActionControls.Disable();
    }
    
    [Header("Rotation")]
    public float rotateDelay = 0.25f;
    public float rotateAmount = 10f;

    float rotateTimer;
    public float rotateValue;

    public Vector3 gravityAngle;

    public float gravityConstant = 9.8f;


    Rigidbody2D rb;

    [Header("Movement Specs")]
    public float moveSpeed = 3f;
    public float jumpForce = 10f;
    public float airMoveMultiplier = 0.5f;

    [Header("Reverse Course")]
    float reverseTimer;
    public float reverseTimerDelay = 0.75f;

    [Header("Speed Change")]
    float speedChangeTimer;
    public float speedChangeDelay = 0.75f;
    public float[] rotateDelays;
    public int startingSpeedValue = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        rotateValue = 0f;

        rb = GetComponent<Rigidbody2D>();

        collectedItems = 0;

        emeraldFound = endingReached = false;

        rotateDelay = rotateDelays[startingSpeedValue];
    }

    // Update is called once per frame
    void Update()
    {
        RotateTimer();
        ChangeObjectGravity();
        CheckIfGrounded();

        if(!endingReached) {
            Move();
            Jump();
        } 

        if(reverseTimer > 0f) {
            reverseTimer -= Time.deltaTime;
        }
        if(speedChangeTimer > 0f) {
            speedChangeTimer -= Time.deltaTime;
        }

        EndingRoutine();
    }

    public void RotateTimer() {
        rotateTimer += Time.deltaTime;

        if(rotateTimer >= rotateDelay) {
            rotateValue += rotateAmount;
            
            rotateTimer = 0f;
        }
        transform.eulerAngles = Vector3.forward * rotateValue;
    }

    void ChangeObjectGravity() {

        

        gravityAngle.x = Mathf.Cos(Mathf.Deg2Rad * (rotateValue - 90));

        gravityAngle.y = Mathf.Sin(Mathf.Deg2Rad * (rotateValue - 90));

        Physics2D.gravity = gravityAngle * gravityConstant;
    }

    void Move() {

        moveInput = playerActionControls.BonusStage.Move.ReadValue<float>();

        if(moveInput != 0f) {

            float movementValue = moveInput * moveSpeed;
            if(!isGrounded) {
                movementValue *= airMoveMultiplier;
            }

            Vector2 movementForce = new Vector2(moveInput * moveSpeed, 0);

            Vector2 rotatedForce = Rotate(movementForce, Mathf.Deg2Rad * (rotateValue));

            rb.AddForce(rotatedForce, ForceMode2D.Impulse);
        }
    }

    void Jump() {

        jumpInput = playerActionControls.BonusStage.Jump.ReadValue<float>();

        if(jumpInput != 0 && isGrounded) {
            Vector2 movementForce = new Vector2(0, jumpForce);
            Vector2 rotatedForce = Rotate(movementForce, Mathf.Deg2Rad * (rotateValue));
            
            rb.velocity = rotatedForce;
        }
    }

    void CheckIfGrounded()
    {
        Collider2D colliders = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);

        if (colliders != null) {
            isGrounded = true;
        } else {
            if (isGrounded) {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
        }

        
    }

    Vector2 Rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
    

    void OnTriggerEnter2D(Collider2D other) {

        if(other.tag == "Pickup") {

            collectedItems++;

            other.GetComponent<PickupBehavior>().DestroySelf();

            SpecialStageUIBehavior.instance.UpdatePickupText(collectedItems);
        }

        if(other.tag == "Emerald") {
            other.GetComponent<PickupBehavior>().DestroySelf();
            endingReached = true;
            emeraldFound = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Goal") {
            endingReached = true;
        }
        

        if(other.gameObject.tag == "Reverse" && reverseTimer <= 0f) {

            reverseTimer = reverseTimerDelay;

            rotateAmount *= -1;
        } 
        if(other.gameObject.tag == "SpeedUp" && speedChangeTimer <= 0f) {

            speedChangeTimer = speedChangeDelay;
        }
        if(other.gameObject.tag == "SpeedDown" && speedChangeTimer <= 0f) {

            speedChangeTimer = speedChangeDelay;
        }
            
    }



    void UpdateRotateDelay() {


    }

    void EndingRoutine() {

        if(endingReached && rotateDelay > endingRotateMin) {

            rotateDelay -= endingRotateRate * Time.deltaTime;
            rotateAmount = endingRotatePerTick;
            EndingFreeze();
        }
    }

    void EndingFreeze() {
         rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
}
