using UnityEngine;

public class WallController : MonoBehaviour
{
    const int DEFAULT_HP = 4;

    public Sprite dmgSprite;
    public int hp = DEFAULT_HP;
    public AudioClip chopSound1;
    public AudioClip chopSound2;

    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
