using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SkillSystem
{
    internal class SMTimer : SMSkillModel
    {
        protected Dictionary<string, float> timers = new();

        public void SetTimer(string _id)
        {
            if(timers.ContainsKey(_id))
            {
                timers[_id] = Time.time;
            }
            else
            {
                timers.Add(_id, Time.time);
            }
        }

        public float CheckTimer(string _id)
        {
            float time;
            if(timers.TryGetValue(_id, out time))
            {
                return time;
            }
            else
            {                
                return 0;
            }
        }
    }
}