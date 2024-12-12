using UnityEngine;

public class MapObject : MonoBehaviour
{
    public Color Color => _color;
    public bool IsStackable => _isStackable;

    [ColorHtmlProperty]
    [SerializeField] private Color _color;
    [SerializeField] private bool _isStackable;

    public void Stack(GameObject obj)
    {
        Instantiate(obj, transform.position + Vector3.up, Quaternion.identity, transform);
        _isStackable = false;
    }
}