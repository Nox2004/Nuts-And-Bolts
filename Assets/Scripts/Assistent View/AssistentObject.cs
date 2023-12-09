using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AssistentObject : MonoBehaviour
{
    [SerializeField] protected float outlineSize = 1f;
    [SerializeField] protected Color outlineColor = Color.white;
    protected SpriteRenderer sprite_renderer;

    protected bool selected = false;

    // Start is called before the first frame update
    protected void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected void Update()
    {
        sprite_renderer.material.SetFloat("_Outline", 0f);
        if (selected)
        {
            sprite_renderer.material.SetFloat("_Outline", 1f);
            sprite_renderer.material.SetColor("_OutlineColor", outlineColor);
            sprite_renderer.material.SetFloat("_OutlineSize", outlineSize);
        }

        selected = false;
    }

    public virtual GameObject OnInteract(GameObject held_item)
    {
        if (held_item != null) return held_item;

        Debug.Log("Interacted with " + gameObject.name);

        return gameObject;
    }

    public virtual void OnSelected(GameObject held_item)
    {
        //if player is holding something, don't select
        if (held_item != null) return;

        selected = true;
    }
}
