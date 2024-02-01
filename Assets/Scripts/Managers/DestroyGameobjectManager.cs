using UnityEngine;

public class DestroyGameobjectManager : MonoBehaviour
{
    private readonly string[] tagsToDestroy = { "Animal", "PickAble" };
    public float destroyLimitY = -100f;

    void Update()
    {
        foreach (string tag in tagsToDestroy)
        {
            CheckAndDeleteObjectsBelowLimit(tag);
        }
    }

    void CheckAndDeleteObjectsBelowLimit(string objectTag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(objectTag);

        foreach (GameObject obj in objectsWithTag)
        {
            float objectY = obj.transform.position.y;

            if (objectY < destroyLimitY)
            {
                Destroy(obj);
            }
        }
    }
}