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
    public bool isMoving = false;

    private float maxMovementSpeed, minMovementSpeed;
    private int environmentMask = 1 << 0;
    private int enemyMask = 1 << 11;

    public Vector3 moveToPosition;

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

    public void MovePlayer(Vector3 movePos)
    {
        isMoving = true;
        moveToPosition = movePos;
    }

    void FixedUpdate()
    {
        if (isMoving && canMove && !forceStop)
        {
            if ((moveToPosition - transform.position).magnitude > 1f)
            {
                Vector3 moveDirection = (moveToPosition - transform.position).normalized;
                moveDirection.y = 0;
                controller.Move(moveDirection * movementSpeed * .15f);
            }
            else
                isMoving = false;
        }
    }

    void Update()
    {
        if (BindableInput.BindDown("Force Stop"))
        {
            isMoving = false;
            forceStop = true;
        }
        if (BindableInput.BindUp("Force Stop"))
        {
            forceStop = false;
        }

        if (!eventSystem.IsPointerOverGameObject())
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit hit;
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, int.MaxValue, enemyMask))
                {
                    inCombatMove = true;
                    targettedEnemy = hit.collider.gameObject;
                    timeClickedOnEnemy = Time.time;
                    Vector3 look = hit.point;
                    look.y = transform.position.y;
                    transform.rotation = Quaternion.LookRotation(look - transform.position);
                }
                else if (Physics.Raycast(ray, out hit, int.MaxValue, environmentMask))
                {
                    isMoving = true;
                    inCombatMove = false;
                    targettedEnemy = null;
                    timeClickedOnEnemy = 0;
                    ParticleSystem newClickEffect = Instantiate(clickEffect);
                    newClickEffect.transform.position = hit.point;
                    Destroy(newClickEffect.gameObject, newClickEffect.main.duration * 5);
                }
            }

            if (!inCombatMove && Input.GetKey(KeyCode.Mouse0))
            {
                RaycastHit hit;
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, int.MaxValue, environmentMask))
                {
                    Vector3 nextMove = hit.point;
                    nextMove.y += halfHeight;
                    moveToPosition = nextMove;
                    Vector3 look = moveToPosition;
                    look.y = transform.position.y;
                    transform.rotation = Quaternion.LookRotation(look - transform.position);
                }
            }
        }
    }
}
