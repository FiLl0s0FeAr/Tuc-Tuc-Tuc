using UnityEngine;

public class XOSpawnerScript : MonoBehaviour
{
    // X prefab
    public GameObject xObject;

    // O prefab
    public GameObject oObject;
    public void SpawnX(float coordinateX, float coordinateY, float coordinateZ = 0)
    {
        // spawn X prefab on the correspondent coorditates
        Instantiate(xObject, new Vector3(coordinateX, coordinateY, coordinateZ), transform.rotation);
    }

    public void SpawnO(float coordinateX, float coordinateY, float coordinateZ = 0)
    {
        // spawn O prefab on the correspondent coorditates
        Instantiate(oObject, new Vector3(coordinateX, coordinateY, coordinateZ), transform.rotation);
    }
}
