using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    public bool TryPlaceObject(GameObject obj, Vector2 mousePositionValue)
    {
        Ray ray = _camera.ScreenPointToRay(mousePositionValue);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out MapObject mapObject))
            {
                if (mapObject.IsStackable)
                {
                    mapObject.Stack(obj);
                    return true;
                }
            }
        }

        return false;
    }
}
