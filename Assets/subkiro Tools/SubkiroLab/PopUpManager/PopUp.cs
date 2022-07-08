
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public  abstract class PopUp : MonoBehaviour
{
    public UnityAction OnCompleteBase;
    public bool blureUsed = false;
    public abstract void Show(UnityAction OnComplete);

    public void RemoveFromPool()
    {
        
        PopUpManager.instance.RemoveFromPoolSelf(this);
    }

   

    public virtual void OnDestroy()
    {
        RemoveFromPool();
    }

    
}
