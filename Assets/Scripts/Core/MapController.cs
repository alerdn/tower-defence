using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MapController : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private List<MapObject> _mapObjects;

    [Header("Animation")]
    [SerializeField] private float _spawnScaleDuration = .1f;
    [SerializeField] private float _spawnScaleInterval = .05f;
    [SerializeField] private bool _showGenerateAnimation;

    public IEnumerator Generate(Sprite map)
    {
        Color[] pixels = map.texture.GetPixels();
        InitPlayer(map);

        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixel = pixels[i];

            MapObject obj = _mapObjects.Find(obj => obj.Color == pixel);
            if (!obj) continue;

            int x = i % map.texture.width;
            int y = i / map.texture.width;

            MapObject objInstance = Instantiate(obj, transform);
            objInstance.transform.position = new Vector3(x, 0, y);

            objInstance.transform.DOScale(1f, _spawnScaleDuration).From(0f);
            if (_showGenerateAnimation) yield return new WaitForSeconds(_spawnScaleInterval);
        }

        yield return new WaitForSeconds(.5f);
        _navMeshSurface.BuildNavMesh();
    }

    private void InitPlayer(Sprite map)
    {
        int mapWidth = map.texture.width;
        int mapHeight = map.texture.height;

        int middleX = mapWidth / 2;
        int middleY = mapHeight / 2;

        Vector3 middlePosition = new Vector3(middleX, 0f, middleY);
        float initialFov = Mathf.Clamp(mapWidth * 2, 30, 60);

        PlayerController.Instance.Init(middlePosition, initialFov);
    }
}