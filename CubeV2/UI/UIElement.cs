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

        public bool Enabled => _alwaysEnabled || _isEnabled();
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

        public bool Clickable { get; private set; } = false;
        private event Action<UserInput> OnLeftClick;
        private event Action<UserInput> OnRightClick;

        public Vector2 Offset;
        private Vector2 Position;

        public Appearance Appearance;

        private List<UIElement> _children = new List<UIElement>();

        public UIElement(string id)
        {
            ID = id;
            AllUIElements.AddUIElement(this, id);
        }
        public void AddChildren(params UIElement[] children) => AddChildren(children.ToList());

        public void AddChildren(List<UIElement> children)
        {
            //TODO: ehehehehe...
            if(!_alwaysEnabled)
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
                Position = Offset + parentOffset;
                Appearance.Draw(spriteBatch, Position);

                foreach (var child in _children)
                {
                    child.Draw(spriteBatch, parentOffset + Offset);
                }
            }
        }

        public void AddLeftClickAction(Action<UserInput> action)
        {
            Clickable = true;
            OnLeftClick += action;
        }
        public void AddRightClickAction(Action<UserInput> action)
        {
            Clickable = true;
            OnRightClick += action;
        }

        public void TryLeftClick(UserInput input)
        {
            if (IsMouseOver(input.MousePos))
            {
                OnLeftClick?.Invoke(input);
            }
        }
        public void TryRightClick(UserInput input)
        {
            if (IsMouseOver(input.MousePos))
            {
                OnRightClick?.Invoke(input);
            }
        }

        public bool IsMouseOver(Vector2 mousePos)
        {
            if(Enabled)
            {
                var size = Appearance.Size;
                return new Rectangle((int)Position.X, (int)Position.Y, (int)size.X, (int)size.Y).Contains(mousePos);
            }

            return false;
        }
    }
}
