﻿using System.Collections.Generic;
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
    public Queue<NoteObject> notes = new Queue<NoteObject>();
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
            if (!resultScreen.activeInHierarchy && !music.isPlaying && GameManager.Instance.notes.Count == 0)
            {
                resultScreen.SetActive(true);

                float accPlayerScore = (scorePerNote * normalHits) + (scorePerGoodNote * goodHits) + (scorePerPerfectNote * perfectHits);
                float accMaxScore = scorePerPerfectNote * totalNotes;
                float acc = accPlayerScore / accMaxScore;
                string rank;
                resultScoreText.text = score.ToString();
                resultNormalHitsText.text = normalHits.ToString();
                resultGoodHitsText.text = goodHits.ToString();
                resultPerfectHitsText.text = perfectHits.ToString();
                resultMissedHitsText.text = missedHits.ToString();
                resultAccuracyText.text = $"{ (acc * 100).ToString("F1") }%";

                if (0.95 <= acc)
                {
                    rank = "S";
                }
                else if (0.90 <= acc)
                {
                    rank = "A";
                }
                else if (0.85 <= acc)
                {
                    rank = "B";
                }
                else if (0.80 <= acc)
                {
                    rank = "C";
                }
                else
                {
                    rank = "D";
                }

                resultRankText.text = rank;
            }
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
        Debug.Log("Note missed");
    }
}
