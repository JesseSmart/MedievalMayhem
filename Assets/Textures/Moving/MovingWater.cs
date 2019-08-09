using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MovingWater : NetworkBehaviour
{

    [SyncVar]
    public float offset;

    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            RpcMaterial(offset);

        }
        //rend.material.SetTextureOffset("_MainTex", new Vector2(Mathf.Sin(offset), 0));
    }

    [ClientRpc] //speed of material is determined in BoatStats script on Ego Boat prefab in scene
    void RpcMaterial(float offVal)
    {
        rend.material.SetTextureOffset("_MainTex", new Vector2(Mathf.Sin(offVal), 0));

    }
}
