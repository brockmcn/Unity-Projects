using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public static bool IsRotatedTowards(GameObject a, Vector2 b)
    {
        bool isRotatedTowards = false;

        if (a.transform.position.y > b.y)
            isRotatedTowards = a.transform.eulerAngles.z == 180;
        else if (a.transform.position.x < b.x)
            isRotatedTowards = a.transform.eulerAngles.z == -90;
        else if (a.transform.position.y < b.y)
            isRotatedTowards = a.transform.eulerAngles.z == 0;
        else if (a.transform.position.x > b.x)
            isRotatedTowards = a.transform.eulerAngles.z == 90;

        return isRotatedTowards;
    }

    public static void RotateTowards(GameObject a, Vector2 b)
    {
        if (a.transform.position.y > b.y)
            a.transform.eulerAngles = new Vector3(0, 0, 180);
        else if (a.transform.position.x < b.x)
            a.transform.eulerAngles = new Vector3(0, 0, -90);
        else if (a.transform.position.y < b.y)
            a.transform.eulerAngles = new Vector3(0, 0, 0);
        else if (a.transform.position.x > b.x)
            a.transform.eulerAngles = new Vector3(0, 0, 90);
    }

    public static Tile TileFacing(GameObject a)
    {
        Grid grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        Tile tile = null;

        if (a.transform.eulerAngles.z == 0 && (int)a.transform.position.y < grid.rows - 1)
            tile = grid.tiles[(int)a.transform.position.x, (int)a.transform.position.y + 1];
        else if (a.transform.eulerAngles.z == 180 && (int)a.transform.position.y > 0)
            tile = grid.tiles[(int)a.transform.position.x, (int)a.transform.position.y - 1];
        else if (a.transform.eulerAngles.z == 270 && (int)a.transform.position.x < grid.columns - 1)
            tile = grid.tiles[(int)a.transform.position.x + 1, (int)a.transform.position.y];
        else if (a.transform.eulerAngles.z == 90 && (int)a.transform.position.x > 0)
            tile = grid.tiles[(int)a.transform.position.x - 1, (int)a.transform.position.y];

        return tile;
    }

    public static bool FacingInBounds(GameObject a)
    {
        Grid grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        bool facingInBounds = false;

        if (a.transform.eulerAngles.z == 0 && (int)a.transform.position.y < grid.rows - 1
            || a.transform.eulerAngles.z == 180 && (int)a.transform.position.y > 0
            || a.transform.eulerAngles.z == 270 && (int)a.transform.position.x < grid.columns - 1
            || a.transform.eulerAngles.z == 90 && (int)a.transform.position.x > 0)
            facingInBounds = true;

        return facingInBounds;
    }
}
