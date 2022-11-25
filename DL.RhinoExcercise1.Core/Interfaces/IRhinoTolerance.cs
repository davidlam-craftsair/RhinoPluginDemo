namespace DL.RhinoExcercise1.Core
{
    public interface IRhinoTolerance
    {
        double RadAngleTol { get; }
        double Tol { get; }

        void SetTol(double tol, double angleTol);
    }
}