using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPositioner : MonoBehaviour
{
    void Start()
    {
        Pose spawnPose = FindObjectOfType<LevelManager>().LevelSpawnPosition;
        this.gameObject.transform.SetPositionAndRotation(spawnPose.position, spawnPose.rotation);
    }
}
