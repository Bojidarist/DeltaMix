using UnityEngine;
using UnityEngine.UI;

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

    /// <summary>
    /// The current score
    /// </summary>
    public int score = 0;

    /// <summary>
    /// The score multiplier
    /// </summary>
    public int scoreMultiplier = 1;

    /// <summary>
    /// The threshold for upping the multiplier
    /// </summary>
    public int multiplierThreshold = 4;

    /// <summary>
    /// Tracks if the multiplier threshold is met
    /// </summary>
    private int multiplierTracker = 1;

    /// <summary>
    /// The score given when a note is hit
    /// </summary>
    public int scorePerNote = 10;

    /// <summary>
    /// The score given when a note is hit with good accuracy
    /// </summary>
    public int scorePerGoodNote = 25;

    /// <summary>
    /// The score given when a note is hit with perfect accuracy
    /// </summary>
    public int scorePerPerfectNote = 50;

    /// <summary>
    /// The current combo
    /// </summary>
    public int combo = 0;

    /// <summary>
    /// The text representing the combo
    /// </summary>
    public Text comboText;

    /// <summary>
    /// The text representing the score
    /// </summary>
    public Text scoreText;

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

    // Start is called before the first frame update
    private void Start()
    {
        // Set defaults
        scoreText.text = $"Score(x{ scoreMultiplier }): { score }";
        comboText.text = $"Combo: { combo }";
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

    public void NormalHit()
    {
        score += scorePerNote * scoreMultiplier;
        NoteHit();
    }

    public void GoodHit()
    {
        score += scorePerGoodNote * scoreMultiplier;
        NoteHit();
    }

    public void PerfectHit()
    {
        score += scorePerPerfectNote * scoreMultiplier;
        NoteHit();
    }

    private void NoteHit()
    {
        combo++;
        if (multiplierTracker % multiplierThreshold == 0)
        {
            scoreMultiplier++;
            multiplierTracker = 1;
        }
        else
        {
            multiplierTracker++;
        }
        scoreText.text = $"Score(x{ scoreMultiplier }): { score }";
        comboText.text = $"Combo: { combo }";
    }

    public void NoteMissed()
    {
        scoreMultiplier = 1;
        multiplierTracker = 1;
        combo = 0;
        Debug.Log("Note missed");
    }
}
