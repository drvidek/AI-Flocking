using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Steered Cohesion")]
public class SteeredCohesionBehaviour : CohesionBehaviour
{
    private Vector2 _currentVelocity;
    public float agentSmoothTime = 0.5f;
    
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {

        Vector2 cohesionMove = base.CalculateMove(agent, context, flock);

        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref _currentVelocity, agentSmoothTime);

        return cohesionMove;
    }
}