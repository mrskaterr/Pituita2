using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    [SerializeField] private bool drawGizmos = true;
    public List<SpaceInfo> spaces = new List<SpaceInfo>();

    public string GetName(Vector3 _pos)
    {
        for (int i = 0; i < spaces.Count; i++)
        {
            float maxX = spaces[i].centerPoint.position.x + spaces[i].size.x / 2f;
            float minX = spaces[i].centerPoint.position.x - spaces[i].size.x / 2f;
            if (_pos.x >= minX && _pos.x <= maxX)
            {
                float maxZ = spaces[i].centerPoint.position.z + spaces[i].size.z / 2f;//TOIMPROVE: spaces[i].size.z / 2f to 1 var
                float minZ = spaces[i].centerPoint.position.z - spaces[i].size.z / 2f;
                if(_pos.z >= minZ && _pos.z <= maxZ)
                {
                    return spaces[i].name;
                }
            }
            continue;
        }

        return "Void";
    }

    //private void Start()
    //{
    //    HUD = FindObjectOfType<PlayerHUD>();
    //}

    //private PlayerHUD HUD;

    //private void Update()
    //{
    //    HUD.SetSpaceName(GetName(HUD.transform.position));
    //}

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            foreach (var space in spaces)
            {
                //Gizmos.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), .75f);
                Gizmos.color = space.color;
                Gizmos.DrawCube(space.centerPoint.position, space.size);
            }
        }
    }
    [System.Serializable]
    public class SpaceInfo
    {
        public string name;
        public Transform centerPoint;
        public Vector3 size;
        public Color color;

        //public SpaceInfo(string _name, Transform _centerPoint, Vector3 _size)
        //{
        //    name = _name;
        //    centerPoint = _centerPoint;
        //    size = _size;
        //}
    }
}