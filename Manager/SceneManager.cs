using System;
using System.Collections.Generic;
using System.Text;
using VorticeDirectX_Sample.Scene;

namespace VorticeDirectX_Sample.Manager {
    public class SceneManager {

        public BaseScene CurrentScene {
            get; private set;
        }

        public Dictionary<string, BaseScene> SceneMap {
            get; private set;
        } = new Dictionary<string, BaseScene>();

        public SceneManager() {
        }

        public void Init() {
            SceneMap.Clear();
            AddScene(new DebugScene());

            ChangeScene("DebugScene");
        }

        public void Update() {
            if (CurrentScene != null) {
                CurrentScene.Update();
                CurrentScene.Draw();
            }
        }

        public void AddScene(BaseScene scene) {
            SceneMap.Add(scene.Name, scene);
        }

        public bool ChangeScene(string name) {
            if (SceneMap.ContainsKey(name)) {
                CurrentScene = SceneMap[name];
                CurrentScene.Init();
                return true;
            }
            return false;
        }

    }
}
