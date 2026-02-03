using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeRenderer : MonoBehaviour
{
    public Rope rope;

    // ADDITIVE
    [SerializeField] private Material ropeMaterial;

    LineRenderer lr;

    // ================================
    void Awake()
    {
        lr = GetComponent<LineRenderer>();

        // ADDITIVE: force correct LineRenderer state (Unity 6 safe)
        lr.useWorldSpace = true;
        lr.textureMode = LineTextureMode.Tile;
        lr.positionCount = 0;

        // ADDITIVE: force material (Inspector assignment alone is unreliable in Unity 6)
        if (ropeMaterial != null)
        {
            lr.material = ropeMaterial;
        }
    }

    // ================================
    void LateUpdate()
    {
        if (rope == null) return;

        int count = rope.segments.Count;

        if (count == 0)
        {
            lr.positionCount = 0;
            return;
        }

        // ORIGINAL LOGIC (unchanged)
        lr.positionCount = count + 1;

        for (int i = 0; i < count; i++)
        {
            lr.SetPosition(i, rope.segments[i].transform.position);
        }

        lr.SetPosition(count, rope.endBody.position);

        // ADDITIVE
        UpdateTextureTiling();
    }

    // ================= ADDITIVE ONLY =================

    void UpdateTextureTiling()
    {
        if (lr.positionCount < 2) return;

        float ropeLength = 0f;

        for (int i = 0; i < lr.positionCount - 1; i++)
        {
            ropeLength += Vector3.Distance(
                lr.GetPosition(i),
                lr.GetPosition(i + 1)
            );
        }

        // Controls how many times texture repeats along rope
        lr.material.mainTextureScale = new Vector2(ropeLength * 2f, 1f);
    }
}
