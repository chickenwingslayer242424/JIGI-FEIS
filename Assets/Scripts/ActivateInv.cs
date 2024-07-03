using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateInv : MonoBehaviour
{
public GameObject ActiveInvObject;
public void ActivateObject()
{
    if (ActiveInvObject.activeSelf != true)
   {
    ActiveInvObject.SetActive(true);
   } 
   else
   {
    ActiveInvObject.SetActive(false);
   }

}
}
