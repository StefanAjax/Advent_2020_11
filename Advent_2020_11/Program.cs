using System.Drawing;

namespace Advent_2020_11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var seatMap = ParseInput("trueData.txt");

            int count = 0;
            int oldCount = 0;
            while (true)
            {
                seatMap = GetNextSeatmapPart1(seatMap);
                oldCount = count;
                count = CountOccupiedSeats(seatMap);
                if (oldCount == count) break;
            }
            Console.WriteLine($"Part 1 Answer: {count}");

            seatMap = ParseInput("trueData.txt");

            count = 0;
            oldCount = 0;
            while (true)
            {
                //PrintSeatmap(seatMap);
                seatMap = GetNextSeatmapPart2(seatMap);
                oldCount = count;
                
                count = CountOccupiedSeats(seatMap);
                if (oldCount == count)
                {
                    PrintSeatmap(seatMap);
                    break;
                }
            }
            Console.WriteLine($"Part 2 Answer: {count}");
        }



        static Dictionary<Point, char> ParseInput(string path)
        {
            var lines = File.ReadAllLines(path);
            Dictionary<Point, char> result = new Dictionary<Point, char>();
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    result.Add(new Point(i, j), lines[i][j]);
                }
            }
            return result;
        }


        static Dictionary<Point, char> GetNextSeatmapPart1(Dictionary<Point, char> input)
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
                if (input[point] == '.') result.Add(point, '.'); //Floor never changes

                else if (input[point] == 'L') // If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
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
                else if (input[point] == '#') // If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
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

        static Dictionary<Point, char> GetNextSeatmapPart2(Dictionary<Point, char> input)
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
                //Console.WriteLine($"Doing point {point.X},{point.Y} with value {input[point]}");

                if (input[point] == '.') {
                    result.Add(point, '.'); //Floor never changes
                    //Console.WriteLine("Floor never changes");
                } 

                else if (input[point] == 'L') // If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
                {
                    result.Add(point, '#');
                    foreach (var direction in directions)
                    {
                        var visiblePoint = Add(point, direction); ;
                        while (input.TryGetValue(visiblePoint, out char visibleChar) && visibleChar == '.'){ 
                            visiblePoint = Add(visiblePoint, direction);
                            //Console.WriteLine($"Moved to {visiblePoint.X}, {visiblePoint.Y}");
                        }
                            
                        // Here the visible point is either a seat or outside of bounds


                        if (input.TryGetValue(visiblePoint, out char visibleSeat))
                        {
                            if (visibleSeat == '#')
                            {
                                result[point] = 'L';
                                break;
                            }
                        }
                    }
                }
                else if (input[point] == '#') // If a seat is occupied (#) and five or more seats adjacent to it are also occupied, the seat becomes empty.
                {
                    result.Add(point, '#');
                    int countOccupiedAdjacentSeats = 0;
                    foreach (var direction in directions)

                    {
                        var visiblePoint = Add(point, direction); ;
                        while (input.TryGetValue(visiblePoint, out char visibleChar) && visibleChar == '.')
                        {
                            visiblePoint = Add(visiblePoint, direction);
                            //Console.WriteLine($"Moved to {visiblePoint.X}, {visiblePoint.Y}");
                        }
                            
                        // Here the visible point is either a seat or outside of bounds

                        if (input.TryGetValue(visiblePoint, out char adjacentSeat))
                        {
                            if (adjacentSeat == '#')
                            {
                                countOccupiedAdjacentSeats++;
                                //Console.WriteLine(countOccupiedAdjacentSeats);
                                if (countOccupiedAdjacentSeats >= 5)
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


        static void PrintSeatmap(Dictionary<Point, char> seatmap)
        {
            // Find the dimensions of the seatmap
            int maxRow = seatmap.Keys.Max(p => p.X);
            int maxCol = seatmap.Keys.Max(p => p.Y);

            // Iterate over each row and column to print the seatmap
            for (int row = 0; row <= maxRow; row++)
            {
                for (int col = 0; col <= maxCol; col++)
                {
                    var point = new Point(row, col);
                    if (seatmap.TryGetValue(point, out char seat))
                    {
                        Console.Write(seat);  // Print the seat character
                    }
                }
                Console.WriteLine();  // Move to the next line after each row
                
            }
            Console.WriteLine();
        }
    }
}
