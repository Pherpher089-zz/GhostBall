using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(ActorEquipment))]
//[RequireComponent(typeof(Animator))]
public class ActorCharacter : MonoBehaviour
{
    public bool isPlayer = false;

    [SerializeField] float m_turnSmooth = 5;
    [SerializeField] float m_JumpPower = 12f;
    [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;
    [SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField] public float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    [SerializeField] float m_GroundCheckDistance = 0.1f;

    Rigidbody m_Rigidbody;
    //Animator m_Animator;
    CapsuleCollider m_Capsule;
    GameObject m_CharacterObject;
    GameObject camObj;
    Vector3 camFoward;
    [HideInInspector] public bool m_IsGrounded;
	float m_OrigGroundCheckDistance;
	const float k_Half = 0.5f;
	float m_xMovement;
	float m_zMovement;
	Vector3 m_GroundNormal;
	float m_CapsuleHeight;
	Vector3 m_CapsuleCenter;

    CameraController cam;
	bool m_Crouching;
    float maxDist;

    //EquipmentVariables
    ActorEquipment charEquipment;

    //Melee Attack
    bool rTriggerDown = false;
    Animator animator;
    int swingTool;


    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_CharacterObject = transform.Find("CharacterBody").gameObject;
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;
        charEquipment = GetComponent<ActorEquipment>();
        camObj = GameObject.FindWithTag("MainCamera");
        camFoward = camObj.transform.parent.forward.normalized;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
        cam = GameObject.FindWithTag("CamObj").GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        swingTool = Animator.StringToHash("SwingTool");
    }


    public void UseEquipment(float primary, float secondary)
    { 
        if(charEquipment.hasItem)
        {
            charEquipment.equipedItem.GetComponent<Item>().PrimaryAction(primary);
            charEquipment.equipedItem.GetComponent<Item>().SecondaryAction(secondary);
        }
        else
        {
            if (!rTriggerDown && primary > 0)
            {
                animator.SetTrigger(swingTool);
            }

            if (primary > 0)
            {
                rTriggerDown = true;
            }
            else
            {
                rTriggerDown = false;
            }
        }
    }

	public void Move(Vector3 move, bool crouch, bool jump)
	{

		// convert the world relative moveInput vector into a local-relative
		// turn amount and forward amount required to head in the desired
		// direction.

		if (move.magnitude > 1f) move.Normalize();
		move = camObj.transform.TransformDirection(move);
		CheckGroundStatus();
		move = Vector3.ProjectOnPlane(move, m_GroundNormal);
		//m_TurnAmount = Mathf.Atan2(move.x, move.z);
		m_zMovement = move.z * m_MoveSpeedMultiplier;
        m_xMovement = move.x * m_MoveSpeedMultiplier;



		// control and velocity handling is different when grounded and airborne:
		if (m_IsGrounded)
		{
			HandleGroundedMovement(crouch, jump);
                
        }
		else
		{
			HandleAirborneMovement();
		}

		ScaleCapsuleForCrouching(crouch);
		PreventStandingInLowHeadroom();

        // send input and other state parameters to the animator
        //UpdateAnimator(move);

        Vector3 preMove = new Vector3(m_xMovement*2, m_Rigidbody.velocity.y, m_zMovement);
  

        //TODO pause this
        m_Rigidbody.velocity = new Vector3(m_xMovement, m_Rigidbody.velocity.y, m_zMovement) ;

    }

    public void Turning(Vector3 direction)
    {
        //TODO pause this too i guess
        Quaternion newDir;

        if (direction != Vector3.zero)
        {
            direction = camObj.transform.TransformDirection(direction);
            direction = new Vector3(direction.x, 0, direction.z);
        }
        else if (m_Rigidbody.velocity != Vector3.zero)
        {
            direction = new Vector3(m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z);
          
        }
        else
        {
            return;
        }

        newDir = Quaternion.LookRotation(direction.normalized, transform.up);
        m_Rigidbody.rotation = Quaternion.Slerp(m_Rigidbody.rotation, newDir, Time.deltaTime * m_turnSmooth);
    }

	void ScaleCapsuleForCrouching(bool crouch)
	{
		if (m_IsGrounded && crouch)
		{
			if (m_Crouching) return;
			m_Capsule.height = m_Capsule.height / 2f;
			m_Capsule.center = m_Capsule.center / 2f;
			m_Crouching = true;
            m_CharacterObject.transform.localScale = (new Vector3(1, .5f, 1));
		}
		else
		{
			Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
			float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				m_Crouching = true;
				return;
			}
			m_Capsule.height = m_CapsuleHeight;
			m_Capsule.center = m_CapsuleCenter;
            m_CharacterObject.transform.localScale = (new Vector3(1, 1 , 1));
            m_Crouching = false;
		}
	}

	void PreventStandingInLowHeadroom()
	{
		// prevent standing up in crouch-only zones
		if (!m_Crouching)
		{
			Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
			float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				m_Crouching = true;
			}
		}
	}

    void HandleAirborneMovement()
	{
		// apply extra gravity from multiplier:
		Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
		m_Rigidbody.AddForce(extraGravityForce);

		m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
	}

	void HandleGroundedMovement(bool crouch, bool jump)
	{
		// check whether conditions are right to allow a jump:
		if (jump && !crouch /*&& m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded")*/)
		{
			// jump!

            //TODO Pause this
			m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
			m_IsGrounded = false;
			//m_Animator.applyRootMotion = false;
			m_GroundCheckDistance = 0.1f;
		}
	}

    void CheckGroundStatus()
	{
		RaycastHit hitInfo;
#if UNITY_EDITOR
		// helper to visualise the ground check ray in the scene view
		Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
		// 0.1f is a small offset to start the ray from inside the character
		// it is also good to note that the transform position in the sample assets is at the base of the character
		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
		{
			m_GroundNormal = hitInfo.normal;
			m_IsGrounded = true;
			//m_Animator.applyRootMotion = true;
		}
		else
		{
			m_IsGrounded = false;
			m_GroundNormal = Vector3.up;
			//m_Animator.applyRootMotion = false;
		}
	}
}

