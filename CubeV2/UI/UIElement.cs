using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    public class UIElement
    {
        public string ID { get; }

        public Appearance Appearance { get; private set; } = Appearance.NoAppearance;
        public bool Clickable { get; private set; } = false;
        public bool Enabled => _alwaysEnabled || _isEnabled();

        public UIElement(string id)
        {
            ID = id;
            AllUIElements.AddUIElement(this, id);
        }



        private event Action<UserInput> _onLeftClick;
        private event Action<UserInput> _onRightClick;
        public void AddLeftClickAction(Action<UserInput> action)
        {
            Clickable = true;
            _onLeftClick += action;
        }
        public void AddRightClickAction(Action<UserInput> action)
        {
            Clickable = true;
            _onRightClick += action;
        }
        public void TryLeftClick(UserInput input)
        {
            if (IsMouseOver(input.MousePos))
            {
                _onLeftClick?.Invoke(input);
            }
        }
        public void TryRightClick(UserInput input)
        {
            if (IsMouseOver(input.MousePos))
            {
                _onRightClick?.Invoke(input);
            }
        }

        public Func<bool> _isEnabled = () => true;
        private bool _alwaysEnabled = true;
        public void SetEnabledCondition(Func<bool> condition)
        {
            _alwaysEnabled = false;
            _isEnabled = condition;

            //TODO: uhmmmmm this is bad
            foreach (var child in _children)
            {
                child.SetEnabledCondition(_isEnabled);
            }
        }

        public void AddAppearance(Appearance toAppend)
        {
            if (Appearance == null || (Appearance is NoAppearance))
            {
                Appearance = toAppend;
            }
            else if (Appearance.GetType().IsSubclassOf(typeof(MultiAppearance)))
            {
                ((MultiAppearance)Appearance).Appearances.Add(toAppend);
            }
            else
            {
                var oldAppearance = Appearance;
                Appearance = MultiAppearance.Create(oldAppearance, toAppend);
            }
        }
        public void AddAppearances(params Appearance[] toAppend)
        {
            foreach (var appearance in toAppend)
            {
                AddAppearance(appearance);
            }
        }

        private Vector2 _position;
        private Vector2 _offset;
        public void SetOffset(Vector2 offset) => _offset = offset;
        public void SetOffset(float width, float height) => SetOffset(new Vector2(width, height));

        private List<UIElement> _children = new List<UIElement>();
        public void AddChildren(params UIElement[] children) => AddChildren(children.ToList());
        public void AddChildren(List<UIElement> children)
        {
            //TODO: ehehehehe...
            if (!_alwaysEnabled)
            {
                foreach (var child in children)
                {
                    child.SetEnabledCondition(_isEnabled);
                }
            }

            _children.AddRange(children);
        }


        public virtual void Draw(SpriteBatch spriteBatch, Vector2 parentOffset)
        {
            if(Enabled)
            {
                _position = _offset + parentOffset;
                Appearance.Draw(spriteBatch, _position);

                foreach (var child in _children)
                {
                    child.Draw(spriteBatch, parentOffset + _offset);
                }
            }
        }

        public bool IsMouseOver(Vector2 mousePos)
        {
            if(Enabled)
            {
                var size = Appearance.Size;
                return new Rectangle((int)_position.X, (int)_position.Y, (int)size.X, (int)size.Y).Contains(mousePos);
            }

            return false;
        }
    }

    public class UIElementMaker
    {
        public static UIElement MakeRectangle(string id, Vector2 size, Vector2 offset, Color color, float layer)
        {
            var element = new UIElement(id);
            element.AddAppearance(new RectangleAppearance(size, color, layer));
            element.SetOffset(offset);

            return element;
        }

    }
}
