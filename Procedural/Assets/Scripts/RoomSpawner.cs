using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 --> need bot door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door

    private RoomTemplates templates;
    private int rand;

    public bool isEntryRoom;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke(nameof(Spawn), 0.1f);
    }

    void Spawn()
    {
        if (isEntryRoom)
        {
            switch (openingDirection)
            {
                case 1:
                    rand = Random.Range(0, templates.botRooms.Length);
                    Instantiate(templates.botRooms[rand], transform.position, templates.botRooms[rand].transform.rotation);
                    break;
                case 2:
                    rand = Random.Range(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
                    break;
                case 3:
                    rand = Random.Range(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                    break;
                case 4:
                    rand = Random.Range(0, templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                    break;
            }
        }

        else if (!isEntryRoom)
        {
            int r = Random.Range(3, 8);
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);

            switch (openingDirection)
            {
                case 1:
                    for (int i = 0; i < r; i++)
                    {
                        Instantiate(templates.yHall, pos, templates.yHall.transform.rotation);
                        pos = new Vector2(transform.position.x, transform.position.y + 5);
                    }
                    break;
                case 2:
                    for (int i = 0; i < r; i++)
                    {
                        Instantiate(templates.yHall, pos, templates.yHall.transform.rotation);
                        pos = new Vector2(transform.position.x, transform.position.y - 5);
                    }
                    break;
                case 3:
                    for (int i = 0; i < r; i++)
                    {
                        Instantiate(templates.xHall, pos, templates.xHall.transform.rotation);
                        pos = new Vector2(transform.position.x + 9, transform.position.y);
                    }
                    break;
                case 4:
                    for (int i = 0; i < r; i++)
                    {
                        Instantiate(templates.xHall, pos, templates.xHall.transform.rotation);
                        pos = new Vector2(transform.position.x - 9, transform.position.y);
                    }
                    break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
            Destroy(gameObject);
    }
}
