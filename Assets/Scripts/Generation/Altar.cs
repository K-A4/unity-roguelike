using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Altar : MonoBehaviour, Iinteractable
{
    public static Altar Instance;
    public UnityEvent OnInteract;
    [SerializeField] private List<int> corpsesOnLevel = new List<int>();
    [SerializeField] private ParticleSystem particles;
    private int corpsesCount;
    public SpriteRenderer spriteRenderer;
    public List<Sprite> sprites = new List<Sprite>();

    private void Awake()
    {
        Instance = this;
    }

    public void Interact(Vector3 pos)
    {
        if (corpsesCount >= corpsesOnLevel[DungeonGeneration.Level])
        {
            OnInteract?.Invoke();
            corpsesCount = 0;
            RedrawVisual();
        }
        else
        {
            UIText.Instance.SendText("I NEED "+  (corpsesOnLevel[DungeonGeneration.Level] - corpsesCount) + " CORPSES");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Corpse corpse))
        {
            corpsesCount += corpse.value;
            RedrawVisual();
            particles.Play();
            Destroy(other.gameObject);
        }
    }

    private void RedrawVisual()
    {
        float i = 0;
        if (corpsesOnLevel[DungeonGeneration.Level] != 0)
        {
            i = Mathf.Clamp01(corpsesCount / (float)corpsesOnLevel[DungeonGeneration.Level]);
        }
        spriteRenderer.sprite = sprites[(int)(i * (sprites.Count - 0.5f))];
    }
}
