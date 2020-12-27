using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagSystem : MonoBehaviour
{
    //private string[] tags = new string[0];
    [SerializeField]
    //private List<string> tags = new List<string>();
    private List<Tags> tags = new List<Tags>();
    public enum Tags
    {
        damagable,
        player
    }

    private void Awake()
    {
        AddTag_Damagable();
    }

    void AddTag_Damagable()
    {
        if (gameObject.GetComponent<Health>() != null)
        {
            tags.Add(Tags.damagable);
        }
    }


    public bool CheckTag(Tags _tag)
    {
        foreach (Tags tag in tags)
        {
            if (tag == _tag)
            {
                return true;
            }
        }
        return false;
    }



}
