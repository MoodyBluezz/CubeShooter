using System.Collections.Generic;
using Client.Scripts.Controller;

namespace Client
{
    public interface ICubeGenerator
    {
        void GenerateCubes();
        List<CubeController> CubeControllers { get; }
    }
}