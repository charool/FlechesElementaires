using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    protected void Start()
    {
        source = GetComponent<AudioSource>();
    }

    protected void Update()
    {
        source.enabled = (
            Input.GetKey(Raccourcis.rigth) ||
            Input.GetKey(Raccourcis.left) ||
            Input.GetKey(Raccourcis.back) ||
            Input.GetKey(Raccourcis.forward)
        );

        if (
            Input.GetKey(Raccourcis.forward) &&
            Input.GetKey(Raccourcis.sprint)
        )
        {
            source.pitch = 1;
        } else
        {
            source.pitch = 0.5f;
        }
    }
}
