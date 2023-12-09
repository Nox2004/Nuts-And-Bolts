using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBody : MonoBehaviour
{
    private struct MissingGear
    {
        public BrokenGear broken;
        public Vector3 position;
        public int type;
        public bool is_placed;
        public DoctorGear placed_gear;

        public MissingGear(BrokenGear broken, Vector3 position, int type)
        {
            this.broken = broken;
            this.position = position;
            this.type = type;

            is_placed = false;
            placed_gear = null;
        }
    }

    private bool opened = false;

    [SerializeField] private GameObject lid;
    [SerializeField] private GameObject body;
    [SerializeField] private Screw[] screws = new Screw[4];
    [SerializeField] private GameObject[] gears = new GameObject[3];
    private MissingGear[] missing_gears = new MissingGear[3];
    private Collider2D collider;

    private bool has_battery = false;
    [SerializeField] private Vector3 battery_position;

    [SerializeField] private DoctorPlayer player;

    [SerializeField] private LevelManager level_manager;

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<Collider2D>();

        int i = 0;
        //deactivates all gears
        foreach (GameObject gear in gears)
        {
            //Debug.Log(gear.GetComponent<BrokenGear>().type);
            int _type = (int) Random.Range(1, 4);
            missing_gears[i] = new MissingGear(gear.GetComponent<BrokenGear>(), gear.transform.position, _type);
            gear.GetComponent<DoctorGear>().properties.type = _type;
            i++;
            gear.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //checks if all screw were screwed
        bool all_screwed = true;
        foreach (Screw screw in screws)
        {
            if (screw.is_screwed == false) 
            {
                all_screwed = false; break;
            }
        }

        //if all screws are screwed, open lid
        if (all_screwed && !opened)
        {
            opened = true;
            lid.GetComponent<CurveAnimation>().GoBack();
            
            //reactivates all gears
            foreach (GameObject gear in gears)
            {
                gear.SetActive(true);
            }
        }

        if (!opened) return;
        //IF ROBOT LID IS OPEN

        //if battery is not placed
        if (!has_battery)
        {
            DoctorBattery battery = null;
            if (player.selected_item != null) battery = player.selected_item.GetComponent<DoctorBattery>();

            //if player is holding battery
            if (battery != null)
            {
                //if mouse is near battery space and battery is charged
                if (Vector3.Distance(battery_position, battery.transform.localPosition) < 20 && battery.properties.charge == 1)
                {
                    //place battery
                    player.selected_item.transform.localPosition = battery_position;
                    player.selected_item.transform.SetParent(body.transform);
                    player.selected_item.GetComponent<DoctorItem>().enabled = false;
                    player.selected_item = null;
                    has_battery = true;
                    level_manager.SetDoctorItems(false);
                }
            }

        }

        DoctorGear holding_gear = null;
        if (player.selected_item != null) holding_gear = player.selected_item.GetComponent<DoctorGear>();

        //if is holding a gear that is not broken
        if (holding_gear != null && !holding_gear.properties.broken)
        {
            int i = -1;

            //if mouse is near some missing gear
            foreach (MissingGear gear in missing_gears)
            {
                i++;

                //if broken gear is still placed, continue
                if (gear.broken != null) continue;

                //if gear was alread placed, continue
                if (gear.is_placed) continue;
                
                if (Vector3.Distance(gear.position, holding_gear.transform.position) < 50)
                {
                    //if gear type is correct
                    if (gear.type == holding_gear.properties.type)
                    {
                        //place gear
                        missing_gears[i].is_placed = true;
                        player.selected_item = null;
                        missing_gears[i].placed_gear = holding_gear;

                        //deactivate holding gear object script
                        holding_gear.GetComponent<DoctorItem>().enabled = false;
                        holding_gear.transform.position = gear.position;
                        holding_gear.gameObject.transform.SetParent(body.transform);
                        level_manager.SetDoctorItems(false);
                        break;
                    }
                }
            }
        }

        //checks if all gears are placed
        bool all_placed = true;
        foreach (MissingGear gear in missing_gears)
        {
            if (gear.is_placed == false)
            {
                all_placed = false; break;
            }
        }

        //if all gears are placed, win the game
        if (all_placed && has_battery)
        {
            level_manager.WinTheLevel();
            foreach (MissingGear gear in missing_gears)
            {
                gear.placed_gear.rotate = true;
            }
        }
    }
}
