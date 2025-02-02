using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : CharacterObject
{
    [SerializeField] private Camera playerCam;
    public Camera PlayerCam => playerCam;

    private Vector3 prevPos;

    public override void Init(Data data)
    {
        base.Init(data);

        var player = data as Player;
        player.Init(this);

        prevPos = Input.mousePosition;
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;

        if (!GameManager.Instance.IsPause)
        {
            transform.Rotate(0, (mousePos.x-prevPos.x)*0.1f, 0);
            PlayerCam.transform.Rotate((prevPos.y-mousePos.y)*0.1f, 0, 0);

            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * Time.deltaTime, Space.Self);
            }

            // 조준
            if (Input.GetMouseButton(1))
            {
                if (GunObject != null)
                {
                    var gun = GunObject.data as GunInfo;

                    GunNode.transform.localPosition = Vector3.Lerp(GunNode.transform.localPosition, gun.GetAimPos(), Time.deltaTime*10);
                    GunNode.transform.localRotation = Quaternion.Slerp(GunNode.transform.localRotation, gun.GetAimRot(), Time.deltaTime*10);
                }
            }
            else
            {
                if (GunObject != null)
                {
                    var gun = GunObject.data as GunInfo;

                    GunNode.transform.localPosition = Vector3.Lerp(GunNode.transform.localPosition, gun.GetNoAimPos(), Time.deltaTime*10);
                    GunNode.transform.localRotation = Quaternion.Slerp(GunNode.transform.localRotation, gun.GetNoAimRot(), Time.deltaTime*10);
                }
            }
        }
        
        prevPos = mousePos;        
    }
}
