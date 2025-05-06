using System.Drawing;

namespace Advent_2020_11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("trueData.txt");
            Dictionary<Point, char> seatMap = new Dictionary<Point, char>();
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    seatMap.Add(new Point(i, j), lines[i][j]);
                }
            }

            int count = 0;
            int oldCount = 0;
            while (true)
            {
                
                seatMap = GetNextSeatmap(seatMap);
                oldCount = count;
                count = CountOccupiedSeats(seatMap);
                if (oldCount == count) break;
                
            }
            Console.WriteLine(count);

        }



        static Dictionary<Point, char> GetNextSeatmap(Dictionary<Point, char> input)
        {
            Dictionary<Point, char> result = new();
            List<Point> directions = new List<Point> {
                new Point(0, -1),
                new Point(0, 1),
                new Point(-1, 0),
                new Point(1, 0),
                new Point(-1, -1),
                new Point(-1, 1),
                new Point(1, -1),
                new Point(1, 1),
            };
            foreach (var point in input.Keys)
            {
                if (input[point]== '.') result.Add(point, '.'); //Floor never changes
                else if (input[point]== 'L') // If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
                {
                    result.Add(point, '#');
                    foreach (var direction in directions)
                    {
                        var adjacentPoint = Add(point, direction); ;
                        if (input.TryGetValue(adjacentPoint, out char adjacentSeat))
                        {
                            if (adjacentSeat == '#')
                            {
                                result[point] = 'L';
                                break;
                            }

                        }
                    }
                }
                else if (input[point] == '#') // If a seat is occupied(#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
                {
                    result.Add(point, '#');
                    int countOccupiedAdjacentSeats = 0;
                    foreach (var direction in directions)
                    {
                        var adjacentPoint = Add(point, direction);
                        if (input.TryGetValue(adjacentPoint, out char adjacentSeat))
                        {
                            if (adjacentSeat == '#')
                            {
                                countOccupiedAdjacentSeats++;
                                if (countOccupiedAdjacentSeats >= 4)
                                {
                                    result[point] = 'L';
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        static int CountOccupiedSeats(Dictionary<Point, char> input)
        {
            int result = 0;
            foreach (var point in input.Keys)
            {
                if (input[point] == '#') { result++; }
            }
            return result;
        }

        static Point Add(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);

    }
}
