using DL.Framework;
using DL.RhinoExcercise1.Core;
using ReportUtilityLib;
using System;
using System.Diagnostics;

namespace RhinoPlugInExcercise1
{
    public class IoCConfiguration: IIoCConfiguration
    {
        public void Configure()
        {
            RegisterAsSingle<ILogWriter>(() => new RhinoAppLogWriter());
            RegisterAsSingle<IReporter>(() => new Reporter(new ErrorReporterV2(Get<ILogWriter>()),
                                                           new EventReporter(),
                                                            Get<ILogWriter>()));
            RegisterAsSingle<IRhinoReplaceManager>(() => new RhinoReplaceManager());
            RegisterAsSingle<IRhinoGetManager>(() => new RhinoGetManager());
            RegisterAsSingle<IPlaneDrawer>(() => new PlaneDrawer());
            RegisterAsSingle<IBrepCleaner>(() => new BrepCleaner(Get<IRhinoTolerance>()));
            RegisterAsSingle<IRhinoTolerance>(() => new RhinoTolerance());
            RegisterAsSingle<IBrepCombiner>(() => new BrepCombinerV2(Get<IRhinoTolerance>()));
            RegisterAsSingle<IRhinoDocContainer>(() => new RhinoDocContainer());
            RegisterAsSingle<IDistanceBtwObjAndViewGetter>(() => new DistanceBtwObjAndViewGetter());
            RegisterAsSingle<IRelevantObjsGetter>(() => new RelevantObjsGetterV2(Get<IRhinoTolerance>(), Get<IRhinoDocContainer>()));
            RegisterAsSingle<IPullFaceFromExtrusionHandler>(() => new PullFaceFromExtrusionHandler(Get<IBrepCleaner>(), Get<IRhinoTolerance>()));
            RegisterAsSingle<IPullFaceFromBrepHandler>(() => new PullFaceFromBrepHandlerV2(Get<IBrepCleaner>(), Get<IBrepCombiner>()));
            RegisterAsSingle<IDrawer>(() => new TypicalDrawer(Get<IPlaneDrawer>(), Get<IDefaultDisplayMaterials>()));
            RegisterAsSingle<IDefaultDisplayMaterials>(() => new DefaultDisplayMaterials());
            RegisterAsSingle<ILineSelectedToTrimGetter>(() => new LineSelectedToTrimGetter(Get<IRhinoTolerance>()));
            RegisterAsSingle<IPolylineTrimInfoGetter>(() => new PolylineTrimInfoGetter(Get<ILineSelectedToTrimGetter>()));
            RegisterAsSingle<IRhinoObjChangeManager>(() => new RhinoObjChangeManager());
            RegisterAsSingle<IRhinoDoc>(() => new RhinoDocImp(Get<IRhinoDocContainer>(),
                                                              Get<IRhinoObjChangeManager>(),
                                                              Get<IRhinoTolerance>(),
                                                              Get<IReporter>()));
            RegisterAsSingle<ISessionDoc>(() => new SessionDoc());
            RegisterAsSingle<IDocManager>(() => new DocManager(Get<IRhinoDoc>(), Get<ISessionDoc>(), Get<IRhinoDocContainer>()));
            RegisterAsSingle<IBrepFloorFactory>(() => new BrepFloorFactoryV2(Get<IRhinoTolerance>(),
                                                                            Get<IReporter>(),
                                                                            Get<IBrepFactoryExt>()
                                                                            ));
            RegisterAsSingle<IBrepFactoryExt>(() => new BrepFactoryExt(Get<IRhinoTolerance>()));
            RegisterAsSingle<IBrepFloorFactoryV2>(() => new BrepFloorFactoryV5(Get<IEdgesSorter>(), Get<IBrepFloorFactory>()));
            RegisterAsSingle<IEdgesSorter>(() => new EdgesSorter());

            RegisterAsSingle<IFloorBrepUpdateSubscriber>(() => new FloorBrepUpdateSubscriberV2(Get<IRhinoObjChangeManager>(),
                                                                                                  Get<IDocManager>()));
            RegisterAsSingle<IClosedPlanarCurveAdditionListener>(() => new ClosedPlanarCurveAdditionListener(Get<IDocManager>()));
            RegisterAsSingle<IClosedPlanarCurveAdditionBag>(() => new ClosedPlanarCurveAdditionBag(Get<IClosedPlanarCurveAdditionListener>()));

            RegisterAsSingle<IBrepSweepsFactory>(() => new BrepSweepsFactory(Get<IRhinoTolerance>()));
            RegisterAsSingle<ISweepAssemblyIdFactory>(() => new SweepAssemblyIdFactory());
            RegisterAsSingle<IRailingAssemblyIdFactory>(() => new RailingAssemblyIdFactory());
            RegisterAsSingle<IAssemblyIdFactory>(() => new AssemblyIdFactory());
            RegisterAsSingle<IRailingComponentIdsByNameFactory>(() => new RailingComponentIdsByNameFactory());
            //RegisterAsSingle<IAssemblyComponentsToSessionAdderFactory>(() => new AssemblyComponentsToSessionAdderFactory());
            AddActions();
        }

        private void AddActions()
        {
            AddFloorPackage();
            AddSweepObjPackage();
        }

        private void AddSweepObjPackage()
        {
            RegisterAsSingle<IEditableSweepCreateActionFactory>(() => new EditableSweepCreateActionFactory());

            // Generic Sweeps
            RegisterAsSingle<IEditableSweepCreateAction>(
                () => IoC.Get<IEditableSweepCreateActionFactory>().Create(new GenericAssemblyComponentsToSessionAdder(IoC.Get<IDocManager>(), IoC.Get<IAssemblyIdFactory>(), "Generic")));
                );
        }

        private void AddFloorPackage()
        {
            AddFloorCreateByBoundaryAction();
            AddFloorBoundaryOpenEditAction();
            AddFloorBoundaryCloseEditAction();
        }

        private void AddFloorBoundaryCloseEditAction()
        {
            RegisterAsSingle<IFloorBoundaryCloseEditAction>(
                ()=> new FloorBoundaryCloseEditActionV3(Get<IDocManager>(),
                                                      Get<IBrepFloorFactoryV2>(),
                                                      Get<IFloorBrepUpdateSubscriber>(),
                                                      Get<IClosedPlanarCurveAdditionBag>()
                                                      )
                );

            //RegisterAsSingle<IFloorBoundaryCloseEditAction>(
            //                                () => new FloorBoundaryCloseEditActionV2(Get<IDocManager>(),
            //                              Get<IBrepFloorFactoryV2>(),
            //                              Get<IFloorBrepUpdateSubscriber>()
            //                              )
            //);
        }

        private void AddFloorBoundaryOpenEditAction()
        {
            RegisterAsSingle<IFloorBoundaryOpenEditAction>(
                            () => new FloorBoundaryOpenEditActionV2(Get<IFloorBrepUpdateSubscriber>(),
                                                                    Get<IClosedPlanarCurveAdditionBag>()
                            ));

            //RegisterAsSingle<IFloorBoundaryOpenEditAction>(
            //                () => new FloorBoundaryOpenEditAction(Get<IFloorBrepUpdateSubscriber>()));
        }

        private void AddFloorCreateByBoundaryAction()
        {
            RegisterAsSingle<IFloorCreateByBoundaryAction>(
                            () => new FloorCreateByBoundaryActionOriginalV2(Get<IDocManager>(),
                                                                          Get<IBrepFloorFactoryV2>()));

            //RegisterAsSingle<IFloorCreateByBoundaryAction>(
            //                () => new FloorCreateByBoundaryActionOriginal(Get<IEdgesSorter>(),
            //                                                              Get<IBrepFloorFactory>()));

            RegisterAsSingle<IFloorCreateByBoundaryActionV2>(
                ()=>new FloorCreateByBoundaryActionV3(Get<IDocManager>(), Get<IFloorCreateByBoundaryAction>())
                );
        }

            //private void AddFloorCreateByBoundaryActionV2()
            //{
            //    RegisterAsSingle<IFloorCreateByBoundaryAction>(
            //                    () => new FloorCreateByBoundaryActionV2(new EdgesSorter(),
            //                                                            new BrepFactoryExt(Get<IRhinoTolerance>())));
            //}

        private T Get<T>()
        {
            var t = IoC.Get<T>();
            if (t == null)
            {
                Debug.WriteLine($"cant get {typeof(T)} ");
            }
            return t;
        }

        private void RegisterAsSingle<T>(Func<T> p)
        {
            IoC.RegisterAsSingle<T>(p);
        }

        public void ClearConfiguration()
        {
            IoC.ClearAll();
        }
    }
}
