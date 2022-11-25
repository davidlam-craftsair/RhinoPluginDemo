using Rhino.Display;
using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    /// <summary>
    /// SampleCsGumballCylinder
    /// </summary>
    public class GumballCylinderV2
  {
    readonly double m_radius;
    readonly double m_height;
    Circle m_circle;
    Cylinder m_cylinder;
    Brep m_brep;

    public GumballCylinderV2()
    {
      if (Create(Plane.WorldXY, 1.0, 1.0))
      {
        m_radius = 1.0;
        m_height = 1.0;
      }
    }

    public GumballCylinderV2(Plane plane, double radius, double height)
    {
      if (Create(plane, radius, height))
      {
        m_radius = radius;
        m_height = height;
      }
    }

    public bool Create(Plane plane, double radius, double height)
    {
      var rc = false;
      if (radius > 0.0 && height > 0.0)
      {
        m_circle = new Circle(plane, radius);
        m_cylinder = new Cylinder(m_circle, height);
        rc = m_cylinder.IsValid;
        if (rc)
        {
          m_brep = ToBrep;
          rc = m_brep.IsValid;
        }
      }
      return rc;
    }

    public Point3d Center
    {
      get
      {
        return m_circle.Center;
      }
    }

    public double Radius
    {
      get
      {
        return m_circle.Radius;
      }
      set
      {
        Create(m_circle.Plane, value, m_cylinder.TotalHeight);
      }
    }

    public double Height
    {
      get
      {
        return m_cylinder.TotalHeight;
      }
      set
      {
        Create(m_circle.Plane, m_circle.Radius, value);
      }
    }

    public Plane RadiusPlane
    {
      get
      {
        var center = m_circle.PointAt(0.0);
        var xaxis = m_circle.TangentAt(0.0);
        xaxis.Unitize();
        var zaxis = m_circle.Normal;
        zaxis.Unitize();
        return new Plane(center, xaxis, zaxis);
      }
    }

    public Plane HeightPlane
    {
      get
      {
        var dir = m_circle.Normal;
        dir.Unitize();
        dir *= m_cylinder.TotalHeight;
        var center = Point3d.Add(m_circle.Center, dir);
        return new Plane(center, m_circle.Plane.XAxis, m_circle.Plane.YAxis);
      }
    }

    public double BaseRadius
    {
      get
      {
        return m_radius;
      }
    }

    public double BaseHeight
    {
      get
      {
        return m_height;
      }
    }

    public void Draw(DisplayPipeline display)
    {
      if (null != m_brep)
        display.DrawBrepWires(m_brep, Rhino.ApplicationSettings.AppearanceSettings.FeedbackColor);
    }

    public Brep ToBrep
    {
      get
      {
        if (m_cylinder.IsValid)
          return m_cylinder.ToBrep(true, true);
        return null;
      }
    }
  }
}
