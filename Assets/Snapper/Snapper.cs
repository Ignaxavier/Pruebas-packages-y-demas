using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Snapper : MonoBehaviour
{
    public      float                   _rayDistance;
    public      float                   _positionOffset;
    public      LayerMask               _layerMask;
    public      Transform               _objectTouched;
    public      dirRay                  _DirectionOfTheRay;
    public      bool                    _onoff;

    public      enum                    dirRay {X, Y, Z};
    private     RaycastHit              hit;
    private     Vector3                 rayVector;
    private     float                   rayVectorMultiplyer;

    void Update()
    {
        if (_onoff)
        {
            VectorRay();
            Touch();
            Ray();
        }
        else
        {
            _objectTouched = null;
        }
    }


    private void VectorRay()
    {
        rayVectorMultiplyer = Mathf.Clamp(_rayDistance, -1f, 1f);

        switch (_DirectionOfTheRay)
        {
            case dirRay.X:

                rayVector = Vector3.right * rayVectorMultiplyer;

                break;

            case dirRay.Y:

                rayVector = Vector3.up * rayVectorMultiplyer;

                break;

            case dirRay.Z:

                rayVector = Vector3.forward * rayVectorMultiplyer;

                break;
        }
    }

    private void Touch()
    {
        if(_objectTouched == null)
        {
            _objectTouched = hit.transform;
        }
    }
    
    private void Ray()
    {
        Debug.DrawRay(this.transform.position, rayVector, Color.red);

        if (Physics.Raycast(this.transform.position, rayVector, out hit, _rayDistance, _layerMask))
        {

            Debug.Log("Toque");

            switch (_DirectionOfTheRay)
            {
                case dirRay.X:

                    transform.position = new Vector3(_objectTouched.position.x + _positionOffset, transform.position.y, transform.position.z);

                    break;

                case dirRay.Y:

                    transform.position = new Vector3(transform.position.x, _objectTouched.position.y + _positionOffset, transform.position.z);

                    break;

                case dirRay.Z:

                    transform.position = new Vector3(transform.position.x, transform.position.y, _objectTouched.position.z + _positionOffset);

                    break;
            }
        }
    }
}
