using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;

    public Bullet bullet;

    public float view_distance = 10;

    public Vector3 target_pos;

    public AStarGrid grid;

    public enum State
    {
        IDLE, LOOKING, CHASING
    }

    public State state = State.IDLE;

    public List<Vector3> path = new();

    void ShootBullet()
    {
        var player_pos = player.transform.position;

        player_pos += player.Motion * 2;

        var my_pos = transform.position;

        var bullet_speed = (player_pos - my_pos).normalized * 0.5f;

        var new_bullet = GameObject.Instantiate(bullet);
        new_bullet.transform.position = my_pos;
        new_bullet.speed = bullet_speed;

    }

    float timer = 0;

    void FixedUpdate()
    {
        //Debug.Log("State: " + state + ", Timer: " + timer);
        switch (state)
        {
            case State.IDLE:

                if (Vector3.Distance(player.transform.position,  transform.position) < view_distance)
                {
                    state = State.LOOKING;
                    timer = 0;
                }

                break;
            case State.LOOKING:

                if (Vector3.Distance(player.transform.position, transform.position) < view_distance)
                {
                    if (timer < 0)
                    {
                        timer += Time.fixedDeltaTime;
                    } else
                    {
                        state = State.CHASING;
                        timer = 0;

                        path = grid.FindPathTo(transform.position, player.transform.position, 16);
                    }
                } else
                {
                    state = State.IDLE;
                    timer = 0;
                }

                break;
            case State.CHASING:

                target_pos = player.transform.position;

                if (path.Count > 0)
                {
                    target_pos = path[0] + new Vector3(0.5f, 0.5f, 0.5f);

                    if (Vector3.Distance(player.transform.position, transform.position) > 2)
                    {

                        if (Vector3.Distance(transform.position, target_pos) > 0.5f)
                        {
                            Vector3 target_dir = (target_pos - transform.position).normalized;
                            target_dir.y *= 0;

                            float speed = 0.05f;

                            transform.position += target_dir * speed;
                        } else
                        {
                            path.RemoveAt(0);
                        }

                        
                    }

                    Vector3 player_pos = new Vector3(Mathf.Floor(player.transform.position.x), 0, Mathf.Floor(player.transform.position.z));

                    if (player_pos != path[path.Count - 1])
                    {
                        path = grid.FindPathTo(transform.position, player.transform.position, 16);
                    }
                }

                

                if (timer < 1)
                {
                    timer += Time.fixedDeltaTime;
                }
                else
                {
                    if (path.Count == 0)
                    {
                        path = grid.FindPathTo(transform.position, player.transform.position, 16);
                    }
                    ShootBullet();
                    timer = 0;
                }

                break;
        }

        
    }
}
