using System.Collections.Generic;
using System.Runtime.InteropServices;
using KaomojiFighters.Mobs;
using Nez;
using Microsoft.Xna.Framework;
using KaomojiFighters.Scenes.OwOWorld;
using KaomojiFighters.Enums;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Nez.Sprites;
using System;

namespace KaomojiFighters.Scenes
{
    class OverworldScene : Scene
    {
        int WorldScale = 5;
        public Dictionary<Vector2,DialogComponent> dialogNPCs = new Dictionary<Vector2, DialogComponent>();

        public OverworldScene(Vector2 priorPlayerPosition = default) : base()
        {
            base.Initialize();
            AddRenderer(new RenderLayerExcludeRenderer(0, 4));
            AddRenderer(new ScreenSpaceRenderer(0, 4));
            var actualTiledMap = Content.LoadTiledMap("OwOWorld");
            var actualLocationofStart = actualTiledMap.ObjectGroups["Objektebene 1"].Objects["Start"];
            var map = CreateEntity("Map").AddComponent(new TiledMapRenderer(actualTiledMap));
            map.RenderLayer = 3;
            map.Transform.SetScale(WorldScale);
            var player = CreateEntity("Koamoji01", priorPlayerPosition != Vector2.Zero ? priorPlayerPosition : new Vector2(actualLocationofStart.X, actualLocationofStart.Y) * WorldScale).AddComponent(new OwOWorldPlayer());
            var webCame = FindEntity("camera");
            var followCame = webCame.AddComponent(new FollowCamera(player.Entity, FollowCamera.CameraStyle.CameraWindow)
            { MapLockEnabled = true, FollowLerp = 0.5F, Camera = webCame.GetComponent<Camera>(), Deadzone = new RectangleF((1920f - 490f) / 2, (1080f - 390) / 2, 490f, 390f) });
            followCame.MapSize = new Vector2(map.Width * WorldScale, map.Height * WorldScale);
            foreach (var element in actualTiledMap.ObjectGroups["Objektebene 1"].Objects)
            {
                switch (element.Type)
                {
                    case "Battle":
                        if (element.Name == "Boss" && SafeFileLoader.GetOwOWorldSafeFile()[0] == 1) map.AddComponent(new OwOWorldTrigger((int)element.Width, (int)element.Height) { owoWorldTriggerType = OwOWOrldTriggerTypes.Bossbattle }).SetLocalOffset(new Vector2(element.X, element.Y)); 
                        else map.AddComponent(new OwOWorldTrigger((int)element.Width, (int)element.Height) { owoWorldTriggerType = OwOWOrldTriggerTypes.battle }).SetLocalOffset(new Vector2(element.X, element.Y));
                        break;
                    case "Goal":
                        map.AddComponent(new OwOWorldTrigger((int)element.Width, (int)element.Height) { owoWorldTriggerType = OwOWOrldTriggerTypes.Goal }).SetLocalOffset(new Vector2(element.X, element.Y));
                        break;
                    case "Shop":
                        map.AddComponent(new OwOWorldTrigger((int)element.Width, (int)element.Height) { owoWorldTriggerType = OwOWOrldTriggerTypes.Shop }).SetLocalOffset(new Vector2(element.X, element.Y));
                        map.AddComponent(new SpriteRenderer(Content.LoadTexture("ShopShroom"))).SetLocalOffset(new Vector2(element.X + element.Width / 2, element.Y + element.Height) * WorldScale).SetRenderLayer(-10);
                        break;
                    case "Dialog":
                        map.AddComponent(new OwOWorldTrigger((int)element.Width, (int)element.Height) { owoWorldTriggerType = OwOWOrldTriggerTypes.Dialog }).SetLocalOffset(new Vector2(element.X, element.Y));
                        dialogNPCs.Add( new Vector2(element.X,element.Y) ,map.AddComponent(new DialogComponent(11, player,GetListOfSpeech(element.X,element.Y) , element, WorldScale)));
                        dialogNPCs[new Vector2(element.X, element.Y)].Enabled = false;
                        break;
                    case "LocationSafer":
                        map.AddComponent(new OwOWorldTrigger((int)element.Width, (int)element.Height) { owoWorldTriggerType = OwOWOrldTriggerTypes.LocationSafer }).SetLocalOffset(new Vector2(element.X, element.Y));
                        break;
                    case "Alchemy":
                        map.AddComponent(new OwOWorldTrigger((int)element.Width, (int)element.Height) { owoWorldTriggerType = OwOWOrldTriggerTypes.Alchemy }).SetLocalOffset(new Vector2(element.X, element.Y));
                        map.AddComponent(new SpriteRenderer(Content.LoadTexture("AlchemyShroom"))).SetLocalOffset(new Vector2(element.X + element.Width / 2, element.Y + element.Height) * WorldScale).SetRenderLayer(-10);
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


        public override void OnStart()
        {
            base.OnStart();
            MediaPlayer.Play(Content.Load<Song>("The Polish Ambassador - Digital Loveless (online-audio-converter.com)"));
        }

        public override void Unload()
        {
            MediaPlayer.Stop();
            base.Unload();
        }

        private List<Dialog> GetListOfSpeech(float x, float y)
        {
            var locationOfNPC = new Vector2(x, y);
            var battleNPC = new Vector2(258.834f, 89.6663f);
            var introNPC = new Vector2(268f, 208) ;
            var shopNPC = new Vector2(407.167f, 374.666f) ;

            if (locationOfNPC == battleNPC)
            {
                return new List<Dialog>
                {
                    new Dialog("Now to advanced Warfare and Combat prowess. Each battle is segmented into turns. During a battle you'll have to manage a energy resource. They are represented as blue stars on the left side. You'll also see 3 Options: Attack, Items and Samstag. You can switch between these with the A and D Keys. You can select an Option with space.", 1, 1),
                    new Dialog("If you'll regret your decision just press Back to return to the main selection. the Attack Option is the most complicated so we'll cover them last.If you press Samstag you'll commit suicide. Obviously you wont get any rewards from it. But you have a new chance to show your opponent who's boss when you walk in to him again.", 3, 1),
                    new Dialog("Items is your Items Menu. You use W,A,S and D to maneuver it. pressing Space will use the Item. After you use an item it will be perminantly lost. So be careful.Each Item has two relevant features. Their cost and their ability. there are two types of costs Positiv and Negative costs. Negativ Costs are represented by red stars, they are subtracted from your energy once you use the Item. ",1,1),
                    new Dialog(" Positiv Costs are represented by blue stars. If you use the item you'll gain the energy stars.The ability of the item you use is permanent, meaning they are not automatically removed after a turn. Now to the real meat and Potats tho! the Attack.  ",3,3),
                    new Dialog("In the bottom left you should see a tiny display with a number on it. This is your word deck. like in a Card game. each turn you'll draw 5 new words its your job to form an insult with them in order to defeat your opponent. use W and S to go to a word o your liking and press Space to select it. Basic grammer must be respected tho. And be careful not to confuse verbs with nomen. even though in our language most words can be both. ",2,1),
                    new Dialog(" We here in the OwO-World strongly discriminate against those practices (this is totally not just a schema for forcing you into getting more words). Words function just like Items but unlike items you can overthink your decision by clicking back which emptys your sentence giving you a chance to form a new one with the same words.After you have assembled",1,4),
                    new Dialog(" your Sentence press Enter to attack your opponent with it! Please be careful to have a correct ending for your sentence or else you'll get punished. That also happens when you use bad grammar btw. If you're for some reason unable to form an Attack just press B in the Attack Option to end your turn. You'll also get punished for doing that tho. Looking forward to it.",2,3),
                    new Dialog(" after you end your turn, all five cards will go to the Graveyard. The little display at the bottom right. but do not worry if your deck is empty all cards in your graveyard will be your new deck.first player to have 0 HP loses, HP is the Green Bar at the Top. Aand that would be everything! An iteration of me is hiding somewhere in the area try to find me and defeat me! I'll reward you with Gold and even one of my Words if you do",2,1),
                };
            }
            if(locationOfNPC == introNPC)
            {
                return new List<Dialog>
                {
                    new Dialog(" Hello there and welcome to the vastness of the Internet! We, as in you and me are currently on the most splendit world of all: the OwO-World!! and i am your splendit guide NPC No.2, but you can call me Nano. I'll be your best friend and Companion in need! but also your worst nightmare if you don't need me. Press Back to skipp me and Space to Continue.",1, 1),
                    new Dialog("But now Lets Talk Business. As you already know you move using W, A, S, D. But what you maybe did not know already is that you can open your inventory with I. Further more, if you follow the way upwards you'll find another Iteration of me there i'll teach you how to fight. If you go down you'll get to the Shopping Area where you guessed it. Another Nano will wait for you", 1, 3),
                    new Dialog("He'll teach you the best tips and tricks to get the offers. If you ever have trouble remembering anything. just go to the same Nano again and i'll \"gladly\" explain it again. ..",1,1),
                    new Dialog("which is to say, my Creator was to lazy to code me with multiple line of Dialog... Anyhow have fun!!",4,1)
                };
            } 
            if(locationOfNPC == shopNPC)
            {
                return new List<Dialog>
                {
                    new Dialog("Glad to see YOUR face here again. Coming often to the Mall ? Anyhow we here in the OwO-World have two kinds of shops. normal shops where you can buy items for gold. And the Alchemy Steeples where you can transform old useless words into items. but be carefull. those words are unredeemable.", 1, 4),
                    new Dialog("The left one is the normal shop so lets start with it! you'll have the option to buy or sell the four basic types of items( HP, ATK, DEF, and SPD) from there. You use A and D to switch between the Menu Options and press Space to select on. then you can use W and S to put a copy of the item in your shopping car. select as many as you want! it would make the owner happy.. poor guy deserves a brake", 4, 3),
                    new Dialog("You can see the prices of the items as Gold lettering next to them. Youre total Gold is gonna be shown at the top of the screen a little to the right, but you'll see. If your unhappy with your choice just press X to decrease your shopping list by 1. Pressing enter will buy or sell the items. Btw he is from Germany i hope you don't mind that. We don't have any racists among us well now do we?", 1, 1),
                    new Dialog("But now to our Pride! The Alchemy Steeple!! There you'll have the choice between Alchemy and Generate. Selecting Generate will give you the Option of generating words from a selection of 10 basic words. You'll switch between them using W, A, S, D select with Space, Decrease with X and start the generation with Enter.You can generate as many as you want to. The Steeple owner is so kind that he'll lend you every Material you'll need for the transmutation.", 1, 2),
                    new Dialog("Using the Option Alchemy will allow you to transform your old Words into Alchemical Potions. With the same values as the origin word.you'll switch between the option with W and S and start the Alchemy Process with Space. But be carefull. It cannot be undone. Every word will be permanently lost", 2, 3),
                };
            }
            return new List<Dialog>
            {
                new Dialog(
                    " This is an Error PLS contact Johannes. Hallo, mein Ricardo ist nicht immer hungrig und lustig wenn es kalt und warm ist, aber Kartoffeln lieben ihn trotzdem.",
                    1, 1),
                new Dialog(
                    "Bob der Ross ist sehr traurig wenn die Teekanne Hildegard sagt it ist friego dans Australien pero defenstration.",
                    1, 2),
                new Dialog("Im Wald der Elfen und der Finsternis fehlt das finstere Etwas, weshalb er immer hell ist!",
                    3, 1)
            };
        }


    }
}
