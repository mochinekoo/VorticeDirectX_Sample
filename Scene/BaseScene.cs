using System;
using System.Collections.Generic;
using System.Text;

namespace VorticeDirectX_Sample.Scene {
    public abstract class BaseScene {

        public string Name {
            get; private set;
        }

        public BaseScene(string name) {
            this.Name = name;
        }

        public abstract void Init();
        public abstract void Update();
        public abstract void Draw();
    }
}
