using UnityEngine;
public class LazyComponent<T> where T : Component
{
    public enum GetComponentMode
    {
        GetComponent,
        GetComponentInChildren,
        GetComponentInParent
    }

    private readonly Component _gameObject;
    private readonly GetComponentMode _getComponentMode;
    private T _component;

    public LazyComponent(Component gameObject, GetComponentMode getComponentMode = GetComponentMode.GetComponent)
    {
        _gameObject = gameObject;
        _getComponentMode = getComponentMode;
    }

    public T Component
    {
        get
        {
            if (_component == null)
            {
                switch (_getComponentMode)
                {
                    case GetComponentMode.GetComponent:
                        _component = _gameObject.GetComponent<T>();
                        break;
                    case GetComponentMode.GetComponentInChildren:
                        _component = _gameObject.GetComponentInChildren<T>();
                        break;
                    case GetComponentMode.GetComponentInParent:
                        _component = _gameObject.GetComponentInParent<T>();
                        break;
                }
            }
            return _component;
        }
    }
    
}
