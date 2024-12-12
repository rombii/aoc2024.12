var input = (await File.ReadAllLinesAsync(Path.Join(Directory.GetCurrentDirectory(), "input.txt")))
                    .Select(s => s.ToCharArray())
                    .ToArray();


var globalVisited = new HashSet<(int, int)>();
var perimeters = new HashSet<((int, int), (int, int), char)>();
long part1Answer = 0, part2Answer = 0;
for (var i = 0; i < input.Length; i++)
{
    for (var j = 0; j < input[i].Length; j++)
    {
        if(globalVisited.Contains((i, j))) continue;
        var area = Dfs(i, j);
        var filteredPerimeters = FilterPerimeters(perimeters);
        part1Answer += area * perimeters.Count;
        part2Answer += area * filteredPerimeters.Count;
        perimeters.Clear();
    }
}

Console.WriteLine($"First part: {part1Answer}");
Console.WriteLine($"Second part: {part2Answer}");
return;

int Dfs(int i, int j)
{
    var directions = new[] { (-1, 0), (0, 1), (1, 0), (0, -1) };
    var directionsPerimeters = new[] { 'h', 'v', 'h', 'v' };
    var area = 1;
    globalVisited.Add((i, j));
    for (var ii = 0; ii < directions.Length; ii++)
    {
        var newI = i + directions[ii].Item1;
        var newJ = j + directions[ii].Item2;
        if (newI < 0 || newI >= input.Length || newJ < 0 || newJ >= input.Length)
        {
            perimeters.Add(((i, j), (newI, newJ), directionsPerimeters[ii]));
        }
        else if (globalVisited.Contains((newI, newJ)))
        {
            if (input[newI][newJ] != input[i][j])
            {
                perimeters.Add(((i, j), (newI, newJ), directionsPerimeters[ii]));
            }
        }
        else if (input[i][j] == input[newI][newJ])
        {
            area += Dfs(newI, newJ);
        }
        else
        {
            perimeters.Add(((i, j), (newI, newJ), directionsPerimeters[ii]));
        }
    }
    return area;
}

HashSet<((int, int), (int, int), char)> FilterPerimeters(HashSet<((int, int), (int, int), char)> perimeters)
{
    var filteredPerimeters = new HashSet<((int, int), (int, int), char)>(perimeters);
    foreach (var (pointA, pointB, direction) in perimeters)
    {
        if (direction == 'v')
        {
            var nextA = (pointA.Item1 + 1, pointA.Item2);
            var nextB = (pointB.Item1 + 1, pointB.Item2);
            while (filteredPerimeters.Contains((nextA, nextB, direction)))
            {
                filteredPerimeters.Remove((nextA, nextB, direction));
                nextA = (nextA.Item1 + 1, nextA.Item2);
                nextB = (nextB.Item1 + 1, nextB.Item2);
            }
        }
        if (direction == 'h')
        {
            var nextA = (pointA.Item1, pointA.Item2 + 1);
            var nextB = (pointB.Item1, pointB.Item2 + 1);
            while (filteredPerimeters.Contains((nextA, nextB, direction)))
            {
                filteredPerimeters.Remove((nextA, nextB, direction));
                nextA = (nextA.Item1, nextA.Item2 + 1);
                nextB = (nextB.Item1, nextB.Item2 + 1);
            }
        } 
    }

    return filteredPerimeters;
}

