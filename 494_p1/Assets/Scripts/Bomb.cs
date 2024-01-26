using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float blast_radius;
    private BoxCollider hitbox;
    public int damage;
    public int knockback_force;
    private bool exploded;
    public GameObject smoke;
    private List<GameObject> smokes = new List<GameObject>();
    public int num_smokes;
    // Start is called before the first frame update
    void Awake()
    {
        exploded = false;
        hitbox = GetComponent<BoxCollider>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) {
            if (!exploded) {
                StartCoroutine(Explode());
            }
            other.gameObject.GetComponent<EnemyHealth>().AlterHealth(damage);
        }
    }

    public IEnumerator Explode() {
        hitbox.size = Vector2.one * blast_radius;
        GetComponent<SpriteRenderer>().enabled = false;
        //spawn smokes randomly
        for (int i = 0; i < num_smokes; ++i) {
            Vector2 random_position = new Vector2(Random.Range(-blast_radius, blast_radius) / 2, Random.Range(-blast_radius, blast_radius) / 2);
            GameObject new_smoke = Instantiate(smoke, (Vector2)transform.position + random_position, Quaternion.identity);
            smokes.Add(new_smoke);
            yield return new WaitForSeconds(Random.Range(0.01f, 0.05f));
        }
        hitbox.enabled = false;
        //delete smokes randomly
        while (smokes.Count > 0) {
            int random_index = Random.Range(0, smokes.Count-1);
            Destroy(smokes[random_index]);
            smokes.RemoveAt(random_index);
            yield return new WaitForSeconds(Random.Range(0.0f, 0.1f));
        }
        if (gameObject != null) {
            Destroy(gameObject);
        }
    }
}
