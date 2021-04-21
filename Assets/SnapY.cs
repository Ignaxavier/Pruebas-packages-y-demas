using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapY : MonoBehaviour
{
    [SerializeField]
    private     float       _rayDistance;
    [SerializeField]
    private     float       _positionYOffset;
    [SerializeField]
    private     LayerMask   _layerMask;

    void Update()
    {
        RaycastHit hit;
    
        Debug.DrawRay(transform.position, new Vector3 (0, - _rayDistance, 0) , Color.red);

        if (Physics.Raycast (transform.position, -Vector3.up, out hit, _rayDistance, _layerMask))
        {
            transform.position = new Vector3(transform.position.x, hit.transform.position.y + _positionYOffset, transform.position.z);
        }
    }
}
