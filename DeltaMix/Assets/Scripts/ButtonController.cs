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
        }
        if (Input.GetKeyUp(keyToPress))
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }
}
