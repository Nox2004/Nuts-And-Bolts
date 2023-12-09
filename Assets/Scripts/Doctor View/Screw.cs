using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screw : MonoBehaviour
{
    private float y_spd = 0, x_spd, grv = 260f;

    [SerializeField] float screw_time = 1.5f;
    private float screw_timer = 0;

    [SerializeField] bool randomized = true;

    public int type;
    public bool is_screwed = false;
    public bool is_being_screwed = false;

    [SerializeField] private float angular_vel;
    [SerializeField] private DoctorPlayer player;

    [SerializeField] private Sprite[] sprite_by_type = new Sprite[3];

    // Start is called before the first frame update
    void Start()
    {
        if (randomized) type = (int) Random.Range(1, 4);
        GetComponent<Image>().sprite = sprite_by_type[type-1];
    }

    // Update is called once per frame
    void Update()
    {
        is_being_screwed = false;

        //If mouse is over screw, start screwing it
        if ((RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition)) && !is_screwed)
        {
            DoctorScrewDriver driver = null;
            if (player.selected_item != null) driver = player.selected_item.GetComponent<DoctorScrewDriver>();

            if (driver != null)
            {
                if (Input.GetMouseButton(0) && driver.properties.type == type)
                {
                    is_being_screwed = true;
                    driver.is_screwing = true;
                }
            }
        }

        if (is_being_screwed)
        {
            transform.Rotate(0, 0, angular_vel * Time.deltaTime);

            screw_timer += Time.deltaTime;
            if (screw_timer >= screw_time)
            {
                is_being_screwed = false;
                is_screwed = true;

                y_spd = Random.Range(30f, 60f);
                x_spd = Random.Range(-6f, 6f);
            }
        }

        if (is_screwed)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);

            y_spd -= grv*Time.deltaTime;
        }
        

        var aux = transform.localPosition;
        aux.x += x_spd * Time.deltaTime;
        aux.y += y_spd * Time.deltaTime;
        transform.localPosition = aux;
    }
}
