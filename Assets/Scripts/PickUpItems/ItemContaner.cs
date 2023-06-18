using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemContaner : Hittable, Iinteractable
{
    public static List<ItemContaner> LevelContainers = new List<ItemContaner>();
    public AudioSource OpenSound;
    public bool isLocked;
    [SerializeField] private List<Objects> itemsToSpawn = new List<Objects>();
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite brokenSprite;
    [SerializeField] private Sprite openSprite;
    private Objects items => itemsToSpawn[DungeonGeneration.Level];
    private bool opened;
    private bool itemSpawned;

    private void Awake()
    {
        if (Random.value > 0.75f)
        {
            isLocked = true;
        }
        LevelContainers.Add(this);
    }

    public void Interact(Vector3 pos)
    {
        if (!opened)
        {
            if (!isLocked)
            {
                SpawnItem(pos);
                spriteRenderer.sprite = openSprite;
                OpenSound.Play();
                opened = true;
                gameObject.layer = LayerMask.NameToLayer("Corpse");
            }
            else
            {
                UIText.Instance.SendText("CONTAINER IS LOCKED");
            }
        }
        else
        {
            if (health > 0)
            {
                SpawnItem(pos);
            }
        }
    }

    private void SpawnItem(Vector3 interactPos)
    {
        if (!itemSpawned)
        {
            var r = Random.value;
            var pos = transform.position - Vector3.up * 0.25f;
            GameObject go = null;
            //if (r > 0.24f)
            //{
                go = Instantiate(items.GetSpawnObject());
                go.transform.SetPositionAndRotation(pos, Quaternion.identity);
                var rb = go.GetComponent<Rigidbody>();
                var force = 2.0f;
                var forceVec = (interactPos - transform.position).normalized;
                forceVec.y = 0;
                rb.AddForce((Vector3.up + forceVec) * force, ForceMode.VelocityChange);
                itemSpawned = true;
            //}
        }
        gameObject.layer = LayerMask.NameToLayer("Corpse");
    }

    public override void GetHit(float damage, Vector3 pos)
    {
        if (hitSound)
        {
            hitSound.Play();
        }
        health -= damage;

        if (health <= 0)
        {
            Die(pos);
        }
        else
        {
            if (!opened)
            {
                if (isLocked)
                {
                    var r = Random.value;
                    if (r > 0.7f)
                    {
                        if (r > 0.9f)
                        {
                            health = 0;
                            Die(pos);
                        }

                        if (health > 0)
                        {
                            isLocked = false;
                            opened = true;
                            spriteRenderer.sprite = openSprite;
                            OpenSound.Play();
                            UIText.Instance.SendText("CONTAINER OPENED");
                        }
                    }
                }
            }
        }

        PlayDieParticles(pos);
    }

    public override void Die(Vector3 pos)
    {
        UIText.Instance.SendText("CONTAINER BROKEN");
        spriteRenderer.sprite = brokenSprite;
        gameObject.layer = LayerMask.NameToLayer("Corpse");
    }
}
