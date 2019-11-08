using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The current playing music
    /// </summary>
    public AudioSource music;

    /// <summary>
    /// Indicates if the player started playing
    /// </summary>
    public bool startPlaying;

    /// <summary>
    /// A <see cref="BeatScroller"/>
    /// </summary>
    public BeatScroller beatScroller;

    /// <summary>
    /// Singleton instance of <see cref="GameManager"/>
    /// </summary>
    public static GameManager Instance { get; private set; }

    // Awake is called before Start
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !startPlaying)
        {
            startPlaying = true;
            beatScroller.hasStarted = true;

            music.Play();
        }
    }

    public void NoteHit()
    {
        Debug.Log($"Note hit");
    }

    public void NoteMissed()
    {
        Debug.Log("Note missed");
    }
}
