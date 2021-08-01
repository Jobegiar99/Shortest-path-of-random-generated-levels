using System;
using System.Collections;
using System.Collections.Generic;

class LevelGeneration
{

    public List<List<int>> matrix;
    public Point start;
    public Point end;

    public LevelGeneration()
    {
        matrix = GenerateMatrix();

    }


    public  List<List<int>> GenerateMatrix()
    {

        List<List<int>> matrix = FillMatrix();

        System.Random rand = new System.Random();
        Point start = new Point(matrix.Count - 1, rand.Next(matrix[0].Count));
        Point end = new Point(0, rand.Next(matrix[0].Count));

        matrix = GenerateLevel(matrix, start, end);


        return matrix;

    }

    private static List<List<int>> FillMatrix()
    {
        List<List<int>> matrix = new List<List<int>>();
        Random rand = new Random();
        int matrixSize = rand.Next(30,200);
        int rowSize = rand.Next(30,200);

        for (int i = 0; i < matrixSize; i++)
        {

            List<int> currentRow = new List<int>();

            for (int j = 0; j < rowSize; j++)
            {

                currentRow.Add(1);


            }

            matrix.Add(currentRow);

        }
        return matrix;
    }



    private  List<List<int>> GenerateLevel(List<List<int>> matrix, Point wander, Point seeker)
    {
        Random rand = new Random();
        this.start = wander;
        this.end = seeker;

        List<Point> createdPath = new List<Point>();

        while (!createdPath.Contains(seeker))
        {
            matrix[wander.row][wander.column] = 0;
            matrix[seeker.row][seeker.column] = 0;

            createdPath.Add(wander);
            createdPath.Add(seeker);

            wander = GetWandererNextMove(matrix, createdPath, wander);
            seeker = GetSeekerNextMove(matrix, createdPath, wander, seeker);

        }
        return matrix;

    }

    private static bool Between(int lowlimit, int highlimit, int value)
    {
        return (lowlimit <= value && value <= highlimit);
    }

    private static bool CheckIfValidOption(int low, int high, int value, List<Point> createdPath, Point point)
    {
        return (Between(low, high, value) && !createdPath.Contains(point));
    }

    private  void CheckWandererPossiblePathOptions(ref List<Point> walkablePath,
                                                         ref Queue<Point> pathExploration,
                                                         List<List<int>> matrix,
                                                         Point currentPoint)
    {

        Point up = new Point(currentPoint.row - 1, currentPoint.column);
        Point down = new Point(currentPoint.row + 1, currentPoint.column);
        Point left = new Point(currentPoint.row, currentPoint.column - 1);
        Point right = new Point(currentPoint.row, currentPoint.column + 1);
        List<Point> movementInfo = new List<Point>() { up, down, left, right };

        int matrixSize = matrix.Count - 2;
        int rowSize = matrix[0].Count - 2;
        for (int i = 0; i < 4; i++)
        {
            int value = (i < 2) ? movementInfo[i].row : movementInfo[i].column;
            int size = (i < 2) ? matrixSize : rowSize;
            int row = movementInfo[i].row;
            int col = movementInfo[i].column;

            if (CheckIfValidOption(1, size, value, walkablePath, movementInfo[i]) && matrix[row][col] == 0)
            {
                pathExploration.Enqueue(movementInfo[i]);
                walkablePath.Add(movementInfo[i]);
            }
        }
    }

    private  Point CheckWandererPossibleMoves(List<List<int>> matrix, Point wanderer, List<Point> createdPath)
    {

        Queue<Point> pathExploration = new Queue<Point>();
        List<Point> walkablePath = new List<Point>();
        System.Random rand = new System.Random();

        pathExploration.Enqueue(wanderer);
        while (pathExploration.Count > 0)
        {
            Point point = pathExploration.Dequeue();
            CheckWandererPossiblePathOptions(ref walkablePath, ref pathExploration, matrix, point);
        }

        return walkablePath[rand.Next(walkablePath.Count)];

    }

    private  Point CreateWandererNextMove(List<Point> moves, List<List<int>> matrix, List<Point> createdPath, Point wanderer)
    {

        List<Point> possibleMoves = new List<Point>();
        System.Random rand = new System.Random();

        int matrixSize = matrix.Count - 2;
        int rowSize = matrix[0].Count - 2;
        for (int i = 0; i < 4; i++)
        {
            int value = (i < 2) ? moves[i].row : moves[i].column;
            int size = (i < 2) ? matrixSize : rowSize;
            if (CheckIfValidOption(1, size, value, createdPath, moves[i]))
                possibleMoves.Add(moves[i]);
        }

        //O(1)
        if (possibleMoves.Count > 0)
            return possibleMoves[rand.Next(possibleMoves.Count)];

        //O(w + r)
        return CheckWandererPossibleMoves(matrix, wanderer, createdPath);

    }

    //O(1)
    private  Point GetWandererNextMove(List<List<int>> matrix, List<Point> createdPath, Point pointA)
    {

        Point up = new Point(pointA.row - 1, pointA.column);
        Point down = new Point(pointA.row + 1, pointA.column);
        Point left = new Point(pointA.row, pointA.column - 1);
        Point right = new Point(pointA.row, pointA.column + 1);

        Random rand = new Random();

        List<Point> moves = new List<Point> { up, down, left, right };

        return CreateWandererNextMove(moves, matrix, createdPath, pointA);


    }


    private static int ManhattanDistance(Point first, Point second)
    {
        int rowResult = (int)Math.Abs(first.row - second.row);
        int columnResult = (int)Math.Abs(first.column - second.column);
        return rowResult + columnResult;
    }


    //O(1)
    private static Point CreateSeekerNextMove(List<int> distances, List<Point> moves, List<List<int>> matrix, List<Point> createdPath)
    {
        /*

            each index corresponds to: 
            distances = original, up, down, left , right

            moves = up, down, left, right

        */
        System.Random rand = new System.Random();
        List<Point> possibleMoves = new List<Point>();
        int matrixSize = matrix.Count - 1;
        int rowSize = matrix[0].Count - 1;

        for (int i = 0; i < 4; i++)
        {
            int value = (i < 2) ? moves[i].row : moves[i].column;
            int size = (i < 2) ? matrixSize : rowSize;
            if (CheckIfValidOption(0, size, value, createdPath, moves[i]) && distances[0] >= distances[i + 1])
                possibleMoves.Add(moves[i]);
        }

        if (possibleMoves.Count > 0)
            return possibleMoves[rand.Next(possibleMoves.Count)];

        return new Point(rand.Next(1, matrixSize), rand.Next(1, rowSize));

    }

    //O(1)
    private static Point GetSeekerNextMove(List<List<int>> matrix,
                                 List<Point> createdPath,
                                 Point pointA,
                                 Point pointB)
    {

        Random rand = new Random();

        Point up = new Point(pointB.row - 1, pointB.column);
        Point down = new Point(pointB.row + 1, pointB.column);
        Point left = new Point(pointB.row, pointB.column - 1);
        Point right = new Point(pointB.row, pointB.column + 1);

        int originalDistance = ManhattanDistance(pointA, pointB);
        int upDistance = ManhattanDistance(pointA, up);
        int downDistance = ManhattanDistance(pointA, down);
        int leftDistance = ManhattanDistance(pointA, left);
        int rightDistance = ManhattanDistance(pointA, right);

        List<int> distances = new List<int> { originalDistance, upDistance, downDistance, leftDistance, rightDistance };
        List<Point> moves = new List<Point> { up, down, left, right };
        return CreateSeekerNextMove(distances, moves, matrix, createdPath);

    }

}
