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
        public void SetManualSize(Vector2 size)
        {
            ((NoAppearance)Appearance).SetManualSize(size);
        }

        public bool Clickable { get; private set; } = false;
        public bool Typeable { get; private set; } = false;

        public bool Enabled => !_alwaysDisabled && (_alwaysEnabled || _checkEnabledConditions());
        private bool _checkEnabledConditions()
        {
            foreach (var condition in _enabledConditions)
            {
                if (!condition())
                {
                    return false;
                }
            }
           return true;
        }

        public UIElement(string id)
        {
            ID = id;
            AllUIElements.AddUIElement(this, id);
        }


        private event Action<UserInput> _onKeyPressed;
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
        public void AddKeyPressedAction(Action<UserInput> action)
        {
            Typeable = true;
            _onKeyPressed += action;
        }

        
        
        public void TryLeftClick(UserInput input)
        {
            /*if(input.MousePos.X >= 500 && _position.X >= 500 && _position.Y <= 50)
            {
                Console.WriteLine("Mouse is over grid.");
            }*/


            if (MouseOver)
            {
  //              Console.WriteLine("Mouse is over");
                _onLeftClick?.Invoke(input);
            }
        }
        public void TryRightClick(UserInput input)
        {
            if (MouseOver)
            {
                _onRightClick?.Invoke(input);
            }
        }
        public void SendKeys(UserInput input)
        {
            _onKeyPressed?.Invoke(input);
        }

        public List<Func<bool>> _enabledConditions = new List<Func<bool>>();
        private bool _alwaysEnabled = true;
        public bool _alwaysDisabled = false;

        public void AddEnabledConditions(List<Func<bool>> conditions)
        {
            foreach(var condition in conditions)
            {
                AddEnabledCondition(condition);
            }
        }
        public void AddEnabledCondition(Func<bool> condition)
        {
            _alwaysEnabled = false;
            _enabledConditions.Add(condition);

            //TODO: uhmmmmm this is bad
            foreach (var child in _children)
            {
                child.AddEnabledCondition(condition);
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
        public void ClearApperances() => Appearance = Appearance.NoAppearance;

        public void AddAppearances(params Appearance[] toAppend)
        {
            foreach (var appearance in toAppend)
            {
                AddAppearance(appearance);
            }
        }

        public Vector2 _position { get; private set; }
        public Vector2 _offset { get; private set; }
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
                    child.AddEnabledConditions(_enabledConditions);
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

        public bool MouseOver;

        public void CheckMouseOver(Vector2 mousePos)
        {
            if(Enabled)
            {
                var size = Appearance.Size;
                MouseOver = new Rectangle((int)_position.X, (int)_position.Y, (int)size.X, (int)size.Y).Contains(mousePos);
                return;
            }

            MouseOver = false;
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
