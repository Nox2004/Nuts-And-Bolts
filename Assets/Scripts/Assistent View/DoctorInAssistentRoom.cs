using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorInAssistentRoom : MonoBehaviour
{
    public bool handing_item = false;
    public GameObject held_item = null;
    public Vector3 instantiate_pos;
    [SerializeField] private Vector3 instantiate_offset;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        instantiate_pos = transform.position + instantiate_offset;
        animator = GetComponent<Animator>();
        animator.SetBool("HandingItem", handing_item);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("HandingItem", handing_item);

        if (handing_item)
        {
            if (held_item == null)
            {
                handing_item = false;
                return;
            }

            if (held_item.transform.position != instantiate_pos) held_item = null;
        }
    }
}
