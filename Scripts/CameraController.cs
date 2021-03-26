using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 basicPosition;
    private Vector3 basicRotaion;
    private bool whitePlayer;
    private bool verticalView;
    public bool isWhite()
    {
        return whitePlayer;
    }
    public void whiteTurn()
    {
        whitePlayer = true;
    }
    public void BlackTurn()
    {
        whitePlayer = false;
    }

    public void SwitchPointofView()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !verticalView)
        {
            transform.position = new Vector3(0, 9.5f, 0);
            transform.eulerAngles = new Vector3(90.0f, 0, 0);
            verticalView = true;
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift) && verticalView)
        {
            verticalView = false;
        }
    }
    public void HorizontalView()
    {
        Vector3 otherPlayerPosition = basicPosition;
        otherPlayerPosition.x = -1 * basicPosition.x;
        if (!whitePlayer)
        {
            transform.position = otherPlayerPosition;
            transform.eulerAngles = new Vector3(50, -90, 0);
        }
        else
        {
            transform.position = basicPosition;
            transform.eulerAngles = basicRotaion;
        }
    }

    void Start()
    {
        basicPosition = transform.position;
        basicRotaion = transform.eulerAngles;
        verticalView = false;
    }

    void FixedUpdate()
    {
        SwitchPointofView();
        if (!verticalView)
            HorizontalView();
    }
    private bool moving;
    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, float time)
    {
        moving = true; // MoveObject started
        float i = 0;
        float rate = 1 / time;
        while (i < 1)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            thisTransform.rotation = Quaternion.Slerp(startRot, endRot, i);
            yield return 0;
        }
        moving = false; // MoveObject ended
    }
    public void switchplayer()
    {
        verticalView = true;
        Quaternion rot = transform.rotation;
        rot.y *= -1;
        rot.z *= -1;
        Vector3 pos = transform.position;
        pos.x *= -1;
        StartCoroutine(MoveObject(transform, transform.position, pos, transform.rotation, rot, 1.0f));
    }
}
