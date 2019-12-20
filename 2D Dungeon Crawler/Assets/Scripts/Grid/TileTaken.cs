using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTaken : MonoBehaviour
{
    public GameObject gridObject;

    void Update()
    {
        Grid grid = gridObject.GetComponent<Grid>();
        if (!grid.IsTileTaken(transform.position.x, transform.position.y))
            grid.SetTileTaken(transform.position.x, transform.position.y, true, gameObject);
    }
}
