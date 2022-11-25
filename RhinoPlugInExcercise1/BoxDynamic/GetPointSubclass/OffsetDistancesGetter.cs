using Rhino;
using Rhino.Input;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class OffsetDistancesGetter : IOffsetDistancesGetter
    {
        public bool Get(out double dx, out double dy)
        {
            var gp = new GetNumber();
            gp.SetCommandPrompt("Input the relative x to the click pt");
            gp.Get();
            if (gp.CommandResult() != Rhino.Commands.Result.Success)
            {
                dx = default;
                dy = default;
                return false;
            }
            dx = gp.Number();

            var gp2 = new GetNumber();
            gp2.SetCommandPrompt("Input the relative y to the click pt");
            gp2.Get();
            if (gp2.CommandResult() != Rhino.Commands.Result.Success)
            {
                dy = default;
                return false;
            }
            dy = gp2.Number();
            RhinoApp.WriteLine($"Offset distances = {dx}, {dy}");

            return true;
        }

        public bool Get(GetPoint gp, out double dx, out double dy)
        {
            throw new NotImplementedException();
        }
    }

    public class OffsetDistancesGetterV2 : IOffsetDistancesGetter
    {
        public bool Get(out double dx, out double dy)
        {
            using (var gp = new GetNumber())
            {
                gp.SetCommandPrompt("Input the offset x to the click pt");
                gp.SetDefaultNumber(0.6);
                gp.AcceptNothing(true);
                gp.Get();
                if (gp.CommandResult() != Rhino.Commands.Result.Success)
                {
                    dx = default;
                    dy = default;
                    return false;
                }
                dx = gp.Number();

            }
            using (var gp2 = new GetNumber())
            {
                gp2.SetCommandPrompt("Input the offset y to the click pt");
                gp2.SetDefaultNumber(-0.6);
                gp2.AcceptNothing(true);
                gp2.Get();
                if (gp2.CommandResult() != Rhino.Commands.Result.Success)
                {
                    dx = default;
                    dy = default;
                    return false;
                }
                dy = gp2.Number();

            }
            
            RhinoApp.WriteLine($"Offset distances = {dx}, {dy}");

            return true;
        }

        public bool Get(GetPoint gp, out double dx, out double dy)
        {
            throw new NotImplementedException();
        }
    }

    public class OffsetDistancesGetterV3 : IOffsetDistancesGetter
    {
        public bool Get(out double dx, out double dy)
        {
            dx = 0.5;
            dy = -0.5;

            return true;
        }

        public bool Get(GetPoint gp, out double dx, out double dy)
        {
            throw new NotImplementedException();
        }
    }

    public class OffsetDistancesGetterV4 : IOffsetDistancesGetter
    {
        public bool Get(out double dx, out double dy)
        {
            double tx = 0;
            double ty = 0;
            Rhino.Commands.Result gp1 = RhinoGet.GetNumber("Input the relative x to the click pt", true, ref tx);
            if (gp1 == Rhino.Commands.Result.Success)
            {
                Rhino.Commands.Result gp2 = RhinoGet.GetNumber("Input the relative y to the click pt", false, ref ty);
                if (gp2 == Rhino.Commands.Result.Success)
                {
                    dx = tx;
                    dy = ty;
                    RhinoApp.WriteLine($"Offset distances = {dx}, {dy}");
                    return true;
                }
            }
            dx = 0;
            dy = 0;
            return false;
        }

        public bool Get(GetPoint gp, out double dx, out double dy)
        {
            throw new NotImplementedException();
        }
    }

    public class OffsetDistancesGetterV5 : IOffsetDistancesGetter
    {
        public bool Get(GetPoint gp, out double dx, out double dy)
        {
            throw new NotImplementedException();
        }

        public bool Get(out double dx, out double dy)
        {
            dx = 0;
            dy = 0;
            var flag = false;
            var go = new GetOption();
            go.SetCommandPrompt("Set offset distance");
            go.AcceptNothing(true);
            var odx = new OptionDouble(0.6);
            var indexX = go.AddOptionDouble("x", ref odx);
            var ody = new OptionDouble(-0.6);
            var indexY = go.AddOptionDouble("y", ref ody);
            while (true)
            {
                var res = go.Get();

                if (res == GetResult.Option)
                {
                    int optionIndex = go.OptionIndex();
                    if (optionIndex == indexX)
                    {
                        dx = odx.CurrentValue;
                        flag = true;
                    }
                    else if (optionIndex == indexY)
                    {
                        dy = ody.CurrentValue;
                        flag = true;

                    }

                    continue;
                }
                break;
            }

            return flag;
        }
    }
}
