using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager instance;
    public List<PopUp> poolList = new List<PopUp>();
    public GameObject BlurUI_Prefab;
    UIBlur m_UiBlur;
    private bool m_WithBlur;
    private bool GameInStartMode = false;
    private void Awake()
    {
        instance = this;
  
    }


  
    public void ShowQueue(UnityAction qeueuMessage) {
        if (poolList.Count > 0) {

            EnqueueAction(qeueuMessage);    
        }
        else
        {

            qeueuMessage?.Invoke();
        }

    }
    public T Show<T>(GameObject prefab, Transform parent = null, bool withBlur = true, float FadeMoveY_Start = 50,float FadeMoveY_End = 0, float FadeSpeed = 0.2f,  params GameObject[] skipBlureList)
    {
        Transform parentObject;
        if (parent == null) parentObject = this.transform;
        else { parentObject = parent; }
        m_WithBlur = withBlur;


        if (m_WithBlur)
        {
            SkipBlureList(skipBlureList, false);
            ShowBlur(parentObject);
            SkipBlureList(skipBlureList, true);
        }

        T popUp = Instantiate(prefab, parentObject).GetComponent<T>();      
        RectTransform viewRect = (popUp as PopUp).GetComponent<RectTransform>();
        CanvasGroup MainInfoGroup = (popUp as PopUp).GetComponent<CanvasGroup>();

        Sequence s = DOTween.Sequence();
        s.SetId(popUp).SetUpdate(true);
        s.Join(viewRect?.DOAnchorPosY(FadeMoveY_End, FadeSpeed).From(Vector2.one * FadeMoveY_Start).SetEase(Ease.OutBack));
        s.Join(MainInfoGroup?.DOFade(1, FadeSpeed));        //All prefabs should implement interface WindowUI


        (popUp as PopUp).Show(() => Hide(popUp as PopUp,FadeMoveY_Start,FadeSpeed));
        (popUp as PopUp).OnCompleteBase = () => Hide(popUp as PopUp, FadeMoveY_Start, FadeSpeed);
        (popUp as PopUp).blureUsed = withBlur;

        AddToPool(popUp as PopUp);
        return (popUp); 
    }
    public T ShowSimple<T>(GameObject prefab, Transform parent = null, float FadeInSpeed = 0.2f, float FadeOutSpeed = 0.2f)
    {
        Transform parentObject;
        if (parent == null) parentObject = this.transform;
        else { parentObject = parent; }



        T popUp = Instantiate(prefab, parentObject).GetComponent<T>();
        RectTransform viewRect = (popUp as PopUp).GetComponent<RectTransform>();
        CanvasGroup MainInfoGroup = (popUp as PopUp).GetComponent<CanvasGroup>();


        Sequence s = DOTween.Sequence();
        s.SetId(popUp).SetUpdate(true);
        s.Join(viewRect?.DOAnchorPosY(0, 0));
        s.Join(MainInfoGroup?.DOFade(1, FadeInSpeed));
        
        //All prefabs should implement interface WindowUI
        (popUp as PopUp).Show(() => FastHide(popUp as PopUp, FadeOutSpeed));
        (popUp as PopUp).OnCompleteBase = () => FastHide(popUp as PopUp, FadeOutSpeed);
        (popUp as PopUp).blureUsed = false;


        AddToPool(popUp as PopUp);
        return (popUp);
    }


 
    public void SkipBlureList(GameObject[] skipBlureList, bool enable)
    {

        if (skipBlureList == null) return;

        foreach (GameObject item in skipBlureList)
        {
            item.SetActive(enable);
        }

    }
    public void ShowBlur(Transform parent = null, bool isCustom = false, float Delay = 0f)
    {
        Transform parentObject;
        if (parent == null) parentObject = this.transform;
        else { parentObject = parent; }

        
        if (m_UiBlur != null)
        {
            m_UiBlur.transform.SetParent(parentObject);
            if (isCustom) m_UiBlur.transform.SetAsFirstSibling(); else m_UiBlur.transform.SetAsLastSibling();
            m_UiBlur.Initialize(Delay);
          
        }
        else
        {

            m_UiBlur = Instantiate(BlurUI_Prefab, parentObject).GetComponent<UIBlur>();
            if (isCustom) m_UiBlur.transform.SetAsFirstSibling(); else m_UiBlur.transform.SetAsLastSibling();
            m_UiBlur.Initialize(Delay);
         
        }


    }
    public void ClearBlur()
    {
        if (m_UiBlur != null) m_UiBlur.ShowBluredImage(false);
    }



    public void CloseAllPopUps()
    {
        for (int i = 0; i < poolList.Count; i++)
        {
            if (poolList[i] != null)
                FastHide(poolList[i]);
        }

    }
    public void Hide(PopUp view, float FadeMoveYPos = 50, float FadeSpeed = 0.1f)
    {
        if (view == null) return;

        if(view.blureUsed) ClearBlur();
        RectTransform viewRect = (view).GetComponent<RectTransform>();
        CanvasGroup MainInfoGroup = (view).GetComponent<CanvasGroup>();



        view?.GetComponentInChildren<UIBlur>()?.gameObject.SetActive(false);
        Sequence s = DOTween.Sequence();
        s.SetId(view).SetUpdate(true);
        s.Join( MainInfoGroup?.DOFade(0, FadeSpeed));
        s.Join( viewRect?.DOAnchorPosY(-FadeMoveYPos, FadeSpeed).SetEase(Ease.InFlash));
        s.OnComplete(() => { DestroyOnComplete(view); 
        });
    }
    public void FastHide(PopUp view,float fadeOutSpeed = 0)
    {
        if (view == null) return;
        if (view.blureUsed) ClearBlur();
        CanvasGroup MainInfoGroup = (view).GetComponent<CanvasGroup>();
       
        Sequence s = DOTween.Sequence();
        s.SetId(view).SetUpdate(true);
        s.Join(MainInfoGroup?.DOFade(0, fadeOutSpeed));
        s.OnComplete(() => {
            DestroyOnComplete(view);
        });
       

      

    }
    public void DestroyOnComplete(PopUp view)
    {
        if(view == null) return;
        DOTween.Kill(view);
        string name = view.name;
        Destroy(view.gameObject);

    }


    //Pooling Functions
    public void AddToPool(PopUp view)
    {
        poolList.Add(view);
    }
    public void RemoveFromPoolSelf(PopUp view ) {
        poolList.Remove(view);

        //RemoveFromPool(view);
        if (poolList.Count == 0)
        {

            DequeueNext();
            BackButtonReset();

        }


    }

    public void CloseLastPopUp() {
        if(poolList==null) return;

        if (poolList.Count > 0)
        {
            (poolList[poolList.Count - 1])?.OnCompleteBase();
            
        }
        
    }
    public void BackButtonReset() {

       
    }




    #region QUEUE MANAGEMENT
    private Queue<UnityAction> m_MainQueue = new Queue<UnityAction>();
    public int QueueCount=0;


    public void EnqueueAction(UnityAction action)
    {
        m_MainQueue.Enqueue(action);
        QueueCount = m_MainQueue.Count;
    }

    public void DequeueNext()
    {
        UnityAction queuedAction = m_MainQueue.Count > 0 ? m_MainQueue.Dequeue() : null;
        queuedAction?.Invoke();
        QueueCount = m_MainQueue.Count;
    }
    #endregion
}
