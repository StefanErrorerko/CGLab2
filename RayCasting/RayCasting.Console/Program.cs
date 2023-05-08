// See https://aka.ms/new-console-template for more information

using RayCasting.Core.Objects;
using RayCasting.Core.ObjLoader;

Mesh mesh = ObjLoader.LoadObj(@"C:\Users\kadde\Downloads\cow.obj");
mesh.Print();