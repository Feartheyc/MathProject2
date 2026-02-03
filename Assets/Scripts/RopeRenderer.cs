using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class RopeRenderer : MonoBehaviour
{
    public Rope rope;
    LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();

        // ADDITIVE: ensure clean state
        lr.useWorldSpace = true;
        lr.positionCount = 0;
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

        // ADDITIVE: sort segments top → bottom to avoid zig-zag
        List<Transform> sortedSegments = new List<Transform>();
        for (int i = 0; i < count; i++)
            sortedSegments.Add(rope.segments[i].transform);

        Transform anchor = rope.startAnchor != null ? rope.startAnchor.transform : sortedSegments[0];


        sortedSegments.Sort((a, b) =>
            Vector2.Distance(anchor.position, a.position)
            .CompareTo(Vector2.Distance(anchor.position, b.position))
        );

        // ADDITIVE: +2 for anchor + end body
        lr.positionCount = sortedSegments.Count + 2;

        int index = 0;

        // ADDITIVE: draw from anchor
        lr.SetPosition(index++, anchor.position);

        // ORIGINAL LOGIC (unchanged, but ordered)
        for (int i = 0; i < sortedSegments.Count; i++)
        {
            lr.SetPosition(index++, sortedSegments[i].position);
        }

        // ORIGINAL
        lr.SetPosition(index, rope.endBody.position);

        // ORIGINAL CALLS (now actually implemented)
        DrawRope();
        UpdateTextureTiling();
    }

    // ================= ADDITIVE METHODS =================

    void DrawRope()
    {
        // Intentionally empty for now
        // Keeps compatibility with your existing calls
    }

    void UpdateTextureTiling()
    {
        if (lr.positionCount < 2) return;

        float ropeLength = 0f;

        for (int i = 0; i < lr.positionCount - 1; i++)
        {
            ropeLength += Vector2.Distance(
                lr.GetPosition(i),
                lr.GetPosition(i + 1)
            );
        }

        // Texture repeats along rope
        lr.material.mainTextureScale = new Vector2(ropeLength, 1f);
    }
}
