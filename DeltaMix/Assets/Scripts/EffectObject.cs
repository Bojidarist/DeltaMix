using UnityEngine;

public class EffectObject : MonoBehaviour
{
    public float lifeTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
