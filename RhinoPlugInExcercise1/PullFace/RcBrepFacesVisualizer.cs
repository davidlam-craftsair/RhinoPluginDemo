using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using Rhino.Input;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    [Guid("30520A72-EA2F-4F3E-A48A-20865CAF4791")]
    public class RcBrepFacesVisualizer : AbsCommand
    {
        private BrepFaceList faces;
        private IEnumerable<Curve> trimCurves;
        private IEnumerable<Curve> edges;
        private IEnumerable<Surface> surfaces;
        private IEnumerable<Brep> brepParts;
        private bool facesVisibility = true;
        private bool edgesVisibility = true;
        private bool surfacesVisibility = true;
        private bool surfaceSelect = false;
        private int selectedBrepFaceIdx = 0;
        private DisplayMaterial selecteBrepFaceMaterial = new DisplayMaterial(System.Drawing.Color.DarkRed, 0.5);
        private bool selectedBrepFaceVisibility;

        protected override Result DoRunCommand()
        {
            DisplayPipeline.PostDrawObjects += OnPostDrawObjects;
            var go = new GetObject();
            go.SetCommandPrompt("Select Brep to visualize");
            go.GeometryFilter = ObjectType.Brep | ObjectType.PolysrfFilter | ObjectType.Surface;
            go.SubObjectSelect = false;
            go.EnablePreSelect(false, false);
            go.GetMultiple(0, 1);  // pick up just one brep
            if (go.CommandResult() != Result.Success)
            {
                return go.CommandResult();
            }

            var objRef = go.Object(0);
            var brep = objRef.Brep();
            if (brep != null)
            {
                brep.Faces.ShrinkFaces();
                faces = brep.Faces;
                trimCurves = brep.Trims.Select(x => x.TrimCurve).ToArray();
                edges = brep.Edges.Select(x => x.EdgeCurve).ToArray();
                surfaces = GetShrinkedSurfaces(brep.Faces);
                brepParts = GetBrepParts(brep.Faces);
            }

            //bool flag = false;
            var getOption = new GetOption();
            getOption.SetCommandPrompt("Options to proceed");


            while (true)
            {
                getOption.ClearCommandOptions();
                var visualizeOptionToggle = new OptionToggle(true, "No", "Yes");
                var visualizeIdx = getOption.AddOptionToggle("Visualize", ref visualizeOptionToggle);
            
                var facesOptionToggle = new OptionToggle(facesVisibility, "No", "Yes");
                var facesIdx = getOption.AddOptionToggle("Faces", ref facesOptionToggle);
            
                var edgesOptionToggle = new OptionToggle(edgesVisibility, "No", "Yes");
                var edgesIdx = getOption.AddOptionToggle("Edges", ref edgesOptionToggle);

                var surfacesOptionToggle = new OptionToggle(surfacesVisibility, "No", "Yes");
                var surfacesIdx = getOption.AddOptionToggle("Surfaces", ref surfacesOptionToggle);

                var surfaceSelectOption = new OptionToggle(surfaceSelect, "No", "Yes");
                var surfaceSelectIdx = getOption.AddOptionToggle("SurfaceSelect", ref surfaceSelectOption);
                var getResult = getOption.Get();
                if (getResult == GetResult.Option)
                {
                    var optionIndex = getOption.OptionIndex();
                    if (visualizeIdx == optionIndex)
                    {
                        if (!visualizeOptionToggle.CurrentValue)  // if currentValue is false, then turn off and return Result.Success
                        {
                            DisplayPipeline.PostDrawObjects -= OnPostDrawObjects;
                            Redraw();
                            return Result.Success;
                        }
                    }
                    else if (facesIdx == optionIndex)
                    {
                        facesVisibility = facesOptionToggle.CurrentValue;
                    }
                    else if (edgesIdx == optionIndex)
                    {
                        edgesVisibility = edgesOptionToggle.CurrentValue;
                    }
                    else if (surfacesIdx == optionIndex)
                    {
                        surfacesVisibility = surfacesOptionToggle.CurrentValue;
                    }
                    else if (surfaceSelectIdx == optionIndex)
                    {
                        if (surfaceSelectOption.CurrentValue)
                        {
                            getOption.ClearCommandOptions();
                            var getSurfaceSelectIndexOption = new OptionInteger(selectedBrepFaceIdx);
                            var getSurfaceSelectIndexOptionIndex = getOption.AddOptionInteger("surfaceIndex", ref getSurfaceSelectIndexOption);

                            surfacesVisibility = false;  // Switch off the surfaces visibility
                            while (true)
                            {
                                var getResult2 = getOption.Get();
                                if (getResult2 == GetResult.Cancel)
                                {
                                    break;
                                }
                                if (getResult2 == GetResult.Option)
                                {
                                    selectedBrepFaceVisibility = true;
                                    var idx = getSurfaceSelectIndexOption.CurrentValue;
                                    selectedBrepFaceIdx = idx;
                                    Redraw();
                                }
                            }

                        }
                        else
                        {
                        }
                                                   
                    }
                }
                Redraw();

            }
        }

        private void Redraw()
        {
            ActiveDoc.Views.Redraw();
        }

        private IEnumerable<Brep> GetBrepParts(BrepFaceList faces)
        {
            foreach (var item in faces)
            {
                yield return item.DuplicateFace(false);
            }
        }

        //private IEnumerable<Surface> GetShrinkedSurfaces(BrepFaceList faces)
        //{
        //    foreach (var item in faces)
        //    {
        //        var t = item.DuplicateFace(false);
        //        if (t.IsSurface)
        //        {
        //            yield return t.Surfaces[0];
        //        }
        //    }
        //}

        private IEnumerable<Surface> GetShrinkedSurfaces(BrepFaceList faces)
        {
            faces.ShrinkFaces();
            return faces.Select(x => x.UnderlyingSurface()).ToArray();
        }

        private void OnPostDrawObjects(object sender, DrawEventArgs e)
        {
            DisplayFaces(e);
            DisplayTrimCurves(e);
            DisplayEdges(e);
            DisplaySurfaces(e);
            DisplaySelectedBrepFace(e);
        }

        //private void DisplaySelectedBrepFace(DrawEventArgs e)
        //{
        //    if (selectedBrepFaceVisibility)
        //    {
        //        var item = brepParts.ElementAtOrDefault(selectedBrepFaceIdx);
        //        if (item != null)
        //        {
        //            e.Display.DrawSurface(item.Faces[0], System.Drawing.Color.DarkRed, 3);
        //        }
        //    }
        //}

        private void DisplaySelectedBrepFace(DrawEventArgs e)
        {
            if (selectedBrepFaceVisibility)
            {
                var item = brepParts.ElementAtOrDefault(selectedBrepFaceIdx);
                if (item != null)
                {
                    e.Display.DrawBrepShaded(item, selecteBrepFaceMaterial);
                }
            }
        }

        private void DisplaySurfaces(DrawEventArgs e)
        {
            if (surfaces != null && surfacesVisibility)
            {
                foreach (var item in brepParts)
                {
                    e.Display.DrawBrepShaded(item, selecteBrepFaceMaterial);
                }
            }
        }

        private void DisplaySurfacesV2(DrawEventArgs e)
        {
            if (surfaces != null && surfacesVisibility)
            {
                foreach (var item in surfaces)
                {
                    e.Display.DrawSurface(item, System.Drawing.Color.DarkRed, 10);
                    //e.Display.DrawBrepShaded(item.ToBrep(),)
                }
            }
        }

        private void DisplayFaces(DrawEventArgs e)
        {
            if (faces != null && facesVisibility)
            {
                var count = 0;
                foreach (var item in faces)
                {
                    item.SetDomain(0, new Rhino.Geometry.Interval(0, 1));
                    item.SetDomain(1, new Rhino.Geometry.Interval(0, 1));
                    var pt = item.PointAt(0.5, 0.5);
                    e.Display.DrawDot(pt, $"face{count}");
                    count += 1;
                }
            }
        }

        private void DisplayEdges(DrawEventArgs e)
        {
            if (edges != null  && edgesVisibility)
            {
                var count = 0;
                foreach (var item in edges)
                {
                    var t = item.PointAtNormalizedLength(0.5);
                    e.Display.DrawDot(t, $"edge{count}");
                    count += 1;
                }
            }
        }

        private void DisplayTrimCurves(DrawEventArgs e)
        {
            if (trimCurves != null)
            {
                var count = 0;
                foreach (var item in trimCurves)
                {
                    var t = item.PointAtNormalizedLength(0.5);
                    e.Display.DrawDot(t, $"trimCrv{count}");
                    count += 1;
                }
            }
        }

        public override string EnglishName => "VisualizeBrepFaces";
    }
}
