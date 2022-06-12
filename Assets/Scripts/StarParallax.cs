using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParallax : MonoBehaviour
{
    public bool auto;
    [SerializeField] private float _parallaxScale;
    [SerializeField] private float _autoScale = 5f;
    PlayerMain _player;
    Vector2 _scale;
    static Vector3 _dir;

    // Start is called before the first frame update
    void Start()
    {
        _dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;

        if (GameObject.Find("Player"))
        _player = GameObject.Find("Player").GetComponent<PlayerMain>();
        SpriteRenderer myRend = GetComponent<SpriteRenderer>();
        _scale = (Vector2)transform.localScale;
    }


    // Update is called once per frame
    void Update()
    {
        if (!auto)
            MoveWithPlayer();
        else
            MoveAuto();
        Loop();
    }

    public void MoveWithPlayer()
    {
        float _finalParallaxScale = GameManager.IsPaused() ? _parallaxScale/2f : _parallaxScale;
        transform.position -= (Vector3)_player.Velocity * Time.deltaTime * _finalParallaxScale;
    }

    private void MoveAuto()
    {
        transform.position += _dir * _autoScale * Time.deltaTime;
    }

    private void Loop()
    {
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
