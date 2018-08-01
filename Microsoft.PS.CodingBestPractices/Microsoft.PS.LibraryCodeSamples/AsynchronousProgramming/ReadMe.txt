****Guidelines****

Old					New										Description
task.Wait			await task								Wait/await for a task to complete
task.Result			await task								Get the result of a completed task
Task.WaitAny		await Task.WhenAny						Wait/await for one of a collection of tasks to complete
Task.WaitAll		await Task.WhenAll						Wait/await for every one of a collection of tasks to complete
Thread.Sleep		await Task.Delay						Wait/await for a period of time
Task constructor	Task.Run or TaskFactory.StartNew		Create a code-based task

****Solutions to Common Async Problems ****

Problem														Solution
Create a task to execute code								Task.Run or TaskFactory.StartNew (not the Task constructor or Task.Start)
Create a task wrapper for an operation or event				TaskFactory.FromAsync or TaskCompletionSource<T>
Support cancellation										CancellationTokenSource and CancellationToken
Report progress												IProgress<T> and Progress<T>
Handle streams of data										TPL Dataflow or Reactive Extensions
Synchronize access to a shared resource						SemaphoreSlim
Asynchronously initialize a resource						AsyncLazy<T>
Async-ready producer/consumer structures					TPL Dataflow or AsyncCollection<T>

****For samples refer****
Microsoft.PS.AspNetCodeSamples.Controllers.AsyncAwaitController

