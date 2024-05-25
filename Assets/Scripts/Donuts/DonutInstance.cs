namespace DefaultNamespace
{
    public class DonutInstance
    {
        public DonutView View;
        public DonutData Data;

        public DonutInstance(DonutData data, DonutView view)
        {
            View = view;
            Data = data;
            view.RegisterData(data);
        }
    }
}