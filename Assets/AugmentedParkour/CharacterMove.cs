using UnityEngine;

/// <summary>
/// Unityちゃんの行動制御スクリプト
/// </summary>
public class CharacterMove : MonoBehaviour
{
    public float speed = 16f;
    public float rotateSpeed = 160f;

    public bool RestrictionOnControl = true;

    private Animator animator = null;
    private Rigidbody charactorRigidbody;

    Vector3 velocity = new Vector3(0, 0, 0);
    Vector3 befPos = new Vector3(0, 0, 0);

    const float WALK_VELOCITY = 40.0f;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        charactorRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Unityちゃんの制御に関係なく、
        //「動いた」「動かされた」どちらの場合でも、「移動」モーションを再生する
        if (RestrictionOnControl)
        {
            Vector3 sub = transform.localPosition - befPos;
            Vector3 normalSub = new Vector3(sub.x, 0f, sub.z).normalized;

            if (0 < normalSub.magnitude)
            {
                animator.SetFloat("Speed", 2.0f);
                transform.LookAt(transform.localPosition + normalSub);
            }
            else
            {
                animator.SetFloat("Speed", 0.0f);
            }

            befPos = transform.localPosition;
        }
        else
        {
            if (WALK_VELOCITY < velocity.magnitude)
            {
                animator.SetFloat("Speed", 2.0f);
            }
            else
            {
                animator.SetFloat("Speed", 0.0f);
            }
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            if (jumpEnable)
            {
                jumpAccelerating = true;
                jumpEnable = false;

                if (jumpAccelerateFrameCount >= maxJumpAccelerateFrameCount)
                {
                    jumpAccelerating = false;
                    downForce = downForceConfig;
                }
            }
        }
        else
        {
            jumpAccelerateFrameCount--;
            if (jumpAccelerateFrameCount < 0)
            {
                jumpAccelerateFrameCount = 0;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpAccelerating = false;
            downForce = downForceConfig;
        }
    }

    public bool jumpAccelerating = false;
    public bool jumpEnable= false;
    public float jumpForce = 240.0f;
    public float downForce = 0.0f;
    public float downForceConfig = 120.0f;
    public int jumpAccelerateFrameCount = 0;
    public int maxJumpAccelerateFrameCount = 16;

    private void OnCollisionEnter(Collision collision)
    {
        jumpAccelerateFrameCount = 0;
        jumpEnable = true;
    }

    void CalcJump()
    {
        if (jumpAccelerating && jumpAccelerateFrameCount < maxJumpAccelerateFrameCount)
        {
            jumpAccelerateFrameCount++;
            charactorRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
        else
        {
            charactorRigidbody.AddForce(Vector3.down * downForce, ForceMode.VelocityChange);
        }
    }

    /// <summary>
    /// GameObjectの実際の「移動」の処理はこっちで行う
    /// </summary>
    void FixedUpdate()
    {
        if (RestrictionOnControl)
        {
            return;
        }

        CalcJump();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        velocity = new Vector3(0, 0, v);
        // キャラクターのローカル空間での方向に変換
        velocity = transform.TransformDirection(velocity);
        // キャラクターの移動
        transform.localPosition += velocity * speed * Time.fixedDeltaTime;
        // キャラクターの回転
        transform.Rotate(0, h * rotateSpeed * Time.fixedDeltaTime, 0);

        Vector3 vec3 = new Vector3();
        vec3.x = transform.position.x;
        vec3.y = 10000.0f;
        vec3.z = transform.position.z;

        RaycastHit hitInfo;
        if (Physics.Raycast(vec3, Vector3.down, out hitInfo))
        {
            if (hitInfo.collider.gameObject.name != "unitychan")
            {
                if (hitInfo.point.y > transform.position.y)
                {
                    Vector3 newPos = transform.position;
                    newPos.y = hitInfo.point.y;
                    transform.position = newPos;
                }
            }
        }
    }

}
