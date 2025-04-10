﻿using UnityEngine;

namespace Crosstales.Common.Util
{
   /// <summary>Random rotation changer.</summary>
   public class RandomRotator : MonoBehaviour
   {
      #region Variables

      ///<summary>Use intervals to change the rotation (default: true).</summary>
      [Tooltip("Use intervals to change the rotation (default: true).")] public bool UseInterval = true;

      ///<summary>Random change interval between min (= x) and max (= y) in seconds (default: x = 10, y = 20).</summary>
      [Tooltip("Random change interval between min (= x) and max (= y) in seconds (default: x = 10, y = 20).")]
      public Vector2 ChangeInterval = new Vector2(10, 20);

      ///<summary>Minimum rotation speed per axis (default: 5 for all axis).</summary>
      [Tooltip("Minimum rotation speed per axis (default: 5 for all axis).")] public Vector3 SpeedMin = new Vector3(5, 5, 5);

      ///<summary>Maximum rotation speed per axis (default: 15 for all axis).</summary>
      [Tooltip("Minimum rotation speed per axis (default: 15 for all axis).")] public Vector3 SpeedMax = new Vector3(15, 15, 15);

      ///<summary>Set the object to a random rotation at Start (default: false).</summary>
      [Tooltip("Set the object to a random rotation at Start (default: false).")] public bool RandomRotationAtStart;

      private Transform tf;
      private Vector3 speed;
      private float elapsedTime;
      private float changeTime;

      #endregion


      #region MonoBehaviour methods

      private void Start()
      {
         tf = transform;

         elapsedTime = changeTime = Random.Range(ChangeInterval.x, ChangeInterval.y);

         if (RandomRotationAtStart)
            tf.localRotation = Random.rotation;
      }

      private void Update()
      {
         if (UseInterval)
         {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > changeTime)
            {
               elapsedTime = 0f;

               speed.x = Random.Range(Mathf.Abs(SpeedMin.x), Mathf.Abs(SpeedMax.x)) * (Random.Range(0, 2) == 0 ? 1 : -1);
               speed.y = Random.Range(Mathf.Abs(SpeedMin.y), Mathf.Abs(SpeedMax.y)) * (Random.Range(0, 2) == 0 ? 1 : -1);
               speed.z = Random.Range(Mathf.Abs(SpeedMin.z), Mathf.Abs(SpeedMax.z)) * (Random.Range(0, 2) == 0 ? 1 : -1);
               changeTime = Random.Range(ChangeInterval.x, ChangeInterval.y);
            }

            tf.Rotate(speed.x * Time.deltaTime, speed.y * Time.deltaTime, speed.z * Time.deltaTime);
         }
      }

      #endregion
   }
}
// © 2015-2020 crosstales LLC (https://www.crosstales.com)