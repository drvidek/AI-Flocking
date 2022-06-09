using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParallax : MonoBehaviour
{
    [SerializeField] private float _parallaxScale;
    PlayerMain _player;
    Vector2 _scale;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerMain>();
        SpriteRenderer myRend = GetComponent<SpriteRenderer>();
        _scale = (Vector2)transform.localScale;
    }


    // Update is called once per frame
    void Update()
    {
        MoveWithPlayer();
    }

    public void MoveWithPlayer()
    {
        float _finalParallaxScale = GameManager.IsPaused() ? _parallaxScale/2f : _parallaxScale;
        transform.position -= (Vector3)_player.Velocity * Time.deltaTime * _finalParallaxScale;
        Vector3 _loopPos = transform.position;

        if (Mathf.Abs(transform.position.x) >= 2.56f * _scale.x)
        {
            _loopPos.x = 0;
        }
        if (Mathf.Abs(transform.position.y) >= 2.56f * _scale.y)
        {
            _loopPos.y = 0;
        }
        transform.position = _loopPos;

    }
}
