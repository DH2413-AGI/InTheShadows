using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalStorage
{
    static public string LastIPAdress {
        get => PlayerPrefs.GetString("LastIPAdress"); 
        set => PlayerPrefs.SetString("LastIPAdress", value);
    }

}
