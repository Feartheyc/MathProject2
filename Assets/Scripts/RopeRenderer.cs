using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeRenderer : MonoBehaviour
{
    public Rope rope;

    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Start()
{
    lr = GetComponent<LineRenderer>();
    lr.positionCount = 0; // clears old ghost points
}


    void LateUpdate()
{
    if (rope == null || rope.startAnchor == null || rope.endBody == null)
        return;

    // Get all rope segments
    int count = rope.transform.childCount;
    if (count == 0) return;

    // Collect positions
    Transform[] segments = new Transform[count];
    for (int i = 0; i < count; i++)
        segments[i] = rope.transform.GetChild(i);

    // SORT segments by distance from anchor (TOP → BOTTOM)
    System.Array.Sort(segments, (a, b) =>
        Vector2.Distance(rope.startAnchor.position, a.position)
        .CompareTo(
        Vector2.Distance(rope.startAnchor.position, b.position))
    );

    // LineRenderer points
    lr.positionCount = count + 2;

    // Start anchor
    lr.SetPosition(0, rope.startAnchor.position);

    // Rope body
    for (int i = 0; i < count; i++)
        lr.SetPosition(i + 1, segments[i].position);

    // End (number)
    lr.SetPosition(lr.positionCount - 1, rope.endBody.position);
}

}
