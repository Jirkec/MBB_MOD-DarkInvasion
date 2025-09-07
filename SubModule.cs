using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using System;
using HarmonyLib;

namespace Bannerlord.DarkNightInvasion
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            new Harmony("army.size").PatchAll();
        }
    }

    public class DebugClass
    {
        public static void HelloWord()
        {
            //output
            Console.WriteLine("Hello World!");

            //debug output
            Debug.Print("Hello World!");

            //in game output
            Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Message",
            new TextObject("Message", null),
            9990,
            () => { InformationManager.DisplayMessage(new InformationMessage("Hello World!")); },
            () => { return (false, null); }));
        }
    }
}