using UnityEngine;

public class ExplosionFade : MonoBehaviour
{
    [SerializeField] private float fadeTime;
    private SpriteRenderer sprite;
    private float startTime;

    private void Start()
    {
        startTime = Time.time;
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        float time = (Time.time - startTime) / fadeTime;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.SmoothStep(1, 0, time));
        if (sprite.color.a == 0)
        {
            Destroy(gameObject);
        }
    }
}
