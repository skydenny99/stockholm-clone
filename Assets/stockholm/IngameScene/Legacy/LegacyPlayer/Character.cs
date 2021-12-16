using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour, ILegacyCreature
{

	public GameObject player;
	public InputManager _input;
	public CharacterAnimator _anim;
	public GameObject arrow;
	public Backpack backPack;

	public Vector2 hitboxSize;
	public Vector2 hitboxOffset;

	public float speed = 3f;
	public float jumpPower = 2f;
	public float rollSpeed = 4f;
	public bool isAlive = true;

	[SerializeField]
	private float maxHp_ = 100f;
	public float maxHp
	{
		get
		{
			return maxHp_;
		}
		set
		{
			maxHp_ = value;
			hp_ = Mathf.Clamp(hp_, 0, maxHp_);
			if(uiModule_ != null)
			{
				uiModule_.ChangePlayerMaxHPValue((int)maxHp_);
				uiModule_.ChangePlayerHPValue((int)hp_);
			}
		}
	}

	[SerializeField]
	private float hp_ = 100f;
	public float hp
	{
		get
		{
			return hp_;
		}
		set
		{
			if(!isAlive)
				return;

			hp_ = Mathf.Clamp(value, 0, maxHp_);
			if (uiModule_ != null)
			{
				uiModule_.ChangePlayerHPValue((int)hp_);
			}
			if(hp_ == 0)
			{
				die();
			}
		}
	}

	public float godTime = 1f;
	public bool isGod = false;
	private float godTimer = 0f;


	public bool canAttack = true;
	public float attackInterval = 0.3f;
	public float attackSpeed = 1f;
	private float attackTimer = 0f;
    
    public bool canClimb = false;
    public bool isClimbing = false;
    public float climbSpeed = 2f;
    public bool stopClimbing = false;
    public bool isGoingUp = true;
    private GameObject currentLadder;                            

	public bool canJump = true;
	public bool isGrounded = true;

	public bool canRoll = true;
	public float rollInterval = 0.1f;
	private float rollTimer = 0f;

	public bool isInteracting = true;

	public float HealingPotionAmound = 1f;

	private Vector3 _lookDirection;
	private Rigidbody2D _rig;
	private Vector2 _currentKnockback;
	private Transform _transform;
	private IInteractable InteractingTarget_;
	private IngameUIModule uiModule_;

	void Awake()
	{
		//backPack.player = this;
	}

	// Use this for initialization
	void Start()
	{
		//Todo : 타이머 시스템 변경해야한다고 한다
		rollTimer = rollInterval;
		attackTimer = attackInterval;
		Debug.Assert(player);
		_lookDirection = transform.root.localScale;
		Debug.Assert(_input);
		Debug.Assert(_anim);
		_rig = transform.root.GetComponent<Rigidbody2D>();
		_transform = transform;
		hitboxSize = player.GetComponent<BoxCollider2D>().size;
		hitboxOffset = player.GetComponent<BoxCollider2D>().offset;
		uiModule_ = ModuleManager.GetInstance().GetModule<IngameUIModule>();
	}

	// Update is called once per frame
	void Update()
	{
		if (!isAlive)
			return;

		if (!_anim.isAnimating(CharacterAnimator.CurrentPlayerAnimate.RollingOver))
		{
			if (!_anim.isAnimating(CharacterAnimator.CurrentPlayerAnimate.Attacking))
			{
				attackTimer += Time.deltaTime;
			}
			BoxCollider2D b = player.GetComponent<BoxCollider2D>();
			if (b.size.y < hitboxSize.y)
			{
				b.offset = hitboxOffset;
				b.size = hitboxSize;
			}
			rollTimer += Time.deltaTime;
		}

		if (rollTimer > rollInterval)
		{
			canRoll = true;
			rollTimer = Mathf.Clamp(rollTimer, 0, 2 * rollInterval);
		}

		if (attackTimer > attackInterval)
		{
			canAttack = true;
			attackTimer = Mathf.Clamp(attackTimer, 0, 2 * attackInterval);
		}

		if (godTimer < godTime)
		{
			godTimer += Time.deltaTime;
		}
		else if (isGod)
			isGod = false;

        if (isClimbing)
        {
            if (stopClimbing)
            {
                _rig.velocity = Vector2.zero;
                _anim.setStopClimbing(0);
            }
            else
            {
                _anim.setStopClimbing(1);
            }
            _rig.gravityScale = 0;
        }
        else
        {
            _rig.gravityScale = 2;
        }

		if (isInteracting)
		{
			if (!canInteract)
			{
				InteractingTarget_.Interact(InteractMessage.ExitNPC, null);
				InteractingTarget_ = null;
				isInteracting = false;
			}
		}
	}



	/// <summary>
	/// Creature interface 구현
	/// </summary>
	/// 
	public void move(float axis)
	{
		if (!isAlive)
			return;

		if (axis > 0)
			transform.parent.localScale = new Vector3(-_lookDirection.x, _lookDirection.y, _lookDirection.z);
		else if (axis < 0)
			transform.parent.localScale = _lookDirection;

		_rig.velocity = new Vector2(speed * axis, _rig.velocity.y);
	}

	public void attack(bool isMelee)
	{
		if (!isAlive)
			return;

		canAttack = false;
		attackTimer = 0;
		if (isMelee)
		{
		}
		else
		{
		}
	}

	/// <summary>
	/// TODO : 넉백 방향과 애니메이션 방향 불일치 해결
	/// </summary>
	/// <param name="t"></param>
	/// <param name="dmg"></param>
	/// <param name="knockBack"></param>
	public void getDamaged(Transform t, float dmg, float knockBack)
	{
		if (!isAlive)
			return;

		if (isGod)
			return;
		else
			isGod = true;
		godTimer = 0;
		hp -= dmg;
		playKnockBack(t, knockBack);
	}
	public void knockback()
	{
		if (!isAlive)
			return;

		_rig.velocity = _currentKnockback;
	}

	public void playKnockBack(Transform t, float knockBack)
	{
		if (!isAlive)
			return;

		float dist = t.position.x - transform.root.position.x;
		if (transform.root.localScale.x * dist > 0)
		{
			_anim.triggerAnimate(CharacterAnimator.PlayerAnimateTrigger.GetBackHit);
		}
		else
		{
			_anim.triggerAnimate(CharacterAnimator.PlayerAnimateTrigger.GetFrontHit);
		}
		_currentKnockback = (Vector2.left * dist).normalized * knockBack;
	}


	public void jump()
	{
		if (!isAlive)
			return;

		_rig.velocity = new Vector2(_rig.velocity.x, jumpPower);
	}

	public void rollOver()
	{
		if (!isAlive)
			return;

		rollTimer = 0;
		canRoll = false;
		_rig.velocity = new Vector2(-transform.parent.localScale.x * rollSpeed, _rig.velocity.y);
	}
	public void drinkPotion()
	{
		if (!isAlive)
			return;

		backPack.UseConsumeItem("item_healing_01", 1);
	}

	public void interact()
	{
		if (!isAlive)
			return;

		var targetObject = interactableTarget.Select(target => target.GetComponent<IInteractable>()).Where(target => target != null).FirstOrDefault();
		if (targetObject == null || targetObject.Equals(InteractingTarget_))
			return;

		targetObject.Interact(InteractMessage.EnterNPC, null);
		InteractingTarget_ = targetObject;
		isInteracting = true;
	}

	public bool canInteract
	{
		get
		{
			return interactableTarget.Length > 0;
		}
	}

	public Collider2D[] interactableTarget
	{
		get
		{
			return Physics2D.OverlapAreaAll(new Vector2(_transform.position.x - hitboxSize.x / 2, _transform.position.y - hitboxSize.y / 2), new Vector2(_transform.position.x + hitboxSize.x / 2, _transform.position.y + hitboxSize.y / 2)).Where(col => col.CompareTag("NPC")).ToArray();
		}
	}

	public void die()
	{
		isAlive = false;
		_anim.triggerAnimate(CharacterAnimator.PlayerAnimateTrigger.Die);
		ModuleManager.GetInstance().GetModule<GameModule>().GameOver("죽었습니다.");
    }

    
    /// <summary>
    /// TODO : 사다리 타기 구현
    /// </summary>
    /// <param name="b"></param>
    public void climb(bool b)
    {
        if (b)
        {
            _rig.velocity = new Vector2(0, climbSpeed);
        }
        else
        {
            _rig.velocity = new Vector2(0, -climbSpeed);
        }
    }

    public bool checkClimb()
    {
        Collider2D[] cols = Physics2D.OverlapAreaAll(new Vector2(player.transform.position.x + hitboxOffset.x - hitboxSize.x, player.transform.position.y + hitboxOffset.y + hitboxSize.y), new Vector2(player.transform.position.x + hitboxOffset.x + hitboxSize.x, player.transform.position.y + hitboxOffset.y - hitboxSize.y));
        //        Debug.DrawLine(new Vector2(player.transform.position.x + hitboxOffset.x - hitboxSize.x, player.transform.position.y + hitboxOffset.y + hitboxSize.y), new Vector2(player.transform.position.x + hitboxOffset.x + hitboxSize.x, player.transform.position.y + hitboxOffset.y - hitboxSize.y), Color.blue);
        bool check = false;
        foreach(Collider2D col in cols)
        {
            if (col.tag.Equals("Ladder"))
            {
                currentLadder = col.transform.gameObject;
                transform.root.position = new Vector3(currentLadder.transform.position.x, transform.root.position.y, transform.root.position.z);
                _rig.velocity = Vector2.zero;
                check = true;
            }
        }
        return check;
    }

    /// <summary>
    /// 아래에는 전부 게터 세터들입니다.
    /// </summary>
    /// <returns></returns>
    

    public bool getIsClimbing()
    {
        return isClimbing;
    }

    public void setIsClimbing(bool climb)
    {
        isClimbing = climb;
    }

    public bool getIsGrounded()
    {
        return isGrounded;
    }

    public void setIsGrounded(bool ground)
    {
        isGrounded = ground;
    }

    public bool getCanClimb()
    {
        return canClimb;
    }

    public void setCanClimb(bool climb)
    {
        canClimb= climb;
    }

    public bool getCanJump()
    {
        return canJump;
    }

    public void setCanJump(bool jump)
    {
        canJump = jump;
    }
}
