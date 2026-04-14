using UnityEngine;

public class HipFix : MonoBehaviour
{
    private Transform hips;

    void Start()
    {
        hips = transform.Find("mixamorig:Hips");
    }

    void LateUpdate()
    {
        if (hips != null)
        {
            hips.localPosition = new Vector3(0, hips.localPosition.y, 0);
        }
    }
}