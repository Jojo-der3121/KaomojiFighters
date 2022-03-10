using KaomojiFighters.Mobs;
using Nez;
using Microsoft.Xna.Framework;
using KaomojiFighters.Scenes.OwOWorld;
using KaomojiFighters.Enums;

namespace KaomojiFighters.Scenes
{
    class OverworldScene : Scene
    {
        public override void Initialize()
        {
            base.Initialize();
            var actualTiledMap = Content.LoadTiledMap("OwOWorld");
            var actualLocationofStart = actualTiledMap.ObjectGroups["Objektebene 1"].Objects["Start"];
            var map = CreateEntity("Map").AddComponent(new TiledMapRenderer(actualTiledMap)) ;
            map.Transform.SetScale(10f);
            var player = CreateEntity("Player", new Vector2(actualLocationofStart.X, actualLocationofStart.Y) * 10).AddComponent(new WASDMovement()).AddComponent(new Player());
            var webCame = FindEntity("camera");
                webCame.AddComponent(new FollowCamera(player.Entity, FollowCamera.CameraStyle.CameraWindow)
                {MapLockEnabled = false, FollowLerp = 0.5F, Camera = webCame.GetComponent<Camera>(), Deadzone = new RectangleF((1920f-490f)/2, (1080f-390)/2, 490f, 390f) });
            foreach (var element in actualTiledMap.ObjectGroups["Objektebene 1"].Objects)
            {
                if(element.Type == "Battle")
                {
                    map.AddComponent(new OwOWorldTrigger((int)element.Width, (int)element.Height) { owoWorldTriggerType = OwOWOrldTriggerTypes.battle}).SetLocalOffset(new Vector2(element.X, element.Y));
                }
            }

        }
    }
}
