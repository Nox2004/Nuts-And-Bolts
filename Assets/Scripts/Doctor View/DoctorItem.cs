using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoctorItem : MonoBehaviour
{
    protected LevelManager level_manager;
    public GameObject assistent_counterpart;
    public ItemProperties properties;

    public bool received = false; //true if item was received from player - false if item was normally created

    protected Image image;
    public bool being_drag = false;
    protected bool leaving = false;

    public float table_x = -43f;

    public float my_width, my_height, canvas_width, canvas_height;

    // Start is called before the first frame update
    void Start()
    {
        level_manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        //change order in the hierarch so is the one before the last
        transform.SetSiblingIndex(transform.parent.childCount-2);

        image = GetComponent<Image>();

        //get the size of the image
        my_width = image.rectTransform.rect.width;
        my_height = image.rectTransform.rect.height;

        //get the size of the canvas
        canvas_width = image.canvas.gameObject.GetComponent<RectTransform>().rect.width;
        canvas_height = image.canvas.gameObject.GetComponent<RectTransform>().rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        //if image changes size
        if (my_width != image.rectTransform.rect.width || my_height != image.rectTransform.rect.height)
        {
            //get the size of the image
            my_width = image.rectTransform.rect.width;
            my_height = image.rectTransform.rect.height;
        }
        if (leaving)
        {
            var aux = transform.localPosition;
            aux.x-= 30 * Time.deltaTime;
            transform.localPosition = aux;
            if (aux.x < -canvas_width / 2 - my_width / 2f)
            {
                Destroy(gameObject);
            }

            return;
        }
        //if is not being dragged
        if (!being_drag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //if mouse is over image start dragging
                if (RectTransformUtility.RectangleContainsScreenPoint(image.rectTransform, Input.mousePosition))
                {
                    //being_drag = true;
                }
            }
        }
        else
        {
            //move the object to the mouse position in canvas
            transform.position = Input.mousePosition;

            //if mouse is released, stop dragging
            if (Input.GetMouseButtonUp(0))
            {
                if (transform.localPosition.x > table_x)
                {
                    transform.localPosition = new Vector3(table_x-my_width/2, transform.localPosition.y, transform.localPosition.z);
                }
                else if (transform.localPosition.x < -canvas_width/2 + my_width / 2f)
                {
                    leaving = true;
                    //Sends to the assistent
                    level_manager.SendItem(new SendItemEventData(assistent_counterpart.name,properties));
                }
                being_drag = false;
            }
        }

        

        //clamps position so the whole image is keep inside the canvas        
        Vector3 pos = transform.localPosition;
        pos.x = Mathf.Clamp(pos.x, -canvas_width/2 - my_width / 2f, canvas_width/2 - my_width / 2f);
        pos.y = Mathf.Clamp(pos.y, -canvas_height/2 + my_height / 2f, canvas_height/2 - my_height / 2f);
        transform.localPosition = pos;

    }
}
