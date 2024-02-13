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
    public GameObject story;
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
        SpawnNew();
        ShowStory(1);
    }
    private void Update(){
        if(lastFind != maxFind){
            FindAnyObjectByType<UnhideThings>().Unhide(maxFind);
            ShowStory(maxFind);
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
            Invoke(nameof(SpawnNew), .5f);
        }

        if(Touchscreen.current.primaryTouch.press.wasReleasedThisFrame && canS){
            if(currentGameObject != null){
                currentGameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1f);
                currentGameObject.GetComponent<Colli>().canHit = true;
                currentGameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                currentGameObject = null;
                canS = false;
                Invoke(nameof(ResetClick), 1);
                Invoke(nameof(SpawnNew), .5f);
            }
        }

    }
    private void SpawnNew(){
        if(currentGameObject == null){
            currentGameObject = Instantiate(ChooseGameObject(), worldPosition, Quaternion.identity);
            currentGameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            currentGameObject.transform.parent = parent.transform;
        }
        CancelInvoke(nameof(SpawnNew));
    }
    private GameObject ChooseGameObject(){
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
    private void ResetClick(){
        canS = true;
    }
    public void AddScore(int id){
        score += (int)Mathf.Pow(id * 3, 1.5f);
    }
    public void SetMax(int tag){
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
        SpawnNew();
    }
    public void Lose(){

    }
    private void ShowStory(int id){
        story.SetActive(true);
        string text1 = "";
        canS = false;
        switch(id){
            case 1:
                text1 = "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Veritatis corporis dolores voluptate minima omnis, aperiam sunt, facilis fuga exercitationem a vero consequuntur fugit ullam optio cumque architecto quia! Fugit, iure." + id.ToString();
            break;
            case 2:
                text1 = "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Veritatis corporis dolores voluptate minima omnis, aperiam sunt, facilis fuga exercitationem a vero consequuntur fugit ullam optio cumque architecto quia! Fugit, iure." + id.ToString();
            break;
            case 3:
                text1 = "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Veritatis corporis dolores voluptate minima omnis, aperiam sunt, facilis fuga exercitationem a vero consequuntur fugit ullam optio cumque architecto quia! Fugit, iure." + id.ToString();
            break;
            case 4:
                text1 = "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Veritatis corporis dolores voluptate minima omnis, aperiam sunt, facilis fuga exercitationem a vero consequuntur fugit ullam optio cumque architecto quia! Fugit, iure." + id.ToString();
            break;
            case 5:
                text1 = "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Veritatis corporis dolores voluptate minima omnis, aperiam sunt, facilis fuga exercitationem a vero consequuntur fugit ullam optio cumque architecto quia! Fugit, iure." + id.ToString();
            break;
        }
        storyText.text = text1;
    }

    public void HideStory(){
        storyText.text = "";
        story.SetActive(false);
        Invoke(nameof(ResetClick), .5f);
    }
}
