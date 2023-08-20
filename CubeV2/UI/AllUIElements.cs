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

        public static UIGameGrid GetGameGrid() => (UIGameGrid)GetUIElement(Config.GameGridName);

        public static void AddUIElement(UIElement element, string id)
        {
            if (_uIElements.ContainsKey(id))
            {
                throw new Exception("Element with id '" + id + "' already exists.");
            }

            _uIElements[id] = element;
        }

        public static UIElement GetUIElement(string id)
        {
            if (!_uIElements.ContainsKey(id))
            {
                throw new Exception("Element with id '" + id + "' does not exist.");
            }

            return _uIElements[id];
        }


        public static IEnumerable<UIElement> GetClickable => _uIElements.Values.Where(u => u.HasMouseClickEvent && u.Enabled);
        public static IEnumerable<UIElement> GetClickableWithMouseOver => _uIElements.Values.Where(u => u.HasMouseClickEvent && u.Enabled && u.MouseOver);

        public static IEnumerable<UIElement> GetPressable => _uIElements.Values.Where(u => u.HasMousePressEvent && u.Enabled);
        public static IEnumerable<UIElement> GetPressableWithMouseOver => _uIElements.Values.Where(u => u.HasMousePressEvent && u.Enabled && u.MouseOver);

        public static IEnumerable<UIElement> GetTypeable => _uIElements.Values.Where(u => u.Typeable && u.Enabled);
        public static IEnumerable<UIElement> GetMouseInteractionElements => _uIElements.Values;

    }
}
