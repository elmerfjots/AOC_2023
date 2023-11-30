# AoC-Csharp-Foundation
A .NET 7 foundation project for solving Advent of Code challenges in C#.
Features:
- Plumbing and scaffolding in place - you just need to implement the actual solvers
- Input made available as lines or a single string
- Utility output methods
- Implementation examples provided

## How do I use this repo?
Click the green "Use this template"-button. That will allow you to create a repository based on this one under your own GitHub account. Any further work you do will be completely independant of this repo and you can make all the improvements you want.

## Okay, but how do I actually solve challenges with this?
You need to do two things:
- Download your input from the Advent of Code page and place it *./inputs/\<year\>/\<day\>/input.txt*, for example *./inputs/2022/01/input.txt*. (Please note the leading zero when specifying single-digit days).
- Implement the *ISolver* interface provided and annotate your implementing class with the *[solves("yyyy-mm-dd")]* attribute, e.g. *[solves("2022-12-01")]*.

That's it, when you run the application, it will automatically read input and select suitable solver(s) based on the current date. You can override this behaviour by providing either an integer argument or a parseable date. Examples:
- *.\AdventOfCodeFoundation\bin\Debug\net7.0\AdventOfCodeFoundation.exe* will attempt to solve the challenge for the current day
- *.\AdventOfCodeFoundation\bin\Debug\net7.0\AdventOfCodeFoundation.exe 12* will attempt to solve the challenge for december 12th, current year
- *.\AdventOfCodeFoundation\bin\Debug\net7.0\AdventOfCodeFoundation.exe 2015-12-01* will attempt to solve the challenge for december 1st, 2015

## But you promised examples!
Sure, check out the ExampleSolver implementation for a Hello, World! example which also demonstrates how to implement required async method signature without any async functionality in the method body. You can also have a look at the Day1Solver2015, which solves the first Advent of Code challenge published - it obivously contain minor spoilers.
