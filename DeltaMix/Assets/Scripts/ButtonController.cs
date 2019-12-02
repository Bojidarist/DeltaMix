using UnityEngine;

public class ButtonController : MonoBehaviour
{
    /// <summary>
    /// The sprite renderer for the current button
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// The default sprite of the button
    /// </summary>
    public Sprite defaultSprite;

    /// <summary>
    /// The sprite when the button is pressed
    /// </summary>
    public Sprite pressedSprite;

    /// <summary>
    /// The key that corresponds to this button
    /// </summary>
    public KeyCode keyToPress;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            spriteRenderer.sprite = pressedSprite;

            if (GameManager.Instance.noteInRange)
            {
                NoteObject note = GameManager.Instance.notes.Peek();
                if (note.keyToPress == keyToPress)
                {
                    if (Mathf.Abs(note.transform.position.y) > 0.5)
                    {
                        Debug.Log("Miss");
                        GameManager.Instance.NoteMissed();
                        Instantiate(note.missEffect, note.gameObject.transform.position, note.missEffect.transform.rotation);
                    }
                    else if (Mathf.Abs(note.transform.position.y) > 0.25)
                    {
                        Debug.Log("Normal hit");
                        GameManager.Instance.NormalHit();
                        Instantiate(note.hitEffect, note.gameObject.transform.position, note.hitEffect.transform.rotation);
                    }
                    else if (Mathf.Abs(note.transform.position.y) > 0.1)
                    {
                        Debug.Log("Good hit");
                        GameManager.Instance.GoodHit();
                        Instantiate(note.goodHitEffect, note.gameObject.transform.position, note.goodHitEffect.transform.rotation);
                    }
                    else
                    {
                        Debug.Log("Perfect hit");
                        GameManager.Instance.PerfectHit();
                        Instantiate(note.perfectHitEffect, note.gameObject.transform.position, note.perfectHitEffect.transform.rotation);
                    }
                    Destroy(note.gameObject);
                }
            }
        }
        else if (Input.GetKeyUp(keyToPress))
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }
}
