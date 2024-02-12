using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using System;


public class GameManager : MonoBehaviour{
    public Camera mainCamera;
    public GameObject[] gameObjects;
    public GameObject currentGameObject;
    public Text storyText;
    public Text text;
    public int score;
    private Vector3 worldPosition;
    private Vector3 pos;
    private bool canS = true;
    public GameObject parent;
    public int lastFind;
    public int maxFind = 1;

    private void Start(){
        worldPosition.z = -1f;
        worldPosition.y = 3.5f;
        lastFind = maxFind;
        spawnNew();
    }
    private void Update(){
        if(lastFind != maxFind){
            FindAnyObjectByType<UnhideThings>().Unhide(maxFind);
            // cose che darà il nuovo coso in base a cosa abbiamo trovato
            lastFind = maxFind;
        }
        text.text = score.ToString();
        if (currentGameObject != null){
            if (Touchscreen.current.primaryTouch.press.isPressed){
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

                worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
                worldPosition.z = -1f;
                worldPosition.y = 3.5f;
                currentGameObject.transform.position = worldPosition;
            }
        }else{
            Invoke(nameof(spawnNew), .5f);
        }

        if(Touchscreen.current.primaryTouch.press.wasReleasedThisFrame && canS){
            if(currentGameObject != null){
                currentGameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1f);
                currentGameObject.GetComponent<Colli>().canHit = true;
                currentGameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                currentGameObject = null;
                canS = false;
                Invoke(nameof(resetClick), 1);
                Invoke(nameof(spawnNew), .5f);
            }
        }

    }
    private void spawnNew(){
        if(currentGameObject == null){
            currentGameObject = Instantiate(chooseGameObject(), worldPosition, Quaternion.identity);
            currentGameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            currentGameObject.transform.parent = parent.transform;
        }
        CancelInvoke(nameof(spawnNew));
    }
    private GameObject chooseGameObject(){
        float rand = UnityEngine.Random.value;
        int maxIterations = maxFind < gameObjects.Length / 2 ? maxFind : gameObjects.Length / 2;
        for(int i = 1; i < maxIterations; i++){
            float probability = 1.0f / Mathf.Pow(2.0f, i);
            if (rand > probability){
                return gameObjects[i];
            }
        }
        return gameObjects[0];

    }
    private void resetClick(){
        canS = true;
    }
    public void addScore(int id){
        score += (int)Mathf.Pow(id * 3, 1.5f);
    }
    public void setMax(int tag){
        if(tag > maxFind){
            maxFind = tag;
        }
    }
    public void Init(){
        worldPosition.x = 0f;
        score = 0;
        maxFind = 0;
        foreach(Transform child in parent.transform){
            Destroy(child.gameObject);
        }
        spawnNew();
    }
    public void lose(){

    }
}
