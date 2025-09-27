using UnityEngine;

public abstract class Tutorial
{
    public TutorialManager manager;
    private Vector3 upPos = Vector3.up * 200f;

    public Tutorial(TutorialManager manager)
    {
        this.manager = manager;
    }

    protected void ChangeFollowFingerPosition(Vector3 pos)
    {
        manager.followFingerImage.SetActive(true);
        manager.followFingerImage.transform.position = pos + upPos;
    }
    public abstract void Play();
    public abstract void Update();
    public abstract void Clear();
}
