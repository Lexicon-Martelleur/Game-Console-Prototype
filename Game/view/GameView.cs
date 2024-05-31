
using Game.constants;
using Game.model;

namespace Game.view
{
    internal class GameView
    {
        internal void DrawMap(Map map)
        {
            Console.Clear();
            var currBackground = Console.BackgroundColor;

            
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    Console.BackgroundColor = map.Cells[y, x].Terrain.Color;
                    var artifact = map.Cells[y, x].Artifact;
                    string cellContent = artifact != null ? $" {artifact.Symbol} " : "   ";
                    Console.Write(GetConsistentCellWidth(cellContent));
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = currBackground;
        }


        /// <summary>
        /// Used to truncate or pad the cell content to ensure consistent width
        /// </summary>
        private string GetConsistentCellWidth(string cellContent)
        {
            int cellWidth = 3;

            string consistentCellWidth = "   ";

            if (cellContent.Length > cellWidth)
            {
                consistentCellWidth = cellContent.Substring(0, cellWidth);
            }
            else if (cellContent.Length < cellWidth)
            {
                consistentCellWidth = cellContent.PadRight(cellWidth);
            }

            return consistentCellWidth;
        }


        internal Move GetCommand()
        {
            ConsoleKey key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    return Move.UP;
                case ConsoleKey.RightArrow:
                    return Move.RIGHT;
                case ConsoleKey.DownArrow:
                    return Move.DOWN;
                case ConsoleKey.LeftArrow:
                    return Move.LEFT;
                default:
                    return Move.NONE;
            }
        }
    }
}