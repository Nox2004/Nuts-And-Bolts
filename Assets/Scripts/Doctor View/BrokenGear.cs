using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrokenGear : MonoBehaviour
{
    private DoctorItem myself;
    private Vector3 initial_position;
    
    private bool was_being_dragged = false;

    [SerializeField] private GameObject body;
    [SerializeField] private GameObject table;

    // Start is called before the first frame update
    void Start()
    {
        myself = GetComponent<DoctorItem>();
        initial_position = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (was_being_dragged && !myself.being_drag)
        {
            if (transform.localPosition.x >= myself.table_x-myself.my_width/2)
            {
                transform.position = initial_position;
                Debug.Log("I was being dragged and now I'm not");
            }
            else 
            {
                //deativate this script and add a normal DoctorItem
                Destroy(this);
                return;
            }
        }

        was_being_dragged = myself.being_drag;

        if (myself.being_drag)
        {
            transform.SetParent(table.transform);
            //change order in the hierarch so is the one before the last
            transform.SetSiblingIndex(transform.parent.childCount-2);
        }
        else
        {
            transform.SetParent(body.transform);
        }

    }
}
