using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] botRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject xHall;
    public GameObject yHall;

    private GameObject entryRoom;

    private List<GameObject> allRooms;

    // Entry Room
    void Start()
    {
        allRooms = new List<GameObject>();
        for (int i = 0; i < botRooms.Length - 1; i++)
            allRooms.Add(botRooms[i]);
        for (int i = 0; i < topRooms.Length - 1; i++)
            allRooms.Add(topRooms[i]);
        for (int i = 0; i < leftRooms.Length - 1; i++)
            allRooms.Add(leftRooms[i]);
        for (int i = 0; i < rightRooms.Length - 1; i++)
            allRooms.Add(rightRooms[i]);

        int rand = Random.Range(0, allRooms.Count - 1);

        entryRoom = allRooms[rand];
        entryRoom = Instantiate(entryRoom);
        entryRoom.transform.Find("SpawnPoints").gameObject.SetActive(true);
        for (int i = 0; i < entryRoom.transform.Find("SpawnPoints").childCount; i++)
            entryRoom.transform.Find("SpawnPoints").GetChild(i).GetComponent<RoomSpawner>().isEntryRoom = true;
    }
}
