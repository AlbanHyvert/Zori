using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemSelector : MonoBehaviour
{
    [SerializeField] private Buttons[] _buttons = new Buttons[4];

    [System.Serializable]
    public struct Buttons
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private Image icon;

        private obj_Item item;

        public obj_Item Item
            => item;

        public obj_Item SetItem(obj_Item newItem)
        {
            item = newItem;
            
            name.text = item.ReturnName();
            //Set Icon

            button.enabled = true;

            return item;
        }
    
        public void Clear()
        {
            item = null;
            
            name.text = "- - - -";
            //Set Icon

            button.enabled = false;
        }
    }

    public Buttons[] ActionBtn
        => _buttons;
}
