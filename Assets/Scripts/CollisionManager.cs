using UnityEngine;
using System.Collections.Generic;

public class CollisionManager : MonoBehaviour{
    public GameObject parent;
    private HashSet<int> handledCollisions = new HashSet<int>();

    public void HandleCollision(GameObject obj1, GameObject obj2, bool destroy){
        int collisionKey = obj1.GetInstanceID();
        int collisionKey1 = obj2.GetInstanceID();

        
        if (!handledCollisions.Contains(collisionKey) && !handledCollisions.Contains(collisionKey1)){
            if(obj1.GetInstanceID() < obj2.GetInstanceID()){
                handledCollisions.Add(collisionKey1);
                int id = int.Parse(obj1.tag);
                if(!destroy){
                    GameObject p = FindAnyObjectByType<GameManager>().gameObjects[id];
                    Vector3 meanPosition = (obj1.transform.position + obj2.transform.position) / 2;
                    p = Instantiate(p, meanPosition, Quaternion.identity);
                    p.GetComponent<Colli>().addForc();
                    p.GetComponent<Colli>().canHit = true;
                    FindAnyObjectByType<GameManager>().setMax(int.Parse(p.gameObject.tag));
                    p.transform.parent = parent.transform;
                }
                Destroy(obj1);
                Destroy(obj2);
                
                FindAnyObjectByType<GameManager>().addScore(id);
            }
        }
    }

}
