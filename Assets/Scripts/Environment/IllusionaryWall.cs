using UnityEngine;

public class IllusionaryWall : MonoBehaviour
{
    [System.NonSerialized] public bool wallHasBeenHit;
    float alpha;
    float fadeTimer = 2.5f;

    Material illusionaryWallMaterial;
    MeshRenderer mesh;
    BoxCollider wallCollider;
    AudioSource audioSource;
    AudioClip illusionaryWallSoundClip;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        illusionaryWallMaterial = mesh.material;
        audioSource = GetComponent<AudioSource>();
        wallCollider = GetComponent<BoxCollider>();
        illusionaryWallSoundClip = GetComponent<AudioSource>().clip;
    }

    private void Update()
    {
        if(wallHasBeenHit)
        {
            FadeIllusionaryWall();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(DarkSoulsConsts.PLAYER))
        {
            wallHasBeenHit = true;
        }
    }

    public void FadeIllusionaryWall()
    {
        alpha = illusionaryWallMaterial.color.a;
        alpha = alpha - Time.deltaTime / fadeTimer;

        Color fadedWallColor = new Color(1, 1, 1, alpha);
        illusionaryWallMaterial.color = fadedWallColor;

        if(wallCollider.enabled)
        {
            wallCollider.enabled = false;
            audioSource.PlayOneShot(illusionaryWallSoundClip);
        }
        if(alpha <= 0)
        {
            Destroy(mesh);
        }
    }
}
