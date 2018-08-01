using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Microsoft.PS.LibraryCodeSamples.BackgroundTask
{
    /// <summary>
    /// Reference url: https://blog.stephencleary.com/2014/06/fire-and-forget-on-asp-net.html
    /// </summary>
    public class FireAndForgetCodeSnippets
    {
        //The easiest method is to just throw work onto a background thread
        //Task.Run, Task.Factory.StartNew, Delegate.BeginInvoke, ThreadPool.QueueUserWorkItem
        //However the ASP.NET runtime has no idea that you’ve queued this work, so it’s not aware that the background work even exists
        //For a variety of reasons, IIS/ASP.NET has to occasionally recycle your application
        //If you have background work running when this recycling takes place, that work will mysteriously disappear.
        public void PerformBackgroundTask_Before(int noOfBackgroundJobs)
        {
            Task.Run(() => BackgroundJobAsync(noOfBackgroundJobs));
        }

        public void PerformBackgroundTask_After(int noOfBackgroundJobs)
        {
            var registerTasks = new RegisteredTasks();
            Task.Run(() => registerTasks.RunBackgroundWork(new BackgroundWork()));
        }

        private async Task BackgroundJobAsync(int noOfBackgroundJobs)
        {
            for (int i = 0; i < noOfBackgroundJobs; i++)
            {
                await Task.Delay(1000);
            }
        }
    }

    public class RegisteredTasks : IRegisteredObject
    {
        private readonly CancellationTokenSource _shutdown;

        public RegisteredTasks()
        {
            _shutdown = new CancellationTokenSource();
            HostingEnvironment.RegisterObject(this);
        }
        
        public async Task RunBackgroundWork(BackgroundWork backgroundWork)
        {
            await backgroundWork.RunAsync(_shutdown.Token);
        }

        public void Stop(bool immediate)
        {
            //trigger the cancellation event
            _shutdown.Cancel();
            if (immediate)
            {               
                Task.Delay(10000);
            }
        }
    }

    public class BackgroundWork
    {
        public async Task RunAsync(CancellationToken cancellation)
        {
            for (int i = 0; i < 10; i++)
            {
                if (cancellation.IsCancellationRequested)
                {
                    //Handle the scenario when cancelation is requested 
                    //exit gracefully
                }
                await Task.Delay(1000);               
            }
        }
    }
}
