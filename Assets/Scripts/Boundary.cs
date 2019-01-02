using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    //when game object is out of the boundary then destory it
    private void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
