﻿using Day12;

using var fh = File.OpenText("input1.txt");
var map = await Solver.LoadMapAsync(fh);
var p1 = Solver.Part1(map);
Console.WriteLine($"Part 1: {p1}");
var p2 = Solver.Part2(map);
Console.WriteLine($"Part 2: {p2}");