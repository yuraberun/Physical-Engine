using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PhysicalEngine.Input
{
    public sealed class PEKeyboard
    {
        private static Lazy<PEKeyboard> LazyInstance = new Lazy<PEKeyboard>(() => new PEKeyboard());

        public static PEKeyboard Instance
        {
            get { return LazyInstance.Value; }
        }

        private KeyboardState currKeyboardState;
        private KeyboardState prevKeyboardState;
        
        public bool IsKeyAvailable
        {
            get { return this.currKeyboardState.GetPressedKeyCount() > 0; }
        }
        
        private PEKeyboard()
        {
            this.currKeyboardState = Keyboard.GetState();
            this.prevKeyboardState = this.currKeyboardState;
        }

        public void Update()
        {
            this.prevKeyboardState = this.currKeyboardState;
            this.currKeyboardState = Keyboard.GetState();
        }

        public bool IsKeyDown(Keys key)
        {
            return this.currKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyClicked(Keys key)
        {
            return this.currKeyboardState.IsKeyDown(key) && !this.prevKeyboardState.IsKeyDown(key);
        }
    }
}