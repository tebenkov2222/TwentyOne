using UnityEngine;
using UnityEngine.UI;

namespace View.Shared
{
    public class FilledImageView: MonoBehaviour
    {
        [SerializeField] private Image _filledImage;
        
        public void UpdateValue(float value)
        {
            //Debug.Log($"FillAmount value = {value}");
            _filledImage.fillAmount = value;
        }
    }
}