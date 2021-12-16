using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode downKey = KeyCode.DownArrow;

    public KeyCode meleeAttackKey = KeyCode.A;
    public KeyCode rangeAttackKey = KeyCode.S;
    public KeyCode jumpKey = KeyCode.D;
    public KeyCode rollKey = KeyCode.Space;
    public KeyCode interactKey = KeyCode.Space;

    public GameObject cAnimator;
    public GameObject cController;
    private CharacterAnimator _anim;
    private Character _control;

    private float xAxis = 0f;

    // Use this for initialization
    void Start()
    {
        Debug.Assert(cAnimator);
        Debug.Assert(cController);
        _anim = cAnimator.GetComponent<CharacterAnimator>();
        _control = cController.GetComponent<Character>();
        Debug.Assert(_anim);
        Debug.Assert(_control);
    }

    // Update is called once per frame
    void Update()
    {
        
        _anim.setGrounded(_control.getIsGrounded());

        if (_anim.isAnimating(CharacterAnimator.CurrentPlayerAnimate.Climbing))
        {
            if ((Input.GetKeyUp(upKey) || Input.GetKeyUp(downKey))||(!Input.GetKey(upKey) && !Input.GetKey(downKey)))
                _control.stopClimbing = true;
        }

        if (!_anim.isAnimating(CharacterAnimator.CurrentPlayerAnimate.GetHit))
        {
            if (!_anim.isAnimating(CharacterAnimator.CurrentPlayerAnimate.RollingOver))
            {
                if ((!Input.GetKey(leftKey) && !Input.GetKey(rightKey)) || (_control.getIsClimbing() && !_control.getIsGrounded()))
                    xAxis = 0;
                else
                {
                    if (_control.getIsClimbing())
                    {
                        _control.stopClimbing = false;
                        _control.setIsClimbing(false);
                        _anim.setClimbing(_control.getIsClimbing());
                        _anim.resetAllTrigger();
                    }
                    if (Input.GetKey(leftKey) && Input.GetKey(rightKey))
                    {
                        xAxis = 0;
                    }
                    else if (Input.GetKey(leftKey))
                    {
                        xAxis = -1;
                    }
                    else if (Input.GetKey(rightKey))
                    {
                        xAxis = 1;
                    }
                }
                xAxis = Mathf.Clamp(xAxis, -1, 1);
                _anim.setSpeed(Mathf.Abs(xAxis));

                //아래 키 누르시 작동
                if (Input.GetKey(downKey))
                {
                    if (!_control.checkClimb())
                    {
                        _control.setIsClimbing(false);
                        _anim.setClimbing(_control.getIsClimbing());
                        _anim.resetAllTrigger();
                    }
                    else
                    {
                        if (!_control.getIsClimbing())
                        {
                            _control.setIsClimbing(true);

                        }
                        _control.isGoingUp = false;
                        _control.stopClimbing = false;
                        _control.climb(false);
                        _anim.setClimbing(_control.getIsClimbing());
                    }


                }

                //위에 키 누를시 행동
                if (Input.GetKey(upKey))
                {
                    if (!_control.checkClimb())
                    {
                        _control.setIsClimbing(false);
                        _anim.setClimbing(_control.getIsClimbing());
                        _anim.resetAllTrigger();
                    }
                    else
                    {
                        if (!_control.getIsClimbing())
                        {
                            _control.setIsClimbing(true);
                        }
                        _control.isGoingUp = true;
                        _control.stopClimbing = false;
                        _control.climb(true);
                        _anim.setClimbing(_control.getIsClimbing());
                    }

                }

                if (Input.GetKeyDown(rollKey) && _control.canRoll)
                {
                    xAxis = 0;
                    _control.player.GetComponent<BoxCollider2D>().offset = new Vector2(_control.hitboxOffset.x, _control.hitboxOffset.y - _control.hitboxSize.y / 2);
                    _control.player.GetComponent<BoxCollider2D>().size = new Vector2(_control.hitboxSize.x, (_control.hitboxSize.y) / 2);
                    _anim.triggerAnimate(CharacterAnimator.PlayerAnimateTrigger.RollOver);
                }

                if (Input.GetKeyDown(meleeAttackKey) && _control.canAttack)
                {
                    _anim.anim.SetFloat("Attack Speed", _control.attackSpeed);
                    _anim.triggerAnimate(CharacterAnimator.PlayerAnimateTrigger.MeleeAttack);
                }

                if (Input.GetKeyDown(rangeAttackKey) && _control.canAttack)
                {
                    _anim.anim.SetFloat("Attack Speed", _control.attackSpeed);
                    _anim.triggerAnimate(CharacterAnimator.PlayerAnimateTrigger.RangeAttack);
                }

                if (Input.GetKeyDown(jumpKey))
                {
                    if (_control.getCanJump() || _control.getIsClimbing())
                    {
                        if (_control.getIsClimbing())
                        {
                            _control.setIsClimbing(false);
                            _anim.setClimbing(_control.getIsClimbing());
                            _anim.resetAllTrigger();
                        }
                        _control.setCanJump(false);
                        _control.jump();
                        _anim.triggerAnimate(CharacterAnimator.PlayerAnimateTrigger.Jump);
                    }
                }

            }


            if (Input.GetKeyDown(interactKey))
            {
                _control.interact();
                _anim.triggerAnimate(CharacterAnimator.PlayerAnimateTrigger.Interaction);
            }

        }


    }

    void FixedUpdate()
    {
        if (!_anim.isAnimating(CharacterAnimator.CurrentPlayerAnimate.GetHit))
        {
            if (!_anim.isAnimating(CharacterAnimator.CurrentPlayerAnimate.RollingOver))
            {
                _control.move(xAxis);
            }
            else
            {
                _control.rollOver();
            }
        }
        else
        {
            _control.knockback();
        }

        // }

    }


}
