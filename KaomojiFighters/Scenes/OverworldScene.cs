using KaomojiFighters.Mobs;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Scenes
{
    class OverworldScene : Scene
    {
        public override void Initialize()
        {
            base.Initialize();
            var Map = CreateEntity("Map").AddComponent(new TiledMapRenderer(Content.LoadTiledMap("OwOWorld"))) ;
            Map.Transform.SetScale(10f);
            var Player = CreateEntity("Player").AddComponent(new WASDMovement()).AddComponent(new Player());
            //.AddComponent(new FollowCamera(FindEntity("Player"), FollowCamera.CameraStyle.CameraWindow));
            Player.Transform.SetPosition(Screen.Center);
        }
    }
}
