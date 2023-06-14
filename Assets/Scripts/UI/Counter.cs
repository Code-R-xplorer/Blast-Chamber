using TMPro;
using UnityEngine;

namespace UI
{
    public class Counter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countText;

        public void SetCount(int count)
        {
            countText.text = count.ToString();
        }
    }
}