﻿using KaomojiFighters.Mobs;
using Nez;
using Microsoft.Xna.Framework;
using KaomojiFighters.Scenes.OwOWorld;
using KaomojiFighters.Enums;
using Nez.Sprites;

namespace KaomojiFighters.Scenes
{
    class OverworldScene : Scene
    {
        int WorldScale = 5;


        public OverworldScene(Vector2 priorPlayerPosition = default) : base()
        {

            base.Initialize();
            var actualTiledMap = Content.LoadTiledMap("OwOWorld");
            var actualLocationofStart = actualTiledMap.ObjectGroups["Objektebene 1"].Objects["Start"];
            var map = CreateEntity("Map").AddComponent(new TiledMapRenderer(actualTiledMap));
            map.RenderLayer = 3;
            map.Transform.SetScale(WorldScale);
            var player = CreateEntity("Koamoji01", priorPlayerPosition != Vector2.Zero ? priorPlayerPosition : new Vector2(actualLocationofStart.X, actualLocationofStart.Y) * WorldScale).AddComponent(new WASDMovement()).AddComponent(new Player());
            var webCame = FindEntity("camera");
            webCame.AddComponent(new FollowCamera(player.Entity, FollowCamera.CameraStyle.CameraWindow)
            { MapLockEnabled = false, FollowLerp = 0.5F, Camera = webCame.GetComponent<Camera>(), Deadzone = new RectangleF((1920f - 490f) / 2, (1080f - 390) / 2, 490f, 390f) });
            foreach (var element in actualTiledMap.ObjectGroups["Objektebene 1"].Objects)
            {
                switch (element.Type)
                {
                    case "Battle":
                        map.AddComponent(new OwOWorldTrigger((int)element.Width, (int)element.Height) { owoWorldTriggerType = OwOWOrldTriggerTypes.battle }).SetLocalOffset(new Vector2(element.X, element.Y));
                        break;
                    case "Goal":
                        map.AddComponent(new OwOWorldTrigger((int)element.Width, (int)element.Height) { owoWorldTriggerType = OwOWOrldTriggerTypes.Goal }).SetLocalOffset(new Vector2(element.X, element.Y));
                        break;
                    case "Shop":
                        map.AddComponent(new OwOWorldTrigger((int)element.Width, (int)element.Height) { owoWorldTriggerType = OwOWOrldTriggerTypes.Shop }).SetLocalOffset(new Vector2(element.X, element.Y));
                        map.AddComponent(new SpriteRenderer(Content.LoadTexture("ShopShroom"))).SetLocalOffset(new Vector2(element.X + element.Width / 2, element.Y + element.Height) * WorldScale).SetRenderLayer(-10);
                        break;
                    case "Dialog":
                        map.AddComponent(new SpriteRenderer(Content.LoadTexture("Kaomoji02")) { Size = new Vector2(310, 92) / WorldScale }).SetLocalOffset(new Vector2(element.X + element.Width / 2, element.Y + element.Height / 2) * WorldScale);
                        break;
                    default:
                        break;
                }
            }
            foreach (var element in actualTiledMap.ObjectGroups["WallCollisions"].Objects)
            {
                if (element.Type == "Polygon")
                {
                    var outputVector = new Vector2[element.Points.Length];
                    var fickdichMatrixVariable = Matrix.CreateTranslation(element.X, element.Y, 0);
                    Vector2.Transform(element.Points, ref fickdichMatrixVariable, outputVector);
                    var polygonCOllider = map.AddComponent(new PolygonCollider(outputVector));
                }
                else
                {
                    map.AddComponent(new BoxCollider(element.X, element.Y, element.Width, element.Height));
                }
            }


        }


    }
}
