using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Rope : MonoBehaviour
{
    public Rigidbody2D startAnchor;
    public Rigidbody2D endBody;
    public RopeSegment segmentPrefab;
    public int segmentCount = 15;
    public float segmentLength = 0.1f;

    public List<RopeSegment> segments = new List<RopeSegment>();

    void Start()
    {
        BuildRope();
    }

    void BuildRope()
    {
        Rigidbody2D previous = startAnchor;

        for (int i = 0; i < segmentCount; i++)
        {
            RopeSegment seg = Instantiate(
            segmentPrefab,
            startAnchor.position + Vector2.down * segmentLength * (i + 1),
            Quaternion.identity,
            transform
            );

            DistanceJoint2D joint = seg.GetComponent<DistanceJoint2D>();
            joint.connectedBody = previous;
            joint.distance = segmentLength;
            joint.enableCollision = false;

            previous = seg.GetComponent<Rigidbody2D>();
            segments.Add(seg);
        }

        DistanceJoint2D endJoint = endBody.gameObject.AddComponent<DistanceJoint2D>();
        endJoint.connectedBody = previous;
        endJoint.distance = segmentLength;
    }


    // ================================
    // CUT MECHANIC
    // ================================
    public void CutAt(Rigidbody2D hitBody)
{
    int cutIndex = segments.FindIndex(
        s => s.GetComponent<Rigidbody2D>() == hitBody
    );

    if (cutIndex == -1) return;

    // 🔥 Upper rope vanishes smoothly
    for (int i = 0; i < cutIndex; i++)
    {
        StartCoroutine(
            VanishAndDestroy(segments[i].gameObject)
        );
    }

    segments.RemoveRange(0, cutIndex);

    // ✂️ Detach remaining rope from anchor
    DistanceJoint2D firstJoint =
        segments[0].GetComponent<DistanceJoint2D>();

    if (firstJoint != null)
        Destroy(firstJoint);
}


    IEnumerator VanishAndDestroy(GameObject obj)
{
    float duration = 0.5f;
    float t = 0f;

    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
    Vector3 startScale = obj.transform.localScale;

    while (t < duration)
    {
        t += Time.deltaTime;
        float k = 1f - (t / duration);

        // Shrink
        obj.transform.localScale = startScale * k;

        // Fade
        if (sr != null)
        {
            Color c = sr.color;
            c.a = k;
            sr.color = c;
        }

        yield return null;
    }

    Destroy(obj);
}

}
