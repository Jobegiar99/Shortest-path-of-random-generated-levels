using System;
using System.Collections.Generic;

    class AStar
    {
        public List<List<int>> levelInformation;
        public AStar(List<List<int>> matrix, Point start, Point goal, bool euclidean)
        {
            List<List<int>> levelInfo = new List<List<int>>();
            this.levelInformation = levelInfo;
            for (int i = 0; i < matrix.Count; i++)
            {
                List<int> test = new List<int>();
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    test.Add(matrix[i][j]);
                }
                levelInfo.Add(test);
            }

            List<Path> open = new List<Path>();
            List<Path> closed = new List<Path>();
            Path path = new Path(new NodeInfo(start, 0, 0), null);
            open.Add(path);

            while (open.Count > 0)
            {
                Path smallest = GetSmallest(ref open);
                path = smallest;

                if (smallest.current.point.Equals(goal))
                {

                    while (path != null)
                    {

                        levelInfo[path.current.point.row][path.current.point.column] = 2;
                        path = path.previous;
                    }
                    this.levelInformation = levelInfo;
                    return;
                }
                else
                {
                    AddNodes(levelInfo, ref open, ref closed, smallest, goal, euclidean);
                    closed.Add(smallest);
                }

            }
        }

        private static void PrintLevel(List<List<int>> matrix, bool clear)
        {
            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    string initial = ((j == 0) ? "|" : "");
                    string closure = ((j == matrix[i].Count - 1) ? "|" : "");
                    string comma = ((j == matrix[i].Count - 1) ? "" : ",");
                    string currentCell = initial + ((matrix[i][j] == 0) ? " " : (matrix[i][j] == 1) ? "x" : "-") + closure;
                    Console.Write(currentCell);
                }
                Console.WriteLine();
            }

            if (clear)
            {
                System.Threading.Thread.Sleep(50);
                Console.Clear();
            }
        }

        private static float  Heuristic(Point nextMove, Point goal, bool euclidean)
        {
            if (euclidean)
            {
                return (float) Math.Sqrt(Math.Pow((nextMove.row - goal.row), 2) + Math.Pow((nextMove.column - goal.column), 2));
            }
            else
            {
                return (float) Math.Abs(nextMove.row - goal.row) + Math.Abs(nextMove.column - goal.column);
            }
        }

        private static void AddNodes(List<List<int>> levelInfo, ref List<Path> open, ref List<Path> closed, Path smallest, Point goal, bool euclidean)
        {

            Point up = new Point(smallest.current.point.row - 1, smallest.current.point.column);
            Point down = new Point(smallest.current.point.row + 1, smallest.current.point.column);
            Point left = new Point(smallest.current.point.row, smallest.current.point.column - 1);
            Point right = new Point(smallest.current.point.row, smallest.current.point.column + 1);
            List<Point> moves = new List<Point> { up, down, left, right };

            for (int i = 0; i < moves.Count; i++)
            {

                if (moves[i].row >= 0 && moves[i].row < levelInfo.Count &&
                    moves[i].column >= 0 && moves[i].column < levelInfo[0].Count &&
                    levelInfo[moves[i].row][moves[i].column] == 0)
                {


                    if (CheckIfPointInList(open, moves[i]))
                    {

                        continue;
                    }
                    else if (CheckIfPointInList(closed, moves[i]))
                    {
                        RemoveFromClosedList(ref closed, ref open, moves[i], smallest);
                    }
                    else
                    {

                        NodeInfo tempNodeInfo = new NodeInfo(moves[i], Heuristic(moves[i], goal, euclidean), smallest.current.gValue + 1);
                        Path tempPath = new Path(tempNodeInfo, smallest);
                        open.Add(tempPath);
                    }
                }
            }
        }

        private static void RemoveFromClosedList(ref List<Path> closed, ref List<Path> open, Point move, Path smallest)
        {

            for (int i = 0; i < closed.Count; i++)
            {
                if (move.Equals(closed[i].current.point))
                {
                    if (closed[i].current.gValue > smallest.current.gValue)
                    {
                        open.Add(closed[i]);
                        closed.Remove(closed[i]);
                    }
                    return;
                }
            }

        }

        private static Path GetSmallest(ref List<Path> open)
        {
            Path smallest = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].current.fValue < smallest.current.fValue)
                    smallest = open[i];
            }
            open.Remove(smallest);
            return smallest;
        }

        public static bool CheckIfPointInList(List<Path> list, Point point)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].current.point.row == point.row && list[i].current.point.column == point.column)
                    return true;
            }
            return false;
        }

    }