using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Wander")]

public class WanderBehavior : FilteredFlockBehaviour
{
    private WaypointPath _path;
    [SerializeField] int _currentWaypoint;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (_path == null)
        {
            FindPath(agent, context);
        }

        return FollowPath(agent);
    }

    private Vector2 FollowPath(FlockAgent agent)
    {
        Debug.Log("Hit Follow Path");
        Debug.Log(_path);

        if (_path == null) return Vector2.zero;

        Vector3 waypointDir;
        Debug.Log("Waypoint " + _currentWaypoint);

        if (WaypointInRadius(agent, _currentWaypoint, out waypointDir))
        {
            _currentWaypoint++;
            if (_currentWaypoint >= _path.waypoints.Count)
                _currentWaypoint = 0;

            return Vector2.zero;
        }
        return (Vector2)waypointDir.normalized;
    }

    private bool WaypointInRadius(FlockAgent agent, int currentWaypoint, out Vector3 waypointDir)
    {
        waypointDir = (Vector2)(_path.waypoints[_currentWaypoint].position - agent.transform.position);

        return (waypointDir.magnitude < _path.radius);
    }

    private void FindPath(FlockAgent agent, List<Transform> context)
    {
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);

        if (filteredContext.Count == 0)
            return;

        int randomPath = Random.Range(0, filteredContext.Count);
        _path = filteredContext[randomPath].GetComponentInParent<WaypointPath>();
    }

}
