using MStopwatch.Views;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace MStopwatch
{
    public class MStopwatchModule : IModule
    {
        IRegionManager _regionManager;

        public MStopwatchModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(MainPageView));
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(ResultPageView));
        }
    }
}