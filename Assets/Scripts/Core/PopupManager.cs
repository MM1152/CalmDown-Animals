using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public GenericPopup[] popups;
    private Stack<GenericPopup> openPopups = new Stack<GenericPopup>();

    Coroutine co;
    private bool isOpening;

    private void Start()
    {
        foreach(var popup in popups)
        {
            popup.Init(this);
            popup.Close();
        }
    }

    public void Update()
    {
        if(TouchManager.touchType == TouchType.Tab)
        {
            Close();
        }

        //#if UNITY_EDITOR
        //        else if (Input.GetMouseButtonDown(0))
        //        {
        //            Close();
        //        }
        //#endif
    }


    public GenericPopup Open(Popup id)
    {
        if (co != null)
        {
            return popups[(int)id];
        }
        co = StartCoroutine(OpenCo(id));
        return popups[(int)id];
    }

    public void Close()
    {
        if (openPopups.Count > 0)
        {
            var popup = openPopups.Peek();
            if(popup.Close())
            {
                openPopups.Pop();
            }
        }
    }

    private IEnumerator OpenCo(Popup id)
    {
        if (openPopups.Contains(popups[(int)id]))
            yield break;

        Debug.Log("Open");
        popups[(int)id].Open();
        yield return null;
        Debug.Log("Push Queue");
        openPopups.Push(popups[(int)id]);
        
        co = null;
    }
}