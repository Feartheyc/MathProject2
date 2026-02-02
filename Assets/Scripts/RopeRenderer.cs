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

    void LateUpdate()
    {
        if (rope == null) return;

        int count = rope.transform.childCount + 2;
        lr.positionCount = count;

        // Start anchor
        lr.SetPosition(0, rope.startAnchor.position);

        // Rope segments
        for (int i = 0; i < rope.transform.childCount; i++)
        {
            lr.SetPosition(i + 1, rope.transform.GetChild(i).position);
        }

        // End body (number)
        lr.SetPosition(count - 1, rope.endBody.position);
    }
}
