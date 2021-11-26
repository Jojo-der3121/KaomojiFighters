using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace KaomojiFighters.Scenes
{
    class MenuScene : Scene

    {
    public override void Initialize()
    {
        AddRenderer(new DefaultRenderer());
        base.Initialize();
        var Button = new VirtualButton().AddKeyboardKey(Keys.Space);
        Core.Scene = new Battle();

    }

    }
}
