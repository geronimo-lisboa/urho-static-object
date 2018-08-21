using System;
using System.Diagnostics;
using Urho;
using Urho.Actions;
using Urho.Gui;
using Urho.Shapes;

namespace MyGame
{
    public class MyGame : Application
    {


        [Preserve]
        public MyGame(ApplicationOptions options) : base(options) { }

        static MyGame()
        {
            UnhandledException += (s, e) =>
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                e.Handled = true;
            };
        }
        private Scene scene;
        private Node CameraNode;
        private Camera camera;
        protected override async void Start()
        {
            base.Start();
            Log.LogMessage += e => Debug.WriteLine($"[{e.Level}] {e.Message}");

            var cache = this.ResourceCache;
            //cria a cena
            scene = new Scene(this.Context);
            scene.CreateComponent<Octree>();

            //cria o node do cubo
            var cubeNode = scene.CreateChild("Cube");
            cubeNode.Position = new Vector3(0, 0, 0);
            var cubeObject = cubeNode.CreateComponent<StaticModel>();
            cubeObject.Model = cache.GetModel("Models/Cube.mdl");
            cubeObject.SetMaterial(cache.GetMaterial("Materials/Dice.xml"));
            cubeNode.RunActions(new RepeatForever(new RotateBy(duration: 1f, deltaAngleX:0, deltaAngleY: 10, deltaAngleZ: 0)));

            //cria o node da luz
            var lightNode = scene.CreateChild("DirectionalLight");
            lightNode.SetDirection(new Vector3(0.6f, -1.0f, 0.8f)); // The direction vector does not need to be normalized
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Directional;
            //cria o node da câmera
            CameraNode = scene.CreateChild("camera");
            camera = CameraNode.CreateComponent<Camera>();
            CameraNode.Position = new Vector3(0, 1, -10);
            //Com isso eu tenho uma luz, uma camera e um objeto qqer pendurados na cena

            // Viewport
            var viewport = new Viewport(Context, scene, camera, null);
            viewport.SetClearColor(Color.Yellow);
            Renderer.SetViewport(0, viewport);

            //FPS
            new MonoDebugHud(this).Show(Color.Black, 25);
        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);
        }

    }
}
