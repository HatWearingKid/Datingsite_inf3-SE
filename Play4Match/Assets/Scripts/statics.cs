using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statics : MonoBehaviour {

    // Een static class enkel om variables te bewaren tussen meerdere scenes, globale naam zodat we alle statics hier kunnen plaatsen
    public static string chatroomID {
        get {
            return chatroomID;
        }
        set {
            chatroomID = value;
        }
    }

}
