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

    public void PlaceObject(GameObject obj)
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out MapObject mapObject))
            {
                if (mapObject.IsStackable)
                {
                    mapObject.Stack(obj);
                }
            }
        }

    }
}
