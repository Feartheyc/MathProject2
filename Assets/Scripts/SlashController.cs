using UnityEngine;
using UnityEngine.InputSystem;

public class SlashController : MonoBehaviour
{
    public LayerMask ropeLayer;

    private Vector2 lastPos;
    private bool isSlashing;

    void Update()
{
    if (Pointer.current == null) return;

    if (Pointer.current.press.wasPressedThisFrame)
    {
        Vector3 p = Pointer.current.position.ReadValue();
        p.z = Mathf.Abs(Camera.main.transform.position.z);
        lastPos = Camera.main.ScreenToWorldPoint(p);
        isSlashing = true;
    }
    else if (Pointer.current.press.isPressed && isSlashing)
    {
        Vector3 p = Pointer.current.position.ReadValue();
        p.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector2 currentPos = Camera.main.ScreenToWorldPoint(p);

        Debug.DrawLine(lastPos, currentPos, Color.red, 0.1f);

        RaycastHit2D hit = Physics2D.Linecast(lastPos, currentPos, ropeLayer);

        if (hit.collider != null)
        {
            Debug.Log("HIT ROPE");
            Rigidbody2D rb = hit.collider.attachedRigidbody;
            if (rb != null)
            {
                Rope rope = hit.collider.GetComponentInParent<Rope>();
                if (rope != null)
                    rope.CutAt(rb);
            }
        }

        lastPos = currentPos;
    }
    else if (Pointer.current.press.wasReleasedThisFrame)
    {
        isSlashing = false;
    }
}

}
