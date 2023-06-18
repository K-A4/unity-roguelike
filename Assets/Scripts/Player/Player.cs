using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : Hittable
{
    public PlayerMovement Movement;
    public Attacker Attacker;
    public Image HitScreen;
    public InventorySystem Inventory;
    public MainHand MainHand;
    public UnityEvent OnItemUsed;
    
    public Vector3 Forward { get { return Camera.main.transform.forward; } }
    [SerializeField] private Color hitColor;
    private Item usingItem;

    
    private void Start()
    {
        HitScreen.color = Color.black;
        OnGetHit.Invoke(Mathf.RoundToInt(health));
        animator = GetComponent<Animator>();
    }
    
    public override void GetHit(float damage, Vector3 pos)
    {
        health -= damage;
        if (health < 0)
        {
            Die(pos);
            health = 0;
        }

        OnGetHit.Invoke(Mathf.RoundToInt(health));
        if (damage > 0)
        {
            var col = hitColor;
            HitScreen.color = col;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MainHand.UseSelectedItem(this);
        }

        if (health > 0 )
        {
            var col = HitScreen.color;
            col.a += -0.5f * Time.deltaTime;
            HitScreen.color = col;
        }
    }

    public void UseAnimation(Item usingItem)
    {
        this.usingItem = usingItem;
        animator.SetTrigger("Use");
    }

    public void OnUseAnimationEnd()
    {
        OnItemUsed?.Invoke();
        OnItemUsed.RemoveListener((usingItem as UsableItem).OnDestroyItem);
    }

    public void OnConsumeAnimation()
    {
        (usingItem as UsableItem).Consume(this);
    }

    public override void Die(Vector3 pos)
    {
        base.Die(pos);
        Destroy(gameObject);
    }
}
