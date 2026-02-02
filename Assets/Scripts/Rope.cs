using UnityEngine;
using System.Collections.Generic;

public class Rope : MonoBehaviour
{
    public Rigidbody2D startAnchor;
    public Rigidbody2D endBody;
    public RopeSegment segmentPrefab;
    public int segmentCount = 15;
    public float segmentLength = 0.1f;

    private List<RopeSegment> segments = new List<RopeSegment>();

    void Start()
    {
        BuildRope();
    }

    void BuildRope()
    {
        Rigidbody2D previous = startAnchor;

        for (int i = 0; i < segmentCount; i++)
        {
            RopeSegment segment = Instantiate(
                segmentPrefab,
                startAnchor.position + Vector2.down * segmentLength * (i + 1),
                Quaternion.identity,
                transform
            );

            SpringJoint2D joint = segment.GetComponent<SpringJoint2D>();
            joint.connectedBody = previous;
            joint.distance = segmentLength;

            previous = segment.GetComponent<Rigidbody2D>();
            segments.Add(segment);
        }

        DistanceJoint2D endJoint = endBody.gameObject.AddComponent<DistanceJoint2D>();
        endJoint.connectedBody = previous;
        endJoint.distance = segmentLength;
    }

    public void CutAt(Rigidbody2D hitBody)
{
    Debug.Log("CUT CALLED");

    // 1. Destroy joint on the hit segment
    SpringJoint2D segmentJoint = hitBody.GetComponent<SpringJoint2D>();
    if (segmentJoint != null)
    {
        Destroy(segmentJoint);
    }

    // 2. ALSO destroy joint connecting the number
    SpringJoint2D endJoint = endBody.GetComponent<SpringJoint2D>();
    if (endJoint != null)
    {
        Destroy(endJoint);
    }
}

}
