using DeltaMix.Core;
using DeltaMix.Data;
using UnityEngine;

namespace DeltaMix
{
    public class NoteObject : MonoBehaviour
    {
        /// <summary>
        /// Indicates if the note can be pressed
        /// </summary>
        public bool canBePressed;

        /// <summary>
        /// The key required for the note to be pressed
        /// </summary>
        public KeyCode keyToPress;

        // Hit effects
        public GameObject hitEffect;
        public GameObject goodHitEffect;
        public GameObject perfectHitEffect;
        public GameObject missEffect;

        /// <summary>
        /// The note's side
        /// </summary>
        [SerializeField]
        public Sides side;

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
                GameManager.Instance.EnqueueNote(this);
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
            GameManager.Instance.noteInRange = false;
            if (GameManager.Instance.GetNotesCount(side) != 0)
            {
                GameManager.Instance.DequeueNote(side);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Activator")
            {
                canBePressed = false;
            }
        }
    }
}