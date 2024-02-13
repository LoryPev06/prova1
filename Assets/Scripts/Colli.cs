using UnityEngine;
using UnityEngine.AI;

public class Colli : MonoBehaviour{
    public bool destroy = false;
    public float radius = 1f;
    public int wall = 0;
    public bool canHit = false;
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag(tag)){
            CollisionManager collisionManager = FindAnyObjectByType<CollisionManager>();
            if (collisionManager != null && canHit){
                collisionManager.HandleCollision(gameObject, other.gameObject, destroy);
                //FindAnyObjectByType<PlayScript>().AddReward(int.Parse(tag));
            }
        }
        if(other.gameObject.CompareTag("Wall")){
            wall++;
        }
        if(other.gameObject.CompareTag("death")){
            Invoke(nameof(lost), 3f);
        }
    }
    private void lost(){
        FindAnyObjectByType<GameManager>().Lose();
    }
    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.CompareTag("Wall")){
            wall--;
        }
        if(other.gameObject.CompareTag("death")){
            CancelInvoke();
        }
    }
    public void addForc(){
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach(Collider2D hi in hits){
            Vector2 direction = hi.transform.position - transform.position;
            if(hi.gameObject.GetComponent<Rigidbody2D>() != null){
                hi.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * 0.5f, ForceMode2D.Impulse);
            }
        }
    }
}
