using UnityEngine;

public class FrInverseKinematic : MonoBehaviour
{
    private readonly string AttackAniamtion_ID = "mixamo_com";


    public Transform rotationPos;
    public Transform followPos;
    public Transform netPoint;
    private Animator animator;
    private Coroutine co;

    public Transform boxTrap;

    public Transform gunLeftHand;
    public Transform gunRightHand;
    //1.31 , 0.258 , -0.41
    private Crew crew;
    void Awake()
    {
        animator = GetComponent<Animator>();
        crew = GetComponent<Crew>();

        netPoint.transform.position = followPos.transform.position;
        netPoint.transform.rotation = rotationPos.rotation;
    }

    private void Update()
    {
        if(crew.weapon.GetWeaponId() == 0)
        {
            netPoint.transform.position = followPos.transform.position;
            netPoint.transform.rotation = rotationPos.rotation;
        }

        if(crew.weapon.GetWeaponId() == 1) 
        {
            netPoint.position = followPos.position;
        }

        if (crew.weapon.GetWeaponId() == 2)
        {
            netPoint.transform.position = followPos.transform.position;
        }
    }
    //0.251 , 0.262 , 0.226
    //0.385 ,  0.529 ,  0.176
    private void OnAnimatorIK(int layerIndex)
    {
        if(crew.weapon.GetWeaponId() == 1)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, boxTrap.position);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, boxTrap.position);
        }
        else if(crew.weapon.GetWeaponId() == 2)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);

            animator.SetIKPosition(AvatarIKGoal.RightHand , gunRightHand.position);
            animator.SetIKPosition(AvatarIKGoal.LeftHand , gunLeftHand.position);
        }
    }
}
