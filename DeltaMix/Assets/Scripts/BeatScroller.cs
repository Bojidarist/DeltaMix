using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    /// <summary>
    /// The speed that the note will scroll
    /// </summary>
    private float scrollSpeed;

    /// <summary>
    /// The beat per minute of the song
    /// </summary>
    public float BPM;

    /// <summary>
    /// Indicates if the player started playing
    /// </summary>
    public bool hasStarted;

    // Start is called before the first frame update
    void Start()
    {
        scrollSpeed = BPM / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            if (Input.anyKeyDown)
            {
                hasStarted = true;
            }
        }
        else
        {
            // Scroll the note
            transform.position -= new Vector3(0f, scrollSpeed * Time.deltaTime, 0f);
        }
    }
}
