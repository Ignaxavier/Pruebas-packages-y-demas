using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Snapper : MonoBehaviour
{
    public      float                   _rayDistance;
    public      float                   _positionOffset;
    public      LayerMask               _layerMask;
    public      dirRay                  _DirectionOfTheRay;
    public      bool                    _onoff = false;

    public      enum                    dirRay {X, Y, Z};
    private     RaycastHit              hit;

    void Update()
    {
        if (_onoff)
        {
            Ray();
        }
    }

    private void Ray()
    {
        Debug.DrawLine(transform.position, -Vector3.up, Color.yellow);

        if(Physics.Raycast(transform.position, -Vector3.up, out hit, _rayDistance, _layerMask, QueryTriggerInteraction.Ignore))
        {
            transform.position = new Vector3(transform.position.x, hit.transform.position.y + _positionOffset, transform.position.z);
        }
    }
}
