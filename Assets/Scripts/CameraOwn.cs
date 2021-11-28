using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOwn : MonoBehaviour
{
    public GameObject playerObj;
    public SubPlayer subPlayer;

    private Player player;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        player = playerObj.GetComponent<Player>();

        offset = transform.position - playerObj.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var subPlayerObj = player.isSubPlayerAlive();
        if (subPlayerObj == null)
        {
            //offset = transform.position - playerObj.transform.position;

            transform.position = playerObj.transform.position + offset;
        }
        else
        {
            transform.position = subPlayerObj.transform.position + offset;
        }
    }
}
