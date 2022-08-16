using System;
using System.Collections.Generic;
using System.Linq;
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

        public uint LastDrawTick = 0;
        
        public bool Invalidated { get; set; }
        public readonly ObservableList<Element> Childs;

        public abstract Element Parent { get; set; }
        
        public abstract Renderer Renderer { get; set; }
        public abstract INative Texture { get; set; }

        public NativeStruct<SDL_Rect> TextureCopyArea { get; set; } = null;

        public Point ParentLocation
        {
            get => new Point(ScreenLocation.X - (Parent?.ScreenLocation.X ?? 0), ScreenLocation.Y - (Parent?.ScreenLocation.Y ?? 0));
            set
            {
                ScreenLocation = new Point(Parent.ScreenLocation.X + value.X, Parent.ScreenLocation.Y + value.Y);
                Invalidated = true;
            }
        }
        
        Point _ScreenLocation = Point.Zero;
        public Point ScreenLocation
        {
            get => _ScreenLocation;
            set
            {
                _ScreenLocation = value;
                Area.Inner.x = value.X;
                Area.Inner.y = value.Y;
                Invalidated = true;
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
            if (Invalidated || LastDrawTick == 0)
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
                Invalidated = false;

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
    }
}