using UnityEngine;
using System.Collections;

public class NumberPlatform : MonoBehaviour
{
    public int numberValue;
    public ParticleSystem collectFX;

    public AudioClip coinSFX;

    [Header("Pop Effect")]
    public float popScale = 1.4f;
    public float popTime = 0.12f;

    bool used;
    SpriteRenderer sr;
    Collider2D col;
    EndlessPuzzleManager puzzleManager;
    Vector3 originalScale;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        puzzleManager = FindObjectOfType<EndlessPuzzleManager>();
        originalScale = transform.localScale; // ✅ FIX: lossyScale can break animations
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;
        if (!other.CompareTag("Player")) return;

        Debug.Log("Coin triggered");

        if (SFXManager.Instance == null)
        {
            Debug.LogError("SFXManager INSTANCE IS NULL");
        }
        else
        {
            SFXManager.Instance.PlaySFX(coinSFX, 0.7f);
        }

        used = true;
        col.enabled = false;

        // ✅ KEEP EXISTING LOGIC
        puzzleManager.OnNumberCollected(numberValue);

        StartCoroutine(PopFadeAndDestroy());
    }

    IEnumerator PopFadeAndDestroy()
    {
        // 🔹 POP UP
        yield return ScaleTo(originalScale * popScale, popTime);

        // 🔹 POP DOWN
        yield return ScaleTo(originalScale * 0.6f, popTime * 0.8f);

        // 🔹 PARTICLE (FIXED)
        if (collectFX)
        {
            ParticleSystem fx = Instantiate(
                collectFX,
                transform.position,
                Quaternion.identity
            );

            // ✅ ADD: force world space so it doesn't move with object
            var main = fx.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            fx.Play(); // ✅ ADD: explicit play

            // ✅ FIX: destroy AFTER full lifetime
            float lifetime = main.duration + main.startLifetime.constantMax;
            Destroy(fx.gameObject, lifetime);

            Debug.Log("Collect FX played");
        }

        // 🔹 FADE OUT
        yield return FadeOut();

        sr.enabled = false;
        Destroy(gameObject);
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        Color c = sr.color;

        while (t < 0.25f)
        {
            t += Time.deltaTime;
            sr.color = new Color(c.r, c.g, c.b, 1 - t / 0.25f);
            yield return null;
        }
    }

    IEnumerator ScaleTo(Vector3 target, float time)
    {
        Vector3 start = transform.localScale;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, target, t / time);
            yield return null;
        }

        transform.localScale = target;
    }
}