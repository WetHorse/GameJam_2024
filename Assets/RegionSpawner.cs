using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RegionSpawner : MonoBehaviour
{
    [SerializeField] private int framesLeft;
    [SerializeField] private int iterationLimitPerFrame = 10;
    [SerializeField] private Transform origin;
    [SerializeField] private float spawnOffsetY = 10f;
    [SerializeField] private float frameSpawnRange = 1f;

    [SerializeField] private List<Region> regions;
    [SerializeField] private float sizeX, sizeY, frameDistanceY;
    [SerializeField] private int preSpawnCount = 3;

    private Vector3 currentFrameOrigin;
  

    private void Start()
    {
        currentFrameOrigin = PreSpawnFrames(origin.position,preSpawnCount);
        framesLeft = regions.Sum((r)=>r.FrameCount);
        StartCoroutine(Spawn());
    }

    [System.Serializable]
    public struct Region
    {
        public int FrameCount;
        public float MinimalDistance;
        public int ObstaclesPerFrame;
        public List<Obstacle> Obstacles;
    }
    
    private IEnumerator Spawn ()
    {
        while (framesLeft > 0)
        {
            if (currentFrameOrigin.y - sizeY - frameDistanceY <= origin.transform.position.y)
            {   
                SpawnFrame(currentFrameOrigin);
                currentFrameOrigin += new Vector3(0,sizeY + frameDistanceY,0);
            }
            yield return null;
        }
    }

    private Region GetCurrentRegion() => regions.FirstOrDefault((r)=>r.FrameCount > 0);


    private Vector3 PreSpawnFrames(Vector3 origin,int count)
    {
        Vector3 frameOffset = new Vector3(0, sizeY + frameDistanceY, 0);
        Vector3 currentOrigin = origin + frameOffset * 2;
        for (int i = 0; i < count; i++)
        {
            SpawnFrame(currentOrigin);
            currentOrigin += frameOffset;
        }
        return currentOrigin;
    }

    private void SpawnFrame(Vector3 frameCenter)
    {
        framesLeft--;
        Region currentRegion = GetCurrentRegion();
        currentRegion.FrameCount--;

        float minX = frameCenter.x - (sizeX / 2);
        float maxX = frameCenter.x + (sizeX / 2);
        float minY = frameCenter.y - (sizeY / 2);
        float maxY = frameCenter.y + (sizeY / 2);

        List<Vector3> placedObstacles = new List<Vector3>();
        int spawnedObstacles = 0;
        int iteration = 0;

        while (spawnedObstacles < currentRegion.ObstaclesPerFrame && iteration < iterationLimitPerFrame)
        {
            iteration++;

            if (currentRegion.Obstacles.Count > 0)
            {
                Obstacle randomObstacle = currentRegion.Obstacles[Random.Range(0, currentRegion.Obstacles.Count)];
                Vector3 obstacleSize = randomObstacle.GetComponent<Renderer>().bounds.size;

                float candidateMinX = minX + obstacleSize.x / 2;
                float candidateMaxX = maxX - obstacleSize.x / 2;
                float candidateMinY = minY + obstacleSize.y / 2;
                float candidateMaxY = maxY - obstacleSize.y / 2;

                Vector3 candidatePosition = new Vector3(
                    Random.Range(candidateMinX, candidateMaxX),
                    Random.Range(candidateMinY, candidateMaxY),
                    frameCenter.z
                );

                bool validPosition = true;
                foreach (var placed in placedObstacles)
                {
                    if (Vector3.Distance(placed, candidatePosition) < currentRegion.MinimalDistance)
                    {
                        validPosition = false;
                        break;
                    }
                }

                if (validPosition)
                {
                    placedObstacles.Add(candidatePosition);

                    Instantiate(randomObstacle, candidatePosition, Quaternion.identity, transform);
                    spawnedObstacles++;
                }
            }
        }

        DrawBox(frameCenter, new Vector3(sizeX, sizeY), Color.red);
    }



    public void DrawBox(Vector3 center, Vector3 size, Color color)
    {
        Gizmos.color = color;

        Vector3 halfSize = size / 2f;
        Vector3 topLeft = center + new Vector3(-halfSize.x, halfSize.y, 0);
        Vector3 topRight = center + new Vector3(halfSize.x, halfSize.y, 0);
        Vector3 bottomLeft = center + new Vector3(-halfSize.x, -halfSize.y, 0);
        Vector3 bottomRight = center + new Vector3(halfSize.x, -halfSize.y, 0);
        Debug.DrawLine(topLeft, topRight,color,5f);
        Debug.DrawLine(topRight, bottomRight, color, 5f);
        Debug.DrawLine(bottomRight, bottomLeft, color, 5f);
        Debug.DrawLine(bottomLeft, topLeft, color, 5f); 
    }



    private void OnDrawGizmos()
    {
        if (origin == null) return;
        Gizmos.color = Color.green;
       // Gizmos.DrawCube(transform.position + new Vector3(0,spawnOffsetY,0),new Vector3(sizeX,sizeY));
    }
}
