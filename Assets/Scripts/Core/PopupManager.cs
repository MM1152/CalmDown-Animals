using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public GenericPopup[] popups;
    private Stack<GenericPopup> openPopups = new Stack<GenericPopup>();

    Coroutine co;

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
        if(TouchManager.TouchType == TouchType.Tab)
        {
            Close();
        }
    }

    public void Open(Popup id)
    {
        if(co != null)
        {
            StopCoroutine(co);
        }
        co = StartCoroutine(OpenCo(id));
    }

    public void Close()
    {
        if (openPopups.Count > 0)
        {
            var popup = openPopups.Pop();
            popup.Close();
        }
    }

    private IEnumerator OpenCo(Popup id)
    {
        yield return null;
        openPopups.Push(popups[(int)id]);
        popups[(int)id].Open();
        co = null;
    }
}