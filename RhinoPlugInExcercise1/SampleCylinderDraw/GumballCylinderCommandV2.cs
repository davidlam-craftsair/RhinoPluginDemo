using Rhino;
using Rhino.Commands;
using Rhino.Display;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Geometry;
using Rhino.UI.Gumball;

namespace RhinoPlugInExcercise1
{
    public class GumballCylinderCommandV2 : Command
  {
    double m_radius;
    double m_height;

    public GumballCylinderCommandV2()
    {
      m_radius = 1.0;
      m_height = 1.0;
    }

    public override string EnglishName
    {
      get { return "SampleCsGumballCylinderV2"; }
    }

    protected override Result RunCommand(RhinoDoc doc, RunMode mode)
    {
      GetPoint gp = new GetPoint();
      gp.SetCommandPrompt("Base of cylinder");
      gp.Get();
      if (gp.CommandResult() != Result.Success)
        return gp.CommandResult();

      Point3d center = gp.Point();
      Plane plane = Plane.WorldXY;
      RhinoView view = gp.View();
      if (null != view)
        plane = view.ActiveViewport.ConstructionPlane();
      plane.Origin = center;

      var cylinder = new GumballCylinderV2(plane, m_radius, m_height);

      var radius_go = new GumballObject();
      var height_go = new GumballObject();

      var radius_dc = new GumballDisplayConduit();
      var height_dc = new GumballDisplayConduit();

      var radius_gas = RadiusGumballAppearanceSettings();
      var height_gas = HeightGumballAppearanceSettings();

      while (true)
      {
        radius_go.SetFromPlane(cylinder.RadiusPlane);
        height_go.SetFromPlane(cylinder.HeightPlane);

        radius_dc.SetBaseGumball(radius_go, radius_gas);
        height_dc.SetBaseGumball(height_go, height_gas);

        radius_dc.Enabled = true;
        height_dc.Enabled = true;

        var gx = new GumballCylinderGetPointV2(cylinder, radius_dc, height_dc);
        gx.SetCommandPrompt("Drag gumball. Press Enter when done");
        gx.AcceptNothing(true);
        gx.MoveGumball();

        radius_dc.Enabled = false;
        height_dc.Enabled = false;

        if (gx.CommandResult() != Result.Success)
          break;

        var res = gx.Result();
        if (res == GetResult.Point)
        {
          var radius = cylinder.Radius;
          var height = cylinder.Height;
          cylinder = new GumballCylinderV2(plane, radius, height);
          continue;
        }
        if (res == GetResult.Nothing)
        {
          m_radius = cylinder.Radius;
          m_height = cylinder.Height;
          cylinder = new GumballCylinderV2(plane, m_radius, m_height);
          var brep = cylinder.ToBrep;
          if (null != brep)
            doc.Objects.AddBrep(brep);
        }

        break;
      }

      doc.Views.Redraw();

      return Result.Success;
    }

    private GumballAppearanceSettings RadiusGumballAppearanceSettings()
    {
      var gas = new GumballAppearanceSettings
      {
        RelocateEnabled = false,
        RotateXEnabled = false,
        RotateYEnabled = false,
        RotateZEnabled = false,
        ScaleXEnabled = false,
        ScaleYEnabled = false,
        ScaleZEnabled = false,
        TranslateXEnabled = false,
        TranslateXYEnabled = false,
        TranslateYEnabled = false,
        TranslateYZEnabled = false,
        TranslateZEnabled = true,
        TranslateZXEnabled = false,
        MenuEnabled = false
      };
      return gas;
    }

    private GumballAppearanceSettings HeightGumballAppearanceSettings()
    {
      var gas = new GumballAppearanceSettings
      {
        RelocateEnabled = false,
        RotateXEnabled = false,
        RotateYEnabled = false,
        RotateZEnabled = false,
        ScaleXEnabled = false,
        ScaleYEnabled = false,
        ScaleZEnabled = false,
        TranslateXEnabled = false,
        TranslateXYEnabled = false,
        TranslateYEnabled = false,
        TranslateYZEnabled = false,
        TranslateZEnabled = true,
        TranslateZXEnabled = false,
        MenuEnabled = false
      };
      return gas;
    }
  }
}
