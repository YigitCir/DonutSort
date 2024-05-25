using UnityEngine;

namespace DefaultNamespace.Pole
{
    public class PoleInstance
    {
        public PoleData Data;
        public PoleView View;

        public PoleInstance(PoleData data, PoleView view)
        {
            Data = data;
            View = view;
            View.RegisterData(Data);
        }

        public void Clear()
        {
            GameObject.Destroy(View.gameObject);
        }
    }
}