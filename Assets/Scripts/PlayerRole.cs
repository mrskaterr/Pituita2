using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerRole
{
    public List<Role> roles = new List<Role>();

    [System.Serializable]
    public class Role
    {
        public string name;
        public Sprite icon;
    }
}