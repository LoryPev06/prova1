using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnhideThings : MonoBehaviour{
    public GameObject parent;
    public void Unhide(int id){
        for (int i = 0; i < parent.transform.childCount; i++){
            GameObject childObject = parent.transform.GetChild(i).gameObject;

            if (childObject.CompareTag(id.ToString())){
                childObject.SetActive(false);
                break;
            }
        }

    }
}
