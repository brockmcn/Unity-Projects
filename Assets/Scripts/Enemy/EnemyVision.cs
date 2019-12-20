using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public int visionLength;
    private Vector2 visionDir;
    private float[] xPosCone;
    private float[] yPosCone;

    private EnemyController enemyController;

    private void Start()
    {
        // Left to right order
        xPosCone = new float[2] { transform.position.x, transform.position.x };
        yPosCone = new float[2] { transform.position.x, transform.position.x };

        enemyController = transform.GetComponent<EnemyController>();
    }

    void Update()
    {
        Rotation();

        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(xPosCone[0], yPosCone[0]), visionDir, visionLength - 1);
        RaycastHit2D hitMiddle = Physics2D.Raycast(transform.position, visionDir, visionLength);
        RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(xPosCone[1], yPosCone[1]), visionDir, visionLength - 1);

        ShowVision(); // Used to see vision cone

        if ((hitLeft.collider != null && hitLeft.collider.tag == "Player") || (hitMiddle.collider != null && hitMiddle.collider.tag == "Player") || (hitRight.collider != null && hitRight.collider.tag == "Player"))
        {
            enemyController.playerFound = true;
        }
    }

    void Rotation()
    {
        if (transform.eulerAngles.z == 180)
        {
            visionDir = Vector2.down;
            // Left
            xPosCone[0] = transform.position.x + 1;
            yPosCone[0] = transform.position.y - 1;
            // Right
            xPosCone[1] = transform.position.x - 1;
            yPosCone[1] = transform.position.y - 1;
        }
        else if (transform.eulerAngles.z == 0)
        {
            // Left
            visionDir = Vector2.up;
            xPosCone[0] = transform.position.x - 1;
            yPosCone[0] = transform.position.y + 1;
            // Right
            xPosCone[1] = transform.position.x + 1;
            yPosCone[1] = transform.position.y + 1;
        }
        else if (transform.eulerAngles.z == 90)
        {
            // Left
            visionDir = Vector2.left;
            xPosCone[0] = transform.position.x - 1;
            yPosCone[0] = transform.position.y - 1;
            // Right
            xPosCone[1] = transform.position.x - 1;
            yPosCone[1] = transform.position.y + 1;
        }
        else if (transform.eulerAngles.z == -90)
        {
            // Left
            visionDir = Vector2.right;
            xPosCone[0] = transform.position.x + 1;
            yPosCone[0] = transform.position.y + 1;
            // Right
            xPosCone[1] = transform.position.x + 1;
            yPosCone[1] = transform.position.y - 1;
        }
    }

    void ShowVision()
    {
        Debug.DrawRay(new Vector2(xPosCone[0], yPosCone[0]), visionDir * (visionLength - 1), Color.yellow);
        Debug.DrawRay(transform.position, visionDir * visionLength, Color.yellow);
        Debug.DrawRay(new Vector2(xPosCone[1], yPosCone[1]), visionDir * (visionLength - 1), Color.yellow);
    }
}
