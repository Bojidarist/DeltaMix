using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode keyToPress;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                gameObject.SetActive(false);

                if (Mathf.Abs(transform.position.y) > 0.25)
                {
                    Debug.Log("Normal hit");
                    GameManager.Instance.GoodHit();
                }
                else if (Mathf.Abs(transform.position.y) > 0.05)
                {
                    Debug.Log("Good hit");
                    GameManager.Instance.GoodHit();
                }
                else
                {
                    Debug.Log("Perfect hit");
                    GameManager.Instance.PerfectHit();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            canBePressed = false;

            if (gameObject.activeSelf)
            {
                GameManager.Instance.NoteMissed();
            }
        }
    }
}
