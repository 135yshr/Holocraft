using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockItemBehaviour : MonoBehaviour, IMinecraftEventHandler
{
    public GameObject blockPrefab;

    public void Received(McData data)
    {
        if (data == null)
        {
            Debug.Log("data is null");
        }
        var msg = string.Format("action: {0}, {1}:{2}:{3}", data.action, data.pos.x, data.pos.y, data.pos.z);
        Debug.Log(msg);
        Instantiate(blockPrefab,
            new Vector3(data.pos.x, data.pos.y, data.pos.z),
            Quaternion.Euler(0, 0, 0));
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //var headPosition = Camera.main.transform.position;
        //var gazeDirection = Camera.main.transform.forward;

        //RaycastHit hitInfo;
        //if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        //{
        //    Debug.Log("position: " + hitInfo.point);
        //    //blockPrefab.transform.localScale = new Vector3(hitInfo.point.x, hitInfo.point.y + 1, hitInfo.point.z);
        //    Instantiate(blockPrefab,
        //        new Vector3(hitInfo.point.x, hitInfo.point.y + 1, hitInfo.point.z),
        //        Quaternion.Euler(0, 0, 0));
        //}
    }
}
