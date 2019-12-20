using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player and camera vars
    private float horizontal;
    private float vertical;
    private bool sameRot;

    // Player speed
    public int currentSpeed;
    public int walkSpeed;
    public int sprintSpeed;
    public int crouchSpeed;

    // Crouch
    bool isCrouching;

    // Grid
    public GameObject gridObject;
    private Grid grid;

    // Camera
    public Camera mainCamera;

    // Key input timer
    private float totalInputTime;
    private float currentInputTime;
    float minInputTime;

    // Player Inventory
    PlayerInventory playerInventory;
    
    void Start()
    {
        horizontal = transform.position.x;
        vertical = transform.position.y;

        currentSpeed = walkSpeed;

        grid = gridObject.GetComponent<Grid>();

        mainCamera.transform.position = new Vector3(horizontal, vertical, -10);

        currentInputTime = Time.deltaTime;
        minInputTime = 0.2f;

        playerInventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        Movement();
        Interact();
        Combat();
    }

    void Movement()
    {
        // Basic movement
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (HasReachedTile())
            {
                Rotate(0);
                if (totalInputTime == 0 && sameRot)
                    minInputTime = 0;
                totalInputTime += currentInputTime;
                if (vertical < grid.rows - 1 && !grid.IsTileTaken(horizontal, vertical + 1)
                    && (totalInputTime >= minInputTime)) vertical++;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (HasReachedTile())
            {
                Rotate(90);
                if (totalInputTime == 0 && sameRot)
                    minInputTime = 0;
                totalInputTime += currentInputTime;
                if (horizontal > 1 && !grid.IsTileTaken(horizontal - 1, vertical)
                    && (totalInputTime >= minInputTime)) horizontal--;

            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (HasReachedTile())
            {
                Rotate(180);
                if (totalInputTime == 0 && sameRot)
                    minInputTime = 0;
                totalInputTime += currentInputTime;
                if (vertical > 1 && !grid.IsTileTaken(horizontal, vertical - 1) 
                    && (totalInputTime >= minInputTime)) vertical--;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (HasReachedTile())
            {
                Rotate(270);
                if (totalInputTime == 0 && sameRot)
                    minInputTime = 0;
                totalInputTime += currentInputTime;
                if (horizontal < grid.columns - 1 && !grid.IsTileTaken(horizontal + 1, vertical)
                    && (totalInputTime >= minInputTime)) horizontal++;
            }
        }

        // Reset total input time
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow)
            || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            totalInputTime = 0;
            minInputTime = 0.2f;
        }

        // Sprinting
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching) { currentSpeed = sprintSpeed; }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !isCrouching) { currentSpeed = walkSpeed;  }

        // Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (isCrouching) { isCrouching = false; currentSpeed = walkSpeed; }
            else { isCrouching = true; currentSpeed = crouchSpeed; }
        }


        // Move player
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(horizontal, vertical),
            currentSpeed * Time.deltaTime);

        // Move camera
        mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position,
            new Vector3(horizontal, vertical, -10), currentSpeed * Time.deltaTime);
    }

    void Rotate(int rotation)
    {
        int rot = 0;

        if (rotation == 0 && transform.eulerAngles.z != 0) { rot = 0; sameRot = false; }
        else if (rotation == 90 && transform.eulerAngles.z != 90) { rot = 90; sameRot = false; }
        else if (rotation == 180 && transform.eulerAngles.z != 180) { rot = 180; sameRot = false; }
        else if (rotation == 270 && transform.eulerAngles.z != 270) { rot = 270; sameRot = false; }
        else sameRot = true;

        if (!sameRot) transform.eulerAngles = new Vector3(0, 0, rot);
    }

    bool HasReachedTile()
    {
        Vector2 endGoal = new Vector2(horizontal, vertical);
        return (Vector2)transform.position == endGoal;
    }

    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Rotator.FacingInBounds(gameObject) && Rotator.TileFacing(gameObject).isTaken)
            {
                Tile tile = Rotator.TileFacing(gameObject);
                if (tile.obj.tag == "Chest")
                {
                    Chest chest = tile.obj.GetComponent<Chest>();
                    playerInventory.gold += chest.getGold();
                }
            }
        }
    }

    void Combat()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Rotator.TileFacing(gameObject).isTaken && Rotator.TileFacing(gameObject).tag == "Enemy")
                Debug.Log("Attack");
        }
    }
}
