using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]

public class AvoidanceBehaviour : FilteredFlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        Vector2 avoidanceMove = Vector2.zero;

        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);

        int count = 0;
        foreach (Transform item in filteredContext)
        {
            if (Vector2.SqrMagnitude(item.position - agent.transform.position) <= flock.SquareAvoidanceRadius)
            {
                avoidanceMove += (Vector2)(agent.transform.position - item.position);
                count++;
            }
        }
        if (count != 0)
        {
            avoidanceMove /= count;
        }

        return avoidanceMove;
    }
}