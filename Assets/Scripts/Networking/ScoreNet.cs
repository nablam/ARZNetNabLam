using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreNet : NetworkBehaviour {

    public TextMesh P1tm;
    public TextMesh P2tm;
    [SyncVar(hook = "OnP1add")]
    int p1score = 0;
    [SyncVar(hook = "OnP2add")]
    int p2score = 0;
    public List<string> playerlist;


    bool serverregistered;
    uint serverplayerId;
    public void RegisterPlayer(uint argpid) {
        if (!serverregistered)
        {
            serverplayerId = argpid;
            Debug.Log("registered WWWWWWWWWWWWWWWWWWWWWWW" + argpid);
            serverregistered = true;
        }

    }

    void OnP1add(int argp1score) { P1tm.text = " P1 score=" + p1score.ToString(); p1score = argp1score; }
    void OnP2add(int argp2score) { P2tm.text = " P2 score=" + p2score.ToString(); p2score = argp2score; }

    //TODO: a better way than this shit.. it's 4:40 and im runnig out of time
    public void AddScoreForPlayer(uint id) {
        if (serverplayerId == id)
        {
            p1score++;
          
        }
        else
        {
            p2score++;
          
        }
    }
    private void Start()
    {
        playerlist = new List<string>();
    }
}
