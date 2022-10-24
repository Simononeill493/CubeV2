using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class AllUIElements
    {
        private static Dictionary<string, UIElement> _uIElements = new Dictionary<string, UIElement>();

        public static void AddUIElement(UIElement element, string id)
        {
            if (_uIElements.ContainsKey(id))
            {
                throw new Exception("Element with id '" + id + "' already exists.");
            }

            _uIElements[id] = element;
        }

        public static IEnumerable<UIElement> GetClickable => _uIElements.Values.Where(u => u.Clickable && u.Enabled);
        public static IEnumerable<UIElement> GetTypeable => _uIElements.Values.Where(u => u.Typeable && u.Enabled);

    }
}
