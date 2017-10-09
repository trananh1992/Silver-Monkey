﻿namespace Monkeyspeak.Libraries
{
    internal class Debug : AbstractBaseLibrary
    {
        public Debug()
        {
            //(0:10000) when a debug breakpoint is hit,
            Add(TriggerCategory.Cause, 10000, WhenBreakpointHit,
                "(0:10000) when a debug breakpoint is hit,");

            //(5:10000) create a debug breakpoint here,
            Add(TriggerCategory.Effect, 10000, CreateBreakPoint,
                "(5:10000) create a debug breakpoint here,");
        }

        public override void OnPageDisposing(Page page)
        {
        }

        private bool CreateBreakPoint(TriggerReader reader)
        {
            if (System.Diagnostics.Debugger.Launch())
            {
                System.Diagnostics.Debugger.NotifyOfCrossThreadDependency();
                System.Diagnostics.Debugger.Break();
                reader.Page.Execute(10000);
            }
            else RaiseError("Debugger could not be attached.");

            return true;
        }

        private bool WhenBreakpointHit(TriggerReader reader)
        {
            return true;
        }
    }
}