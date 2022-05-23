using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
   private float speed;
   private Vector3 dir;
   private float timeToDesTroy;
 
   private void Start()
   {
      //speed = Data.fireSpeed;
      dir = transform.parent.forward;
      Destroy(gameObject, 2);
   }

   public void SetData(float speed, Vector3 dir, float time)
   {
      this.speed = speed;
      this.dir = dir;
      timeToDesTroy = time;

   }

   public Move()
   {
      this.speed = this.speed;
   }

  

   private void Update()
   {
      
      transform.Translate(dir * speed * Time.deltaTime, Space.World);
   }
}
