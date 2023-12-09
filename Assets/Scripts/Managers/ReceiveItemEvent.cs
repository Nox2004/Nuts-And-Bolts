using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class ReceiveItemEvent : MonoBehaviour, IOnEventCallback
{
    public CharacterID character;
    public LevelManager levelManager;

    public void OnEvent(EventData photonEvent)
    {
        // Check if the event code is the same as the one sent in the SendItemEvent script
        if (photonEvent.Code == 1)
        {
            // Get the item data from the event data
            //SendItemEventData itemData = (SendItemEventData)photonEvent.CustomData;
            object[] data = (object[])photonEvent.CustomData;

            string itemPrefabName = (string) data[1];

            ItemProperties properties = null;
            switch (data[0])
            {
                case 0:
                    properties = new GearProperties((int) data[2], (bool) data[3]);
                break;
                case 1:
                    properties = new ScrewDriverProperties((int) data[2] );
                break;
                case 2:
                    properties = new BatteryProperties((float) data[2] );
                break;
            }

            // Instantiate the item prefab with its properties
            //GameObject itemInstance = (character == CharacterID.Assistent) ? levelManager.ReceiveAssistent(itemData.itemPrefabName,itemData.properties) : levelManager.ReceiveDoctor(itemData.itemPrefabName,itemData.properties);
            GameObject itemInstance = (character == CharacterID.Assistent) ? levelManager.ReceiveAssistent(itemPrefabName,properties) : levelManager.ReceiveDoctor(itemPrefabName,properties);

            // Set the item's properties
            //itemInstance.GetComponent<Item>().properties = (ItemProperties)itemData.properties;
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }   
}