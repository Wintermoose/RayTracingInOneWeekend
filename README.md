# Raytracing in a weekend

This repository contains the implementation of code from the excellent series, which can be found here:
https://raytracing.github.io/

The code is written in C#, but tries to stay as close to the original as reasonable (for the time being, at least).

## Choosing a scene

The main entry, Program.cs, contains a block with code like this 
```
var scene = new RayTracingInOneWeekend.Scenes.Book1CoverScene();
```

Just enable the scene you're interested in. You can also tweak the desired resolution (using 400px width
is really recommended while picking a scene, as the full 1920px image will take quite a bit to calculate;
especially for the book's final scene).

The program creates images in the PPM file format, and writes them to STDOUT. You'll want to run it while
redirecting the output to a file, f.ex

```
RayTracingInOneWeekend.exe > test.ppm
```

## Future improvements
- proper file output, multiple file formats
- UI
- randomized pixel selection to provide fast feedback?
- consider moving to .net's own Vector3 class
- optimizations

## Requirements
- .NET core 6.0. 
- the code has been tested in Visual Studio 2022