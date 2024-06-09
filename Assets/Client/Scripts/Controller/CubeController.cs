using Client.ScriptableObjects;
using Client.Scripts.Model;
using Client.Scripts.View;

namespace Client.Scripts.Controller
{
    public class CubeController
    {
        private readonly CubeModel _model;
        public CubeView View { get; }

        public CubeController(CubeData data, CubeView view)
        {
            _model = new CubeModel(data);
            View = view;

            UpdateView();
        }

        private void UpdateView()
        {
            View.SetPosition(_model.Position);
            View.SetRotation(_model.Rotation);
            View.UpdateCounter(_model.Counter);
            View.SetColor(_model.Color);
        }
    }
}