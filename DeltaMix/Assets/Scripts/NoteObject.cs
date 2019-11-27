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
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                gameObject.SetActive(false);

                if (Mathf.Abs(transform.position.y) > 0.25)
                {
                    Debug.Log("Normal hit");
                    GameManager.Instance.NormalHit();
                    Instantiate(hitEffect, gameObject.transform.position, hitEffect.transform.rotation);
                }
                else if (Mathf.Abs(transform.position.y) > 0.1)
                {
                    Debug.Log("Good hit");
                    GameManager.Instance.GoodHit();
                    Instantiate(goodHitEffect, gameObject.transform.position, goodHitEffect.transform.rotation);
                }
                else
                {
                    Debug.Log("Perfect hit");
                    GameManager.Instance.PerfectHit();
                    Instantiate(perfectHitEffect, gameObject.transform.position, perfectHitEffect.transform.rotation);
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
                Instantiate(missEffect, gameObject.transform.position, missEffect.transform.rotation);
            }
        }
    }
}
