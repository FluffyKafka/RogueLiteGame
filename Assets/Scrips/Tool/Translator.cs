using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TranslatePair
{
    public string from;
    public string to;
}
public class Translator : MonoBehaviour
{
    public static Translator instance;

    [SerializeField] protected List<TranslatePair> translateInfo = new();
    Dictionary<string, string> dictionary = new();

    protected void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (TranslatePair pair in translateInfo)
        {
            dictionary.Add(pair.from, pair.to);
        }
    }

    protected void Start()
    {
        
    }
    protected void AddEnumTranslateSlot()
    {

    }

    public string Translate(string _eng)
    {
        if (dictionary.TryGetValue(_eng, out string trans))
        {
            return trans;
        }
        else
        {
            return _eng;
        }
    }
}
