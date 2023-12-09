using System;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class SendItemEvent : MonoBehaviour
{
    void Awaken()
    {

    }
    public void SendItem(SendItemEventData data)
    {
        // Set the event code
        byte photonEventCode = 1;

        ///////////// DELETAR DEPOIS
        object[] content = new object[1]; 

        if (data.properties is GearProperties)
        {
            GearProperties prop = (GearProperties) data.properties;
            content = new object[] { 0, data.itemPrefabName, prop.type, prop.broken };
        }

        if (data.properties is ScrewDriverProperties)
        {
            ScrewDriverProperties prop = (ScrewDriverProperties) data.properties;
            content = new object[] { 1, data.itemPrefabName, prop.type };
        }

        if (data.properties is BatteryProperties)
        {
            BatteryProperties prop = (BatteryProperties) data.properties;
            content = new object[] { 2, data.itemPrefabName, prop.charge };
        }

        //////////// 

        // Send the event to the target player
        PhotonNetwork.RaiseEvent(photonEventCode, content, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
    }
}

[Serializable]
public abstract class ItemProperties
{
    public ItemProperties()
    {
        
    }
}

[Serializable]
public class SendItemEventData
{
    public string itemPrefabName;

    public ItemProperties properties;

    public SendItemEventData(string itemPrefabName, ItemProperties properties)
    {
        this.itemPrefabName = itemPrefabName;
        this.properties = properties;
    }
}

[Serializable]
public class GearProperties : ItemProperties
{
    [Range(1, 3)]
    public int type;
    public bool broken;

    public GearProperties(int type, bool broken) : base()
    {
        this.type = type;
        this.broken = broken;
    }
}

[Serializable]
public class ScrewDriverProperties : ItemProperties
{
    [Range(1, 3)]
    public int type;

    public ScrewDriverProperties(int type) : base()
    {
        this.type = type;
    }
}

[Serializable]
public class BatteryProperties : ItemProperties
{
    [Range(0, 1)]
    public float charge;

    public BatteryProperties(float charge) : base()
    {
        this.charge = charge;
    }
}