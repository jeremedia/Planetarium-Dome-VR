using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VR = UnityEngine.VR;

public class CameraAppear : MonoBehaviour
{
    // http://answers.unity3d.com/questions/1349152/vr-popup-menu-in-front-of-camera-in-fixed-position.html

    // http://stackoverflow.com/questions/26423549/how-to-modify-recttransform-properties-in-script-unity-4-6-beta

    // Camera Orientation - https://forums.oculus.com/developer/discussion/26763/best-practice-for-camera-rotation-offset-in-unity-5
    // Very important - https://forum.unity.com/threads/different-content-in-each-eye.332575/

    public Camera playerCamera;
    public GameObject triggerObject;
    private Quaternion CameraHeadRotation;

    public GameObject itemObject;
 //   private RectTransform rectBox;
    private float distance = 2.0f;

    private void Start()
    {

    }

    // Use this for initialization
    void OnEnable()
    {
        //if (itemObject != null)
        //{
        //    CameraHeadRotation = VR.InputTracking.GetLocalRotation(VR.VRNode.Head);
            
        //    //       Debug.Log("X Rotation" + CameraHeadRotation.x);
        //    if ((CameraHeadRotation.x >= -0.2f) && (CameraHeadRotation.x <= 0.2f))
        //    {
        //        itemObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward * distance;
        //        itemObject.transform.rotation = new Quaternion(0.0f, playerCamera.transform.rotation.y, 0.0f, playerCamera.transform.rotation.w);

        //        Vector3 position = transform.position;
        //        position.y = 0;
        //        transform.position = position;
        //    }
        //    else
        //    {
        //   //     Debug.Log("NOT DOING ");
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
     //   CameraHeadRotation = VR.InputTracking.GetLocalRotation(VR.VRNode.LeftEye);
      // Debug.Log("X Rotation" + CameraHeadRotation.x);

        if (itemObject != null)
        {
       //     CameraHeadRotation = VR.InputTracking.GetLocalRotation(VR.VRNode.CenterEye); // * Quaternion.Euler(45, 0, 0);

       //     Debug.Log("X Rotation" + CameraHeadRotation.x);
       
            //            if ((CameraHeadRotation.x >= -0.03f) && (CameraHeadRotation.x <= 0.03f))


            //if ((CameraHeadRotation.x >= -0.1f) && (CameraHeadRotation.x <= 0.1f))
            //{
       //         Debug.Log("X Rotation" + CameraHeadRotation.x);

                if (ProjectManager.isExperienceStarted)
                {
                //    Debug.Log("DOING ");
                    itemObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward * distance;
                    //       itemObject.transform.rotation = new Quaternion(0.0f, playerCamera.transform.rotation.y, 0.0f, playerCamera.transform.rotation.w);
                    itemObject.transform.rotation = new Quaternion(playerCamera.transform.rotation.x, playerCamera.transform.rotation.y, playerCamera.transform.rotation.z, playerCamera.transform.rotation.w);
                }
                else
                {
                ////        Debug.Log("NOT DOING ");
                //if (ProjectManager.isVideoCompleted)
                //    {
                //        Vector3 position = transform.position;
                //        position.x = 0;
                //        position.y = 0;
                //        position.z = 2;
                //        transform.position = position;
                //    }
                }
            //}
        }

        //if (triggerObject != null)
        //{
        //    triggerObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward * distance;
        //    triggerObject.transform.rotation = new Quaternion(0.0f, playerCamera.transform.rotation.y, 0.0f, playerCamera.transform.rotation.w);
        //}
    }
}