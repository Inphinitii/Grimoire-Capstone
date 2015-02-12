using UnityEngine;
using System.Collections.Generic;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Animation Controler
 *
 * Description: Dictates the current animation state of 
 * the actor.
 *
 * Also will provide functionality to swap out Mecanim motions
 * based on the ComponentCollection attached to this particular
 * actor.
 *
 * AnimatorOverrideController <---- Use this to override
 * the animations used by the AnimationController in Mecanim.
 * We're going to need a separate controller per character.
 =========================================================*/

public class AnimationController : MonoBehaviour
{
    Animator m_Animator;
    PhysicsController m_physicsController;
    ActionController m_actionController;
    MovementController m_movementController;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_physicsController = transform.gameObject.GetComponentInParent<PhysicsController>();
        m_actionController = transform.gameObject.GetComponentInParent<ActionController>();
        m_movementController = transform.gameObject.GetComponentInParent<MovementController>();
    }

    void Update()
    {
        WalkingAnimations();
        JumpingAnimations();
        AttackAnimations();
    }

    void WalkingAnimations()
    {
        m_Animator.SetBool("Moving", m_movementController.m_isMoving);
        m_Animator.SetFloat("MovementSpeed", Mathf.Abs(m_physicsController.Velocity.x));
    }

    void JumpingAnimations()
    {
        if (m_physicsController.Velocity.y > 0)
        {
            m_Animator.SetBool("Jumping", true);
            m_Animator.SetBool("Falling", false);
        }
        else if (m_physicsController.Velocity.y < 0)
        {
            m_Animator.SetBool("Jumping", false);
            m_Animator.SetBool("Falling", true);
        }
        else if (m_physicsController.Velocity.y == 0)
            m_Animator.SetBool("Falling", false);


    }

    void AttackAnimations() {
        m_Animator.SetBool("Casting", m_actionController.m_smashAttack);
        m_Animator.SetFloat("CastingDuration", m_actionController.m_chargeDuration);
        m_Animator.SetFloat("AttackDuration", m_actionController.m_attackDuration);

    }

}