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
    //0.25 , 0.55 , 0.226 boxTrap rot.z = 90
    //0.074 ,-0.006,-0.045 Net
    //-0.036, ,0.22 ,0.021 , -90 , 0 , 90
    private Crew crew;
    void Awake()
    {
        animator = GetComponent<Animator>();
        crew = GetComponent<Crew>();
    }

    private void Update()
    {
        if(crew.weapon.GetWeaponId() == 0)
        {
            netPoint.transform.position = followPos.transform.position;
            netPoint.transform.rotation = rotationPos.rotation;
        }

        if(crew.weapon.GetWeaponId() == 2)
        {
            netPoint.transform.position = followPos.transform.position;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(crew.weapon.GetWeaponId() == 1)
        {
            netPoint.position = animator.GetIKPosition(AvatarIKGoal.RightHand);
            //animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            //animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            //animator.SetIKPosition(AvatarIKGoal.RightHand, boxTrap.position);
            //animator.SetIKPosition(AvatarIKGoal.LeftHand, boxTrap.position);
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
