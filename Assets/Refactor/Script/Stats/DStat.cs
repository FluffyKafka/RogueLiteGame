using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StatsSystem
{
    [System.Serializable]
    internal class DStat
    {
        [SerializeField] protected float baseValue;
        [Header("Test")]
        [SerializeField] protected List<float> adders = new List<float>();
        [SerializeField] protected List<float> multiplyers = new List<float>();

        public float GetValue()
        {
            float finalValue = baseValue;
            foreach (float add in adders)
            {
                finalValue += add;
            }
            foreach(float multi in multiplyers)
            {
                finalValue *= multi;
            }
            return finalValue;
        }

        public void SetDefaultValue(float _value)
        {
            baseValue = _value;
        }

        public void AddAdder(float _modifier)
        {
            adders.Add(_modifier);
        }

        public void RemoveAdder(float _modifier)
        {
            adders.Remove(_modifier);
        }

        public void AddMultiplyer(float _modifier)
        {
            multiplyers.Add(_modifier);
        }

        public void RemoveMultiplyer(float _modifier)
        {
            multiplyers.Remove(_modifier);
        }
    }
}