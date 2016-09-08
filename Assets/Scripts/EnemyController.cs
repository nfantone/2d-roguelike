using UnityEngine;

public class EnemyController : MovingObject
{
    const string PLAYER_TAG = "Player";
    const string ENEMY_ATTACK_TRIGGER = "enemyAttack";
    public int playerDamage;
    public AudioClip enemyAttack1;
		public AudioClip enemyAttack2;

    Animator animator;
    Transform target;
    bool skipMove;

    // Use this for initialization
    protected override void Start()
    {
        GameManager.instance.RegisterEnemy(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int x, int y)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(x, y);
        skipMove = true;
    }

    public void MoveEnemy()
    {
        int x = 0;
        int y = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            y = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            x = target.position.x > transform.position.x ? 1 : -1;
        }

        AttemptMove<PlayerController>(x, y);
    }

    protected override void OnCantMove<T>(T component)
    {
        PlayerController hitPlayer = component as PlayerController;
        animator.SetTrigger(ENEMY_ATTACK_TRIGGER);
        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
        hitPlayer.LoseFood(playerDamage);
    }
}
