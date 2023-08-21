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

        public Appearance Appearance { get; protected set; } = Appearance.NoAppearance;

        public bool HasMouseClickEvent { get; private set; } = false;
        public bool HasMousePressEvent { get; private set; } = false;

        public bool Typeable { get; private set; } = false;

        public bool Enabled => !_forceDisable && (_alwaysEnabled || _checkEnabledConditions());
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

        private event Action<UserInput> _onLeftMousePressed;
        private event Action<UserInput> _onRightMousePressed;

        private event Action<UserInput> _onLeftClick;
        private event Action<UserInput> _onRightClick;



        public void AddLeftClickAction(Action<UserInput> action)
        {
            HasMouseClickEvent = true;
            _onLeftClick += action;
        }
        public void AddRightClickAction(Action<UserInput> action)
        {
            HasMouseClickEvent = true;
            _onRightClick += action;
        }

        public void AddLeftMousePressedAction(Action<UserInput> action)
        {
            HasMousePressEvent = true;
            _onLeftMousePressed += action;
        }
        public void AddRightMousePressedAction(Action<UserInput> action)
        {
            HasMousePressEvent = true;
            _onRightMousePressed += action;
        }

        public void AddKeyPressedAction(Action<UserInput> action)
        {
            Typeable = true;
            _onKeyPressed += action;
        }

        
        
        public void LeftClick(UserInput input)
        {
            /*if(input.MousePos.X >= 500 && _position.X >= 500 && _position.Y <= 50)
            {
                Console.WriteLine("Mouse is over grid.");
            }*/

            _onLeftClick?.Invoke(input);            
        }
        public void RightClick(UserInput input)
        {
            _onRightClick?.Invoke(input);
        }
        public void PressLeft(UserInput input)
        {
            _onLeftMousePressed?.Invoke(input);
        }
        public void PressRight(UserInput input)
        {
            _onRightMousePressed?.Invoke(input);
        }

        public void SendKeys(UserInput input)
        {
            _onKeyPressed?.Invoke(input);
        }

        public List<Func<bool>> _enabledConditions = new List<Func<bool>>();
        private bool _alwaysEnabled = true;
        public bool _forceDisable = false;

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
            if(_hasChildren)
            {
                foreach (var child in _children)
                {
                    child.AddEnabledCondition(condition);
                }
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

        private List<UIElement> _children;
        private bool _hasChildren = false;
        public void AddChildren(params UIElement[] children) => AddChildren(children.ToList());
        public void AddChildren(List<UIElement> children)
        {
            if(!_hasChildren)
            {
                _children = new List<UIElement>();
                _hasChildren = true;
            }

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


        public virtual void Draw(SpriteBatch spriteBatch, Vector2 parentOffset, GameTime gameTime)
        {
            if(Enabled)
            {
                _position = _offset + parentOffset;
                Appearance.Draw(spriteBatch, _position,gameTime);

                if(_hasChildren)
                {
                    foreach (var child in _children)
                    {
                        child.Draw(spriteBatch, parentOffset + _offset, gameTime);
                    }
                }
            }
        }

        public bool MouseOver;

        public virtual void CheckMouseOver(Vector2 mousePos)
        {
            if(Enabled)
            {
                MouseOver = UserInput.IsMouseInArea(_position, Appearance.Size, mousePos);
                return;
            }

            MouseOver = false;
        }


        public override string ToString()
        {
            return ID;
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
