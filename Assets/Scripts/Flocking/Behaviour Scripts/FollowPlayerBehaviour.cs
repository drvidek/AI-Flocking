using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Follow Player")]
public class FollowPlayerBehaviour : FlockBehaviour
{
    public Transform _player;

    [SerializeField] private Vector2 _centre;
    [SerializeField] private float _radius = 15f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (_player == null)
            _player = GameObject.Find("Player").transform;
        _centre = _player.position;
        Vector2 _centreOffset = _centre - (Vector2)agent.transform.position;
        float t = _centreOffset.magnitude / _radius;

        if (t < 0.9f)
        {
            return Vector2.zero;
        }
        return _centreOffset * t * t;
    }
}
