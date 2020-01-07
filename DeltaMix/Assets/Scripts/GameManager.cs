using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The current playing music
    /// </summary>
    public AudioSource music;

    /// <summary>
    /// The volume of the music
    /// </summary>
    [SerializeField, GetSet(nameof(MusicVolume))]
    private float _musicVolume = 1f;
    public float MusicVolume { get { return _musicVolume; } set { _musicVolume = value; music.volume = value; } }

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

    // Note/Hit count
    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;

    // Result screen
    public GameObject resultScreen;
    public Text resultAccuracyText, resultNormalHitsText, resultGoodHitsText,
        resultPerfectHitsText, resultMissedHitsText, resultScoreText, resultRankText;

    // Notes
    public Queue<NoteObject> leftNotes = new Queue<NoteObject>();
    public Queue<NoteObject> rightNotes = new Queue<NoteObject>();
    public bool noteInRange = false;

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
        totalNotes = FindObjectsOfType<NoteObject>().Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                beatScroller.hasStarted = true;

                music.Play();
            }
        }
        else
        {
            if (!resultScreen.activeInHierarchy && !music.isPlaying && GameManager.Instance.leftNotes.Count == 0 && GameManager.Instance.rightNotes.Count == 0)
            {
                FinishSong();
            }
        }
    }

    private void FinishSong()
    {
        OpenResultScreen(CalculateAccuracy());
    }

    private void OpenResultScreen(float accuracy)
    {
        resultScreen.SetActive(true);
        resultScoreText.text = score.ToString();
        resultNormalHitsText.text = normalHits.ToString();
        resultGoodHitsText.text = goodHits.ToString();
        resultPerfectHitsText.text = perfectHits.ToString();
        resultMissedHitsText.text = missedHits.ToString();
        resultAccuracyText.text = $"{ (accuracy * 100).ToString("F1") }%";
        resultRankText.text = CalculateRank(accuracy);
    }

    public float CalculateAccuracy()
    {
        float accPlayerScore = (scorePerNote * normalHits) + (scorePerGoodNote * goodHits) + (scorePerPerfectNote * perfectHits);
        float accMaxScore = scorePerPerfectNote * totalNotes;
        return accPlayerScore / accMaxScore;
    }

    public string CalculateRank(float acc)
    {
        if (0.95 <= acc)
        {
            return "S";
        }
        else if (0.90 <= acc)
        {
            return "A";
        }
        else if (0.85 <= acc)
        {
            return "B";
        }
        else if (0.80 <= acc)
        {
            return "C";
        }
        else
        {
            return "D";
        }
    }

    public void JudgeNote(NoteObject note)
    {
        if (Mathf.Abs(note.transform.position.y) > 0.5)
        {
            NoteMissed();
            Instantiate(note.missEffect, note.gameObject.transform.position, note.missEffect.transform.rotation);
        }
        else if (Mathf.Abs(note.transform.position.y) > 0.25)
        {
            NormalHit();
            Instantiate(note.hitEffect, note.gameObject.transform.position, note.hitEffect.transform.rotation);
        }
        else if (Mathf.Abs(note.transform.position.y) > 0.1)
        {
            GoodHit();
            Instantiate(note.goodHitEffect, note.gameObject.transform.position, note.goodHitEffect.transform.rotation);
        }
        else
        {
            PerfectHit();
            Instantiate(note.perfectHitEffect, note.gameObject.transform.position, note.perfectHitEffect.transform.rotation);
        }
    }

    public void NormalHit()
    {
        score += scorePerNote * scoreMultiplier;
        NoteHit();
        normalHits++;
    }

    public void GoodHit()
    {
        score += scorePerGoodNote * scoreMultiplier;
        NoteHit();
        goodHits++;
    }

    public void PerfectHit()
    {
        score += scorePerPerfectNote * scoreMultiplier;
        NoteHit();
        perfectHits++;
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
        comboText.text = $"Combo: { combo }";
        missedHits++;
    }

    public NoteObject PeekNote(Sides side)
    {
        switch (side)
        {
            case Sides.LEFT:
                return leftNotes.Peek();
            case Sides.RIGHT:
                return rightNotes.Peek();
            default:
                throw new ArgumentException($"Invalid side { Enum.GetName(typeof(Sides), side) } in PeekNote()");
        }
    }

    public void EnqueueNote(NoteObject note)
    {
        switch (note.side)
        {
            case Sides.LEFT:
                leftNotes.Enqueue(note);
                break;
            case Sides.RIGHT:
                rightNotes.Enqueue(note);
                break;
            default:
                throw new ArgumentException($"Invalid side { Enum.GetName(typeof(Sides), note.side) } in EnqueueNote()");
        }
    }

    public NoteObject DequeueNote(Sides side)
    {
        switch (side)
        {
            case Sides.LEFT:
                return leftNotes.Dequeue();
            case Sides.RIGHT:
                return rightNotes.Dequeue();
            default:
                throw new ArgumentException($"Invalid side { Enum.GetName(typeof(Sides), side) } in DequeueNote()");
        }
    }

    public int GetNotesCount(Sides side)
    {
        switch (side)
        {
            case Sides.LEFT:
                return leftNotes.Count;
            case Sides.RIGHT:
                return rightNotes.Count;
            default:
                throw new ArgumentException($"Invalid side { Enum.GetName(typeof(Sides), side) } in GetNotesCount()");
        }
    }
}
