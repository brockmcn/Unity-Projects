using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DynamicallyChangingNav : MonoBehaviour
{

    public AIWayPointNetwork WayPointNetwork = null;
    public int CurrentIndex = 0;
    public bool HasPath = false;
    public bool PathPending = false;
    public bool PathStale = false;
    public NavMeshPathStatus PathStatus = NavMeshPathStatus.PathInvalid;
    public AnimationCurve JumpCurve = new AnimationCurve();
    public List<Transform> scaredWaypoints;

    private NavMeshAgent _navAgent = null;

    // Use this for initialization
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        if (WayPointNetwork == null) return;

        SetNextDestination(false);
    }

    void SetNextDestination(bool newDestination)
    {

        if (!WayPointNetwork) return;

        int randomDestination = Random.Range(0, WayPointNetwork.Waypoints.Count);
        Transform nextWaypointTransform = null;

        int nextWaypoint = (CurrentIndex == randomDestination) ? Random.Range(0, WayPointNetwork.Waypoints.Count) : randomDestination; //Picks random waypoint not equal to current waypoint hopefully...
        nextWaypointTransform = WayPointNetwork.Waypoints[nextWaypoint];

        if (nextWaypointTransform != null)
        {
            CurrentIndex = nextWaypoint;
            _navAgent.destination = nextWaypointTransform.position;
            return;
        }

        CurrentIndex = nextWaypoint;


    }

    public void SetNextDestination(int newDestination)
    {
        if (!WayPointNetwork) return;

        Transform nextWaypointTransform = null;
        nextWaypointTransform = WayPointNetwork.Waypoints[newDestination];

        if (nextWaypointTransform != null)
        {
            CurrentIndex = newDestination;
            _navAgent.destination = nextWaypointTransform.position;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HasPath = _navAgent.hasPath;//can't use haspath because of unity bug with haspath and jumplinks, known bug not yet fixed, use remaining distance instead, 
        PathPending = _navAgent.pathPending;
        PathStale = _navAgent.isPathStale;
        PathStatus = _navAgent.pathStatus;//for if you can't reach the waypoint set the agent does a partial path as far as it can, stops, the path is considered completed and it moves to the next path

        if (_navAgent.isOnOffMeshLink)
        {
            StartCoroutine(Jump(1.0f));
            return;
        }

        if ((/*!HasPath*/_navAgent.remainingDistance <= _navAgent.stoppingDistance && !PathPending) || PathStatus == NavMeshPathStatus.PathInvalid /*|| PathStatus == NavMeshPathStatus.PathPartial*/)//uncomment last condition if you want the agent to not even attempt to move to waypoints that would require executing only a partial path because they are cut off in some way, and instead simply move to the waypoint after that
            SetNextDestination(true);
        else if (_navAgent.isPathStale)
            SetNextDestination(false);
    }

    IEnumerator Jump(float duration)
    {
        OffMeshLinkData data = _navAgent.currentOffMeshLinkData;
        Vector3 startPos = _navAgent.transform.position;
        Vector3 endPos = data.endPos + (+_navAgent.baseOffset * Vector3.up);
        float time = 0.0f;

        while (time <= duration)
        {
            float t = time / duration;
            _navAgent.transform.position = Vector3.Lerp(startPos, endPos, t) + JumpCurve.Evaluate(t) * Vector3.up;
            time += Time.deltaTime;
            yield return null;
        }

        _navAgent.CompleteOffMeshLink();
    }
}

//navmeshagent stop, resume, and set destination methods are useful if you want your agents to do something when interrupted, and setteringtarget for seeing where going