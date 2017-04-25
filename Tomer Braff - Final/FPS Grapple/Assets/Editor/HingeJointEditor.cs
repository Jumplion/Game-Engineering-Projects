using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(HingeJoint))]
public class HingeJointEditor : Editor
{
  //public override void OnInspectorGUI()
  
  public override void OnInspectorGUI()
  {
    //DrawDefaultInspector();

    HingeJoint myTarget = (HingeJoint)target;

    myTarget.connectedBody = (Rigidbody)EditorGUILayout.ObjectField("Connected Body", myTarget.connectedBody, typeof(Rigidbody), true);

    myTarget.anchor = EditorGUILayout.Vector3Field("Anchor", myTarget.anchor);

    myTarget.axis = EditorGUILayout.Vector3Field("Axis", myTarget.axis);

    myTarget.autoConfigureConnectedAnchor = EditorGUILayout.Toggle("Auto Configure Connected Anchor", myTarget.autoConfigureConnectedAnchor);

    //using(var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myTarget.autoConfigureConnectedAnchor)))
    //  if (myTarget.autoConfigureConnectedAnchor == false)
        myTarget.connectedAnchor = EditorGUILayout.Vector3Field("Connected Anchor", myTarget.connectedAnchor);

    //myTarget.breakForce = EditorGUILayout.FloatField("Break Force", myTarget.breakForce);
    //myTarget.breakTorque = EditorGUILayout.FloatField("Break Torque", myTarget.breakForce);

    //myTarget.enableCollision = EditorGUILayout.Toggle("Enable Collision", myTarget.enableCollision);
    //myTarget.enablePreprocessing = EditorGUILayout.Toggle("Enable Preprocessing", myTarget.enablePreprocessing);

  }
}