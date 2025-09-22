using System.Collections;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;

public class FrInverseKinematic : MonoBehaviour
{
    private readonly string AttackAniamtion_ID = "mixamo_com";
    private readonly int Ani_AttackId = Animator.StringToHash("Attack");

    public Transform rotationPos;
    public Transform followPos;
    public Transform netPoint;
    private Animator animator;
    private Coroutine co;
    void Awake()
    {
        animator = GetComponent<Animator>();
        //0.074 ,-0.006,-0.045
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger(Ani_AttackId);
        }
        netPoint.transform.position = followPos.transform.position;
        netPoint.transform.rotation = rotationPos.rotation;
    }
}
