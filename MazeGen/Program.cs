using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static System.Console;

namespace MazeGen
{
    class Program
    {
        private const int MazeWidth = 10;
        private const int MazeHeight = 5;

        private const int ScreenWidth = MazeWidth * 3;
        private const int ScreenHeight = MazeHeight * 3;

        private static readonly Random Random = new Random();

        static void Main()
        {
            SetWindowSize(ScreenWidth, ScreenHeight);
            SetBufferSize(ScreenWidth, ScreenHeight);
            CursorVisible = false;

            while (true)
            {
                Clear();

                List<Room> rooms = new List<Room>();
                Stack<Room> roomStack = new Stack<Room>();

                var initialRoom = new Room { X = 0, Y = 0 };

                roomStack.Push(initialRoom);
                rooms.Add(initialRoom);

                while (rooms.Count < MazeHeight * MazeWidth)
                {
                    Room currentRoom = roomStack.Peek();

                    List<Direction> availableDirections = new List<Direction>();

                    if (currentRoom.X != 0 && !rooms.Any(r => r.X == currentRoom.X - 1 && r.Y == currentRoom.Y))
                        availableDirections.Add(Direction.Left);
                    if (currentRoom.X != MazeWidth - 1 && !rooms.Any(r => r.X == currentRoom.X + 1 && r.Y == currentRoom.Y))
                        availableDirections.Add(Direction.Right);
                    if (currentRoom.Y != 0 && !rooms.Any(r => r.X == currentRoom.X && r.Y == currentRoom.Y - 1))
                        availableDirections.Add(Direction.Up);
                    if (currentRoom.Y != MazeHeight - 1 && !rooms.Any(r => r.X == currentRoom.X && r.Y == currentRoom.Y + 1))
                        availableDirections.Add(Direction.Down);

                    if (availableDirections.Count > 0)
                    {
                        Direction newDirection = availableDirections[Random.Next(0, availableDirections.Count)];

                        if (availableDirections.Contains(Direction.Right))
                        {
                            if(Random.Next(0, 11) < 5)
                                newDirection = Direction.Right;
                        }

                        var newRoom = new Room();

                        switch (newDirection)
                        {
                            case Direction.Right:
                                newRoom.X = currentRoom.X + 1;
                                newRoom.Y = currentRoom.Y;
                                newRoom.Links.Add(Direction.Left);
                                currentRoom.Links.Add(Direction.Right);
                                break;
                            case Direction.Left:
                                newRoom.X = currentRoom.X - 1;
                                newRoom.Y = currentRoom.Y;
                                newRoom.Links.Add(Direction.Right);
                                currentRoom.Links.Add(Direction.Left);
                                break;
                            case Direction.Up:
                                newRoom.X = currentRoom.X;
                                newRoom.Y = currentRoom.Y - 1;
                                newRoom.Links.Add(Direction.Down);
                                currentRoom.Links.Add(Direction.Up);
                                break;
                            case Direction.Down:
                                newRoom.X = currentRoom.X;
                                newRoom.Y = currentRoom.Y + 1;
                                newRoom.Links.Add(Direction.Up);
                                currentRoom.Links.Add(Direction.Down);
                                break;
                        }

                        roomStack.Push(newRoom);
                        rooms.Add(newRoom);
                    }
                    else
                    {
                        roomStack.Pop();
                    }

                    Thread.Sleep(300);
                    Clear();

                    ForegroundColor = ConsoleColor.DarkBlue;
                    for (int y = 0; y < ScreenHeight; y++)
                    {
                        for (int x = 0; x < ScreenWidth; x++)
                        {
                            SetCursorPosition(x, y);
                            Write('X');
                        }
                    }

                    foreach (Room room in rooms)
                    {
                        if (!room.Links.Contains(Direction.Right))
                        {
                            ForegroundColor = ConsoleColor.Cyan;

                            for (int i = 0; i < 3; i++)
                            {
                                SetCursorPosition(room.X * 3 + 2, room.Y * 3 + i);
                                Write('█');
                            }
                        }

                        if (!room.Links.Contains(Direction.Down))
                        {
                            ForegroundColor = ConsoleColor.Cyan;

                            for (int i = room.X == 0 ? 0 : -1; i < 3; i++)
                            {
                                SetCursorPosition(room.X * 3 + i, room.Y * 3 + 2);
                                Write('█');
                            }
                        }

                        Room cursor = roomStack.Peek();

                        ForegroundColor = ConsoleColor.Green;
                        for (int x = 0; x < 2; x++)
                        {
                            for (int y = 0; y < 2; y++)
                            {
                                SetCursorPosition(cursor.X * 3 + x, cursor.Y * 3 + y);
                                Write('█');
                            }
                        }
                    }
                }

                ReadKey();
            }
        }
    }

    class Room
    {
        public int X { get; set; }

        public int Y { get; set; }

        public List<Direction> Links { get; set; } = new List<Direction>();
    }

    enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }
}
