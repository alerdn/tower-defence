using System.Collections.Generic;
using Unity.AI.Navigation;
using Cinemachine;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private List<MapObject> _mapObjects;

    public void Generate(Sprite map)
    {
        Color[] pixels = map.texture.GetPixels();
        MapObject[,] mapMatrix = new MapObject[map.texture.width, map.texture.height];

        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixel = pixels[i];

            MapObject obj = _mapObjects.Find(obj => obj.Color == pixel);
            if (!obj) continue;

            int x = i % map.texture.width;
            int y = i / map.texture.width;

            MapObject objInstance = Instantiate(obj, transform);
            objInstance.transform.position = new Vector3(x, 0, y);
            mapMatrix[x, y] = objInstance;
        }

        int mapWidth = mapMatrix.GetLength(0);
        int mapHeight = mapMatrix.GetLength(1);

        int middleX = mapWidth / 2;
        int middleY = mapHeight / 2;

        Transform middleObject = mapMatrix[middleX, middleY].transform;
        float initialFov = Mathf.Clamp(mapWidth * 2, 30, 60);
        
        PlayerController.Instance.Init(middleObject.position, initialFov);

        _navMeshSurface.BuildNavMesh();
    }
}