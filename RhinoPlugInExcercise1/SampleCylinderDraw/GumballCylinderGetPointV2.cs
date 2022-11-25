using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Geometry;
using Rhino.UI.Gumball;

namespace RhinoPlugInExcercise1
{
    /// <summary>
    /// SampleCsGumballCylinderGetPoint
    /// </summary>
    public class GumballCylinderGetPointV2 : GetPoint
  {
    private readonly GumballCylinderV2 m_cylinder;
    private readonly GumballDisplayConduit m_radius_dc;
    private readonly GumballDisplayConduit m_height_dc;
    private Point3d m_base_origin;
    private Point3d m_base_point;

    public GumballCylinderGetPointV2(GumballCylinderV2 cylinder,
                                    GumballDisplayConduit radiusDc,
                                    GumballDisplayConduit heightDc)
    {
      m_cylinder = cylinder;
      m_radius_dc = radiusDc;
      m_height_dc = heightDc;
      m_base_origin = Point3d.Unset;
      m_base_point = Point3d.Unset;
    }

    protected override void OnMouseDown(GetPointMouseEventArgs e)
    {
      if (m_radius_dc.PickResult.Mode != GumballMode.None || m_height_dc.PickResult.Mode != GumballMode.None)
        return;

      m_base_origin = Point3d.Unset;
      m_base_point = Point3d.Unset;

      m_radius_dc.PickResult.SetToDefault();
      m_height_dc.PickResult.SetToDefault();

      var pick_context = new PickContext
      {
        View = e.Viewport.ParentView,
        PickStyle = PickStyle.PointPick
      };

      var xform = e.Viewport.GetPickTransform(e.WindowPoint);
      pick_context.SetPickTransform(xform);

      Line pick_line;
      e.Viewport.GetFrustumLine(e.WindowPoint.X, e.WindowPoint.Y, out pick_line);

      pick_context.PickLine = pick_line;
      pick_context.UpdateClippingPlanes();

      // try picking one of the gumballs
      if (m_radius_dc.PickGumball(pick_context, this))
      {
        m_base_origin = m_cylinder.RadiusPlane.Origin;
        m_base_point = e.Point;
      }
      else if (m_height_dc.PickGumball(pick_context, this))
      {
        m_base_origin = m_cylinder.HeightPlane.Origin;
        m_base_point = e.Point;
      }
    }

    protected override void OnMouseMove(GetPointMouseEventArgs e)
    {
      if (m_base_origin.IsValid && m_base_point.IsValid)
      {
        Line world_line;
        if (e.Viewport.GetFrustumLine(e.WindowPoint.X, e.WindowPoint.Y, out world_line))
        {
          var dir = e.Point - m_base_point;
          var len = dir.Length;
          if (m_base_origin.DistanceTo(e.Point) < m_base_origin.DistanceTo(m_base_point))
            len = -len;

          if (m_radius_dc.PickResult.Mode != GumballMode.None)
          {
            // update radius gumball
            m_radius_dc.UpdateGumball(e.Point, world_line);
            // update cylinder
            m_cylinder.Radius = m_cylinder.BaseRadius + len;
          }
          else if (m_height_dc.PickResult.Mode != GumballMode.None)
          {
            // update hight gumball
            m_height_dc.UpdateGumball(e.Point, world_line);
            // update cylinder
            m_cylinder.Height = m_cylinder.BaseHeight + len;
          }
        }
      }

      base.OnMouseMove(e);
    }

    protected override void OnDynamicDraw(GetPointDrawEventArgs e)
    {
      m_cylinder.Draw(e.Display);

      // Disable default GetPoint drawing by not calling the base class
      // implementation. All aspects of gumball display are handled by 
      // GumballDisplayConduit
      //base.OnDynamicDraw(e);
    }

    public GetResult MoveGumball()
    {
      var rc = Get(true);
      return rc;
    }
  }
}
