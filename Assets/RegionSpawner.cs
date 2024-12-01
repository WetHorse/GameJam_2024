using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RegionSpawner : MonoBehaviour
{
    [SerializeField] private int framesLeft;
    [SerializeField] private int iterationLimitPerFrame = 10;
    [SerializeField] private Transform origin;
    [SerializeField] private List<Region> regions;
    [SerializeField] private float sizeX, sizeY, frameDistanceY;
    [SerializeField] private int preSpawnCount = 3;

    private Vector3 currentFrameOrigin;


    private void Start()
    {
        framesLeft = regions.Sum((r) => r.FrameCount);
        currentFrameOrigin = PreSpawnFrames(origin.position,preSpawnCount);
        StartCoroutine(Spawn());
    }

    [System.Serializable]
    public class Region
    {
        public int FrameCount;
        public float MinimalDistance;
        public int ObstaclesPerFrame;
        public List<Obstacle> Obstacles;
        public UnityEvent onRegionEnded;
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
        yield return new WaitUntil(()=> currentFrameOrigin.y + (sizeY/2) <= origin.transform.position.y);
        WinManager.Instance.GameWon();
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
        Region currentRegion = GetCurrentRegion();

        // Calculate screen bounds in world space
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        float screenHeight = mainCamera.orthographicSize * 2f; // Height of the screen in world space
        float screenWidth = screenHeight * mainCamera.aspect;  // Width of the screen in world space

        float minX = frameCenter.x - (screenWidth / 2) + (sizeX / 2); // Adjust for obstacle size
        float maxX = frameCenter.x + (screenWidth / 2) - (sizeX / 2);
        float minY = frameCenter.y - (screenHeight / 2) + (sizeY / 2);
        float maxY = frameCenter.y + (screenHeight / 2) - (sizeY / 2);

        List<Vector3> placedObstacles = new List<Vector3>();
        int spawnedObstacles = 0;
        int iteration = 0;

        while (currentRegion  != null && spawnedObstacles < currentRegion.ObstaclesPerFrame && iteration < iterationLimitPerFrame)
        {
            iteration++;

            if (currentRegion.Obstacles.Count > 0)
            {
                Obstacle randomObstacle = currentRegion.Obstacles[Random.Range(0, currentRegion.Obstacles.Count)];
                Vector3 obstacleSize = randomObstacle.transform.localScale;

                // Adjust spawn area to ensure at least 50% of the obstacle is visible
                float candidateMinX = minX + obstacleSize.x / 4; // Half of half (50%)
                float candidateMaxX = maxX - obstacleSize.x / 4;
                float candidateMinY = minY + obstacleSize.y / 4;
                float candidateMaxY = maxY - obstacleSize.y / 4;

                Vector3 candidatePosition = new Vector3(
                    Random.Range(candidateMinX, candidateMaxX),
                    Random.Range(candidateMinY, candidateMaxY),
                    frameCenter.z
                );

                bool validPosition = true;
               /* foreach (var placed in placedObstacles)
                {
                    if (Vector3.Distance(placed, candidatePosition) < currentRegion.MinimalDistance)
                    {
                        validPosition = false;
                        break;
                    }
                }*/

                if (validPosition)
                {
                    placedObstacles.Add(candidatePosition);

                    var spawnedObject = Instantiate(randomObstacle, candidatePosition, Quaternion.identity);
                    currentRegion.FrameCount--;
                    spawnedObstacles++;
                }
            }
        }

        if (currentRegion.FrameCount == 0)
        {
            currentRegion.onRegionEnded?.Invoke();
        }

        DrawBox(frameCenter, new Vector3(screenWidth, screenHeight), Color.red);
    }




    public void DrawBox(Vector3 center, Vector3 size, Color color)
    {
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
        Gizmos.color = Color.red;
        Vector3 center = origin.position;
        Vector3 size = new Vector3(sizeX, sizeY);
        Vector3 halfSize = size / 2f;
        Vector3 topLeft = center + new Vector3(-halfSize.x, halfSize.y, 0);
        Vector3 topRight = center + new Vector3(halfSize.x, halfSize.y, 0);
        Vector3 bottomLeft = center + new Vector3(-halfSize.x, -halfSize.y, 0);
        Vector3 bottomRight = center + new Vector3(halfSize.x, -halfSize.y, 0);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
