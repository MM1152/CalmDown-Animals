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
    //0.25 , 0.55 , 0.226 boxTrap rot.z = 90
    //0.074 ,-0.006,-0.045 Net

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

    }
}
