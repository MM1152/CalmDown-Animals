using UnityEngine;

public class ControlCloud : MonoBehaviour
{
    public Cloudes cloud;
    private void OnEnable()
    {
        if(Variable.onCloud)
        {
            cloud?.gameObject.SetActive(true);
        }
    }
    private void OnDisable()
    {
        if(cloud != null)
        {
            cloud?.gameObject.SetActive(false);
        }
    }
}
