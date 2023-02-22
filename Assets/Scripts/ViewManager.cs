using System.Collections.Generic;
using UnityEngine;

public interface IViewOperater
{
    void Show();
    void Hide();
    void Update();
}

public class ViewManager : MonoBehaviourSingletonTemplate<ViewManager>
{
    private IViewOperater m_lastPopedView;
    private IViewOperater m_currentView;
    private Stack<IViewOperater> m_viewStack;
    private Transform m_rootCanvas;

    public IViewOperater GetCurrentView() 
    {
        return m_currentView;
    }

    public IViewOperater GetLastPopedView()
    {
        return m_lastPopedView;
    }

    public Transform GetRootTransform()
    {
        return m_rootCanvas;
    }

    void Start()
    {
        m_viewStack = new Stack<IViewOperater>();
        m_rootCanvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        var rootView = new HomeView(m_rootCanvas);
        Push(rootView);
    }

    void Update()
    {
        m_currentView.Update();
    }

    public void Push(IViewOperater view)
    {
        m_viewStack.Push(view);
        m_currentView = view;
        m_currentView.Show();
    }

    public void Pop()
    {
        m_lastPopedView = m_viewStack.Pop();
        m_currentView = m_viewStack.Peek();
        if (m_currentView != null) {
            m_currentView.Show();
        }
    }
}
