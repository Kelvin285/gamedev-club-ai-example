using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    public bool[] filled = new bool[256 * 256];

    public GameObject cube;

    public bool started = false;

    void Start()
    {
        
    }

    private void Update()
    {
        if (!started)
        {
            started = true;

            foreach (var t in gameObject.GetComponentsInChildren<Transform>())
            {
                var rotation = t.rotation;
                var scale = t.lossyScale;
                var pos = t.position;


                float width = Mathf.Ceil(scale.x);
                float length = Mathf.Ceil(scale.z);

                for (int x = -(int)Mathf.Ceil(width / 2); x < (int)Mathf.Ceil(width / 2); x++)
                {
                    for (int z = -(int)Mathf.Ceil(length / 2); z < (int)Mathf.Ceil(length / 2); z++)
                    {
                        Vector3 world_pos = new Vector3(x, 0, z);
                        world_pos = rotation * world_pos;
                        world_pos += pos;

                        int world_x = (int)Mathf.Floor(world_pos.x);
                        int world_y = (int)Mathf.Floor(world_pos.z);

                        GameObject.Instantiate(cube);
                        cube.transform.position = world_pos + new Vector3(0.5f, 0.5f, 0.5f);

                        SetFilled(world_x, world_y, true);
                    }
                }
            }
        }
    }

    public bool IsFilled(int x, int y)
    {
        int wrapped_x = x & 255;
        int wrapped_y = y & 255;
        int index = wrapped_x + wrapped_y * 256;
        return filled[index];
    }

    public void SetFilled(int x, int y, bool f)
    {
        int wrapped_x = x & 255;
        int wrapped_y = y & 255;
        int index = wrapped_x + wrapped_y * 256;
        filled[index] = f;
    }


    public List<Vector3> FindPathTo(Vector3 start, Vector3 end, int radius)
    {
        List<Vector3> points = new();

        Dictionary<Vector3, Vector3> arrows = new Dictionary<Vector3, Vector3>();

        List<Vector3> set_pos = new List<Vector3>();

        Vector3 start_point = new Vector3(Mathf.Floor(start.x), Mathf.Floor(start.y), Mathf.Floor(start.z));
        set_pos.Add(start_point);

        bool TestPoint(Vector3 point)
        {
            if (Vector3.Distance(point, start) > radius)
            {
                return false;
            }

            int x = (int)Mathf.Floor(point.x);
            int y = (int)Mathf.Floor(point.y);
            int z = (int)Mathf.Floor(point.z);

            if (IsFilled(x, z))
            {
                return false;
            }

            if (arrows.ContainsKey(point))
            {
                return false;
            }

            return true;
        }

        Vector3 end_point = new Vector3(Mathf.Floor(end.x), 0, Mathf.Floor(end.z));
        while (set_pos.Count > 0)
        {
            var point = set_pos[0];
            set_pos.RemoveAt(0);

            if (point == end_point)
            {
                Vector3 next_point = point;
                for (int i = 0; i < radius * radius * 4; i++)
                {
                    if (next_point == start_point) break;
                    points.Add(next_point);

                    Vector3 dir;
                    arrows.TryGetValue(next_point, out dir);
                    next_point -= dir;
                }
                

                break;
            }


            if (TestPoint(point + Vector3.left))
            {
                arrows.Add(point + Vector3.left, Vector3.left);
                set_pos.Add(point + Vector3.left);
            }

            if (TestPoint(point + Vector3.right))
            {
                arrows.Add(point + Vector3.right, Vector3.right);
                set_pos.Add(point + Vector3.right);
            }

            if (TestPoint(point + Vector3.forward))
            {
                arrows.Add(point + Vector3.forward, Vector3.forward);
                set_pos.Add(point + Vector3.forward);
            }

            if (TestPoint(point + Vector3.back))
            {
                arrows.Add(point + Vector3.back, Vector3.back);
                set_pos.Add(point + Vector3.back);
            }
        }

        points.Reverse();

        return points;
    }

}
