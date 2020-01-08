using UnityEngine;

namespace DeltaMix
{
    public class EffectObject : MonoBehaviour
    {
        /// <summary>
        /// The lifetime of the effect
        /// </summary>
        public float lifeTime = 1f;

        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, lifeTime);
        }
    }
}
