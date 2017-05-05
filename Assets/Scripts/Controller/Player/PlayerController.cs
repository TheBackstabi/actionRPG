using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float baseMovementSpeed = 1f;
    private float movementSpeed;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private ParticleSystem clickEffect;
    private float halfHeight;
    private bool canMove = true, forceStop = false;
    private Vector3 currentPosition;
    private CharacterController controller;
    private PlayerStats stats;
    public PlayerSpells spells;
    public bool inCombatMove = false;
    public float timeClickedOnEnemy = 0f;
    public GameObject targettedEnemy;
    private EventSystem eventSystem;
    private bool isMoving = false;

    private float maxMovementSpeed, minMovementSpeed;
    private int environmentMask = 1 << 0;
    private int enemyMask = 1 << 11;

    Vector3 moveToPosition;

    #region ACCESSORS

    public float MovementSpeed
    {
        get
        {
            return movementSpeed;
        }
        set
        {
            movementSpeed = value;
            Mathf.Clamp(movementSpeed, minMovementSpeed, maxMovementSpeed);
        }
    }
    public Vector3 PlayerPosition
    {
        get
        {
            return currentPosition;
        }
        set
        {
            Vector3 newPos = value;
            if (halfHeight != 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(value, new Vector3(0, -1, 0), out hit))
                {
                    newPos = hit.point;
                    newPos.y += halfHeight;
                }
            }
            currentPosition = gameObject.transform.position = newPos;
        }
    }
    public bool CanMove
    {
        get { return canMove; }
    }
    public bool ForceStop
    {
        get { return forceStop; }
    }

    #endregion

    void FixedUpdate()
    {
        if (canMove && isMoving)
        {
            if ((moveToPosition - transform.position).magnitude > 1f)
            {
                Vector3 moveDirection = (moveToPosition - currentPosition).normalized;
                moveDirection.y = 0;
                controller.Move(moveDirection * movementSpeed * .15f);
                Vector3 nextPos = currentPosition;
                nextPos.x = transform.position.x;
                nextPos.z = transform.position.z;
                RaycastHit hit;
                if (Physics.Raycast(nextPos, new Vector3(0, -1, 0), out hit, halfHeight + Mathf.Deg2Rad * controller.slopeLimit, environmentMask))
                {
                    nextPos.y = hit.point.y + halfHeight;
                    transform.position = currentPosition = nextPos;
                }
                else
                {
                    transform.position = currentPosition;
                }
            }
        }
    }

    public void MovePlayer(Vector3 nextMove)
    {
        moveToPosition = nextMove;
        isMoving = true;
    }
    
    public void StopMoving()
    {
        isMoving = false;
    }

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(this.gameObject);
        stats = gameObject.GetComponent<PlayerStats>();
        spells = gameObject.GetComponent<PlayerSpells>();
        movementSpeed = baseMovementSpeed;
        maxMovementSpeed = baseMovementSpeed * 2;
        minMovementSpeed = baseMovementSpeed * .5f;
        halfHeight = gameObject.GetComponent<MeshRenderer>().bounds.extents.y;
        controller = gameObject.GetComponent<CharacterController>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit))
        {
            Vector3 newPos = hit.point;
            newPos.y += halfHeight;
            transform.position = newPos;
        }
        currentPosition = this.gameObject.transform.position;
        playerCamera = Camera.main;
        eventSystem = FindObjectOfType<EventSystem>();
        moveToPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!GameVariables.isPaused)
        {
            movementSpeed = Mathf.Clamp(baseMovementSpeed * stats.IncreasedMovementSpeed, minMovementSpeed, maxMovementSpeed);
            if (playerCamera)
            {
                if (Input.GetButtonDown("Mouse PlayerMove"))
                {
                    RaycastHit hit;
                    Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                    if (!eventSystem.IsPointerOverGameObject())
                    {
                        if (!BindableInput.BindDown("Force Move", true) && Physics.Raycast(ray, out hit, int.MaxValue, enemyMask))
                        {
                            inCombatMove = true;
                            targettedEnemy = hit.collider.gameObject;
                            timeClickedOnEnemy = Time.time;
                            Vector3 look = hit.point;
                            look.y = transform.position.y;
                            transform.rotation = Quaternion.LookRotation(look - transform.position);
                        }
                        else
                        {
                            if (Physics.Raycast(ray, out hit, int.MaxValue, environmentMask))
                            {
                                ParticleSystem newClickEffect = Instantiate(clickEffect);
                                newClickEffect.transform.position = hit.point;
                                Destroy(newClickEffect.gameObject, newClickEffect.main.duration * 5);
                                inCombatMove = false;
                                targettedEnemy = null;
                                timeClickedOnEnemy = 0;
                            }
                        }
                    }
                }
                else if (Input.GetButton("Mouse PlayerMove") && !inCombatMove)
                {
                    RaycastHit hit;
                    Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                    if (!eventSystem.IsPointerOverGameObject())
                    {
                        if (Physics.Raycast(ray, out hit, int.MaxValue, environmentMask))
                        {
                            if (canMove && !BindableInput.BindDown("Force Stop", true))
                            {
                                Vector3 nextMove = hit.point;
                                nextMove.y += halfHeight;
                                MovePlayer(nextMove);
                            }
                            else
                                StopMoving();
                            Vector3 look = hit.point;
                            look.y = transform.position.y;
                            transform.rotation = Quaternion.LookRotation(look - transform.position);
                        }
                    }
                }
            }
        }
	}
}
