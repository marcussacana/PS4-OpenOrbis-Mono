using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SDL2.Exceptions;
using SDL2.Interface;
using SDL2.Types;
using static SDL2.SDL;

namespace SDL2.Object
{
    public abstract class Element : IDisposable
    {

        #region Proprieties
        private static Dictionary<string, Element> Elements = new Dictionary<string, Element>();

        public readonly string Name;

        public uint LastDrawTick;

        public bool Invalidated { get; set; } = true;

        public readonly ObservableList<Element> Childs;

        public abstract Element Parent { get; set; }

        public abstract Renderer Renderer { get; set; }
        public abstract INative Texture { get; set; }

        public NativeStruct<SDL_Rect> TextureCopyArea { get; set; } = null;

        Point _ParentLocation = Point.Zero;

        /// <summary>
        /// Location of this Element relative to the Parent Element
        /// </summary>
        public Point ParentLocation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ParentLocation;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (_ParentLocation != value)
                {
                    ParentLocation.Set(value.X, value.Y);
                }
            }
        }

        Point _ScreenLocation = Point.Zero;

        /// <summary>
        /// Location of this Element in the Screen
        /// </summary>
        public Point ScreenLocation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ScreenLocation;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (Area.Inner.x != value.X || Area.Inner.y != value.Y)
                {
                    _ScreenLocation.Set(value.X, value.Y);
                }
            }
        }

        private Size _Size;
        public Size Size
        {
            get => _Size;
            set
            {
                _Size = value;
                Area.Inner.w = Size.Width;
                Area.Inner.h = Size.Height;
                Invalidated = true;
            }
        }

        /// <summary>
        /// Area in the Screen to draw this element, when manually modified, you must invalidate the Element
        /// </summary>
        public NativeStruct<SDL_Rect> Area { get; private set; }
        
        public bool Visible { get; set; }
        
        #endregion
        
        public Element(string Name)
        {
            this.Name = Name;
            Elements.Add(Name, this);

            Visible = true;
            
            Area = new NativeStruct<SDL_Rect>();
            
            ScreenLocation = Point.Zero;
            ScreenLocation.OnChanged = RefreshScreenLocation;

            ParentLocation = Point.Zero;
            ParentLocation.OnChanged = ApplyParentLocation;

            Size = new Size(0, 0);
            
            Area = new Rectangle(_ScreenLocation, _Size);
            Childs = new ObservableList<Element>(() => { Invalidated = true; });
        }

        

        /// <summary>
        /// Checks if the given element or any of the childs must be redraw
        /// </summary>
        /// <param name="Tick">The current frame tick</param>
        public virtual bool NeedsRedraw(uint Tick)
        {
            if (Invalidated)
                return true;
            
            if (Childs == null)
                return false;

            return Childs.Any(Child => Child.NeedsRedraw(Tick));
        }

        /// <summary>
        /// Draw the element and his childs to the Renderer
        /// </summary>
        /// <param name="Tick">The current frame tick</param>
        public virtual void OnDraw(uint Tick)
        {
            if (Renderer != null && Texture != null)
            {
                LastDrawTick = Tick;

                if (Visible)
                {
                    int Status = SDL_RenderCopy(Renderer.Handler, Texture.Handler, TextureCopyArea, Area);
                    if (Status < 0)
                        throw new SDLException();
                }
            }

            if (Childs == null)
                return;
            
            foreach (var Child in Childs)
            {
                Child.OnDraw(Tick);
            }
        }

        public virtual void Dispose()
        {
            if (Childs != null)
            {
                foreach (var Child in Childs)
                {
                    Child.Dispose();
                }
            }

            TextureCopyArea.Dispose();
            Area.Dispose();

            if (Texture != null)
            {
                SDL_DestroyTexture(Texture.Handler);
                Texture = null;
            }

            if (Elements.ContainsKey(Name))
                Elements.Remove(Name);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void RefreshScreenLocation(int XDelta, int YDelta)
        {
            Area.Inner.x = _ScreenLocation.X;
            Area.Inner.y = _ScreenLocation.Y;
            RefreshParentLocationFromScreen();
            ApplyChildLocationDelta(XDelta, YDelta);
            Invalidated = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void RefreshParentLocationFromScreen()
        {
            if (Parent == null)
                return;

            var ParentScreenLocation = Parent?.ScreenLocation ?? throw new NullReferenceException("Failed to get the parent location");

            _ParentLocation.Set(ScreenLocation.X - ParentScreenLocation.X, ScreenLocation.Y - ParentScreenLocation.Y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ApplyParentLocation(int XDelta, int YDelta)
        {
            ScreenLocation.Sum(XDelta, YDelta);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ApplyChildLocationDelta(int XDelta, int YDelta)
        {
            foreach (var Child in Childs)
            {
                Child.ScreenLocation.Sum(XDelta, YDelta);
            }
        }
    }
}