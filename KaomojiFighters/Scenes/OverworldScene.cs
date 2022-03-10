using KaomojiFighters.Mobs;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez.Tiled;

namespace KaomojiFighters.Scenes
{
    class OverworldScene : Scene
    {
        public override void Initialize()
        {
            base.Initialize();
            var map = CreateEntity("Map").AddComponent(new TiledMapRenderer(Content.LoadTiledMap("OwOWorld"))) ;
            map.Transform.SetScale(10f);
            var player = CreateEntity("Player").AddComponent(new WASDMovement()).AddComponent(new Player());
            
            var webCame = FindEntity("camera");
                webCame.AddComponent(new FollowCamera(player.Entity, FollowCamera.CameraStyle.CameraWindow)
                {MapLockEnabled = false, FollowLerp = 0.5F, Camera = webCame.GetComponent<Camera>(), Deadzone = new RectangleF(800f,800f,490f,390f) });
            player.Transform.SetPosition(Screen.Center);
        }
    }
}
