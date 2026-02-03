using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeRenderer : MonoBehaviour
{
    public Rope rope;
    LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void LateUpdate()
    {
        if (rope == null) return;

        int count = rope.segments.Count;

        if (count == 0)
        {
            lr.positionCount = 0;
            return;
        }

        lr.positionCount = count + 1;

        for (int i = 0; i < count; i++)
        {
            lr.SetPosition(i, rope.segments[i].transform.position);
        }

        lr.SetPosition(count, rope.endBody.position);
    }
}
