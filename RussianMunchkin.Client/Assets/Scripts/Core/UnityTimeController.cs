using RussianMunchkin.Common.Time;
using UnityEngine;

namespace Core
{
    public class UnityTimeController: TimeController
    {
        public void Update()
        {
            UpdateTime((long)(Time.deltaTime * 1000));
        }
    }
}