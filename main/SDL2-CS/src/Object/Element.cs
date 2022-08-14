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
        
        private static Dictionary<string, Element> Elements = new Dictionary<string, Element>();
        public readonly string Name;
        
        public Element(string Name)
        {
            this.Name = Name;
            Elements.Add(Name, this);
        }
        
        public virtual IList<Element> Childs { get; } = new List<Element>();

        public abstract Element Parent { get; set; }
        
        public abstract INative Renderer { get; set; }
        public abstract INative Texture { get; set; }

        public NativeStruct<SDL_Rect> TextureCopyArea { get; set; } = null;

        public Point ParentLocation
        {
            get => new Point(ScreenLocation.X - (Parent?.ScreenLocation.X ?? 0), ScreenLocation.Y - (Parent?.ScreenLocation.Y ?? 0));
            set
            {
                ScreenLocation.X = Parent.ScreenLocation.X + value.X;
                ScreenLocation.Y = Parent.ScreenLocation.Y + value.Y;
            }
        }
        public Point ScreenLocation { get; set; } = Point.Zero;
        public virtual Size Size { get; set; } = new Size();
        
        public NativeStruct<SDL_Rect> Area => new Rectangle(ScreenLocation, Size);
        

        /// <summary>
        /// Checks if the given element or any of the childs must be redraw
        /// </summary>
        /// <param name="Tick">The current frame tick</param>
        public virtual bool NeedsRedraw(uint Tick)
        {
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
                int Status = SDL_RenderCopy(Renderer.Handler, Texture.Handler, TextureCopyArea, Area);
                if (Status < 0)
                    throw new SDLException();
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