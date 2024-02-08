using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float blast_radius;
    private BoxCollider hitbox;
    public int damage;
    public int knockback_force;
    public GameObject smoke;
    private List<GameObject> smokes = new List<GameObject>();
    public int num_smokes;

    private AudioClip explosionSound;
    // Start is called before the first frame update
    void Awake()
    {
        hitbox = GetComponent<BoxCollider>();
        hitbox.enabled = false;
        blast_radius = hitbox.size.x / 2.0f;
        explosionSound = Resources.Load<AudioClip>("Zelda/Sound-Effects/SoundEffect11");
    }
    void OnTriggerStay(Collider other)
    {
        Debug.Log("explosion hit");
        if (other.gameObject.CompareTag("Enemy") && other.gameObject != null) {
            other.gameObject.GetComponent<EnemyHealth>().AlterHealth(damage);
        }
    }

    public IEnumerator Explode() {
        AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position);
        GetComponent<SpriteRenderer>().enabled = false;
        hitbox.enabled = true;
        //spawn smokes randomly
        for (int i = 0; i < num_smokes; ++i) {
            Vector2 random_position = new Vector2(Random.Range(-blast_radius, blast_radius), Random.Range(-blast_radius, blast_radius));
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
