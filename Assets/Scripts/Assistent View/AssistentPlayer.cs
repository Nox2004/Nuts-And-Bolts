using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistentPlayer : MonoBehaviour
{
    //Speed of the player
    [SerializeField] private float speed = 5f;

    //Collider
    private Collider2D collider;

    //Hand properties
    [SerializeField] private float hand_offset = 1f;
    private Vector3 hand_direction = Vector3.down;
    private GameObject held_item = null;

    //My Level Manager
    [SerializeField] private LevelManager level_manager;

    //Animation and sounds stuff
    private Animator animator;
    private AudioSource walk_audio_source;
    private AudioSource interact_audio_source;
    
    //All sounds
    [SerializeField] private AudioClip get_item_sound, drop_item_sound, walking_sound, idle_sound;

    //Store item stuff
    private string item_layer;
    private int item_order;
    private GameObject hand;
    private SpriteRenderer hand_sprite;

    void Start()
    {
        //Gets animator components
        animator = GetComponent<Animator>();

        //Creates audio sources
        walk_audio_source = gameObject.AddComponent<AudioSource>();
        interact_audio_source = gameObject.AddComponent<AudioSource>();

        walk_audio_source.loop = true;
        //play idle sound
        walk_audio_source.clip = idle_sound;
        walk_audio_source.Play();

        //Sets up hand stuff
        held_item = null;

        collider = GetComponent<Collider2D>();
        hand = transform.GetChild(0).gameObject;
        hand_sprite = hand.GetComponent<SpriteRenderer>();
        hand_sprite.sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
    }

    void Update()
    {
        // Get the player's input.
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the player's movement vector.
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;

        // Move the player's hand.
        if (movement!=Vector3.zero)
        {
            hand_direction = movement.normalized;
        }
        
        #region //Animation

        if (movement.x == 0)
        {
            if (movement.y > 0)
            {
                animator.SetTrigger("Back");
            }
            else if (movement.y < 0)
            {
                animator.SetTrigger("Front");
            }
        }
        else  
        {
            transform.localScale = new Vector3(Mathf.Sign(movement.x), 1, 1);
            animator.SetTrigger("Side");
        }

        //if (movement == Vector3.zero) // pause animation 
        //{
        //    animator.enabled = false;
        //}
        //else
        //{
        //    animator.enabled = true;
        //}

        #endregion

        #region //Collision

        // Check for collisions using my collider + movement offset
        // If the player is colliding with an wall, don't move.
        var aux = transform.position; aux.x += movement.x; aux.y += -0.07f; //APAGAR DEPOIS
        Collider2D[] hits = Physics2D.OverlapBoxAll(aux, collider.bounds.size, 0);

        foreach (Collider2D hit in hits)
        { 
            if (hit.gameObject.tag == "Collision")
            {
                movement.x = 0f; break;
            }
        }

        aux = transform.position; aux.y += movement.y; aux.y += -0.07f; //APAGAR DEPOIS
        hits = Physics2D.OverlapBoxAll(aux, collider.bounds.size, 0);

        foreach (Collider2D hit in hits)
        { 
            if (hit.gameObject.tag == "Collision")
            {
                movement.y = 0f; break;
            }
        }

        #endregion

        #region //Play Walking/Idle sounds

        //if movement is 0,0 stop the walk sound and play the idle sound
        /*if (movement == Vector3.zero)
        {
            if (walk_audio_source.clip != idle_sound)
            {
                walk_audio_source.Stop();
                walk_audio_source.clip = idle_sound;
                walk_audio_source.Play();
            }
        }
        else //if movement is not 0,0 play the walk sound
        {
            if (walk_audio_source.clip != walking_sound)
            {
                walk_audio_source.Stop();
                walk_audio_source.clip = walking_sound;
                walk_audio_source.Play();
            }
        }*/

        #endregion

        // Move the player.
        transform.position = transform.position + movement;
        
        #region // Move the player's hand.

        Vector3 hand_position = transform.position + hand_direction * hand_offset;
        Quaternion hand_rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.down, hand_direction, Vector3.forward));
        int hand_order = GetComponent<SpriteRenderer>().sortingOrder + ((hand_direction.y < 0) ? 1 : -1);

        hand.transform.position = hand_position;
        hand.transform.rotation = hand_rotation;
        hand_sprite.sortingOrder = hand_order;

        #endregion

        #region //Move item with player and drops it

        // If the player is holding an item, move it with the player.
        if (held_item != null)
        {
            held_item.transform.position = hand_position;
            held_item.transform.rotation = hand_rotation;
            held_item.GetComponent<SpriteRenderer>().sortingOrder = hand_order;

            //if right button is pressed, drop the item
            if (Input.GetMouseButtonDown(1))
            {
                held_item.GetComponent<SpriteRenderer>().sortingLayerName = item_layer;
                held_item.GetComponent<SpriteRenderer>().sortingOrder = item_order;
                held_item = null;

                //play drop sound
                interact_audio_source.PlayOneShot(drop_item_sound);
            }
        }

        #endregion

        #region //Interact with objects

        //check if there is an object in front of the player
        RaycastHit2D[] hand_hits = Physics2D.RaycastAll(transform.position, hand_direction, hand_offset*2);
        foreach (RaycastHit2D hit in hand_hits)
        {
            if (hit.collider.gameObject == held_item) continue; //skips held item

            AssistentObject col_object = hit.collider.gameObject.GetComponent<AssistentObject>();

            //if there is an interactble object in front of the player
            if (col_object != null)
            {
                //Tries to select the object
                col_object.OnSelected(held_item);

                //if left button is pressed, interact
                if (Input.GetMouseButtonDown(0))
                {
                    //OnInteract returns the new held item (if there is one)
                    var new_held_item = col_object.OnInteract(held_item);

                    //if item is pickable, pick it up
                    if (held_item != new_held_item)
                    {
                        if (held_item != null && new_held_item == null) interact_audio_source.PlayOneShot(drop_item_sound); //play drop sound
                        if (new_held_item != null) interact_audio_source.PlayOneShot(get_item_sound); //play get sound 
                        
                        if (held_item != null)
                        {
                            //drops old item
                            held_item.GetComponent<SpriteRenderer>().sortingLayerName = item_layer;
                            held_item.GetComponent<SpriteRenderer>().sortingOrder = item_order;
                            held_item = null;
                        }   
                        
                        //gets new item
                        held_item = new_held_item;

                        //store the item's sorting layer and order
                        item_layer = held_item.GetComponent<SpriteRenderer>().sortingLayerName;
                        item_order = held_item.GetComponent<SpriteRenderer>().sortingOrder;

                        held_item.GetComponent<SpriteRenderer>().sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
                    }
                }
                
                break;
            }
        }

        #endregion
    }
}
