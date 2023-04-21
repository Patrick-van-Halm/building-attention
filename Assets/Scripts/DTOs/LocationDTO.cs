using System;

[Serializable]
public class LocationDTO
{
    [Serializable]
    public class Locationcoordinate
    {
        public float x;
        public float y;
        public string unit;
    }

    [Serializable]
    public class Geocoordinate
    {
        public int latitude;
        public int longitude;
        public string unit;
    }

    [Serializable]
    public class Rawlocation
    {
        public int rawX;
        public int rawY;
        public string unit;
    }

    [Serializable]
    public class Maxdetectedrssi
    {
        public string apMacAddress;
        public string band;
        public int rssi;
        public int lastHeardInSeconds;
    }

    [Serializable]
    public class Hierarchydetails
    {
        public Campus campus;
        public Building building;
        public Floor floor;
    }

    [Serializable]
    public class Campus
    {
        public string id;
        public string name;
    }

    [Serializable]
    public class Building
    {
        public string id;
        public string name;
    }

    [Serializable]
    public class Floor
    {
        public string id;
        public string name;
    }

    [Serializable]
    public class Rssientry
    {
        public string apMacAddress;
        public string band;
        public int rssi;
        public int lastHeardSecs;
        public int slot;
        public int antennaIndex;
    }

    public string notificationType;
    public string subscriptionName;
    public int eventId;
    public string locationMapHierarchy;
    public Locationcoordinate locationCoordinate;
    public Geocoordinate geoCoordinate;
    public int confidenceFactor;
    public string apMacAddress;
    public bool associated;
    public string username;
    public string[] ipAddress;
    public string ssid;
    public string band;
    public string floorId;
    public string entity;
    public string deviceId;
    public DateTime lastSeen;
    public Rawlocation rawLocation;
    public string locComputeType;
    public string manufacturer;
    public Maxdetectedrssi maxDetectedRssi;
    public Hierarchydetails hierarchyDetails;
    public Rssientry[] rssiEntries;
    public long timestamp;
    public string sourceNotification;
    public string sourceNotificationKey;
    public string notificationTime;
    public string macAddress;
}
