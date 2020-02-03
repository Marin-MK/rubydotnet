using System;

namespace RubyDotNET
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Internal.Initialize();

            Module mRPG = new Module("RPG");
            Table.CreateClass();
            Tone.CreateClass();
            Color.CreateClass();
            AudioFile.CreateClass();
            Map.CreateClass();
            EventCommand.CreateClass();
            MoveCommand.CreateClass();
            MoveRoute.CreateClass();
            Event.CreateClass();
            Page.CreateClass();
            Condition.CreateClass();
            Graphic.CreateClass();

            Animation.CreateClass();
            Frame.CreateClass();
            Timing.CreateClass();

            RPGSystem.CreateClass();
            Words.CreateClass();
            TestBattler.CreateClass();

            Tileset.CreateClass();

            CommonEvent.CreateClass();

            MapInfo.CreateClass();

            Console.ReadKey();
        }
    }
}
