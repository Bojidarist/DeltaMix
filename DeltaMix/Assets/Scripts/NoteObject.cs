using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode keyToPress;

    public GameObject hitEffect;
    public GameObject goodHitEffect;
    public GameObject perfectHitEffect;
    public GameObject missEffect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canBePressed == true)
        {
            GameManager.Instance.noteInRange = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            GameManager.Instance.notes.Enqueue(this);
            canBePressed = true;
        }
        else if (collision.tag == "MissTrigger")
        {
            Destroy(this.gameObject);
            GameManager.Instance.NoteMissed();
            Instantiate(missEffect, gameObject.transform.position, missEffect.transform.rotation);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.notes.Dequeue();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            canBePressed = false;
        }
    }
}
