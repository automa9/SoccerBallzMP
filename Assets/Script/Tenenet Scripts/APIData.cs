using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ContainerA
{
    public Message message;
    public string status;
}

[System.Serializable]
public class Message
{
    public string first_name;
    public string last_name;
    public string alias;
    public string id;
    public string created;
    public List<Score> score;
}

[System.Serializable]
public class Score
{
    public string metric_id;
    public string metric_name;
    public string metric_type;
    public string value;
}

[System.Serializable]
public class ContainerB
{
    public SecondMessage message;
    public string status;
}

[System.Serializable]
public class SecondMessage
{
    public List<LeaderBoard> data;
}

[System.Serializable]
public class LeaderBoard
{
    public string alias;
    public int rank;
}