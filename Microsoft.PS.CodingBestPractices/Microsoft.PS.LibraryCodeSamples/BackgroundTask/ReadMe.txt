**** Guidelines to run background tasks **** 

Use ThreadPool
Use HostingEnvironment.QueueBackgroundWorkItem

**** Recommended Approach ****

Distributed Architecture

1. The first thing you need is a reliable storage medium. I prefer queues (e.g., Azure queues, WebSphere message queues, Microsoft message queueing, etc), but any kind of reliable storage would work (as previously mentioned, HangFire prefers databases).

2. Then you’ll need a way to store the background work in that storage. HangFire uses an interesting method of serializing the delegate, which I am a little leery of. I’d prefer a solution that stored the background work semantically. This also means that the schema for the background work should be versioned.

3. The next thing is a host to perform the background work. These days I’d prefer an Azure WebJob, but you could also use a separate thread in an ASP.NET app. The host must be reliable in the sense that it should either complete the work or leave the work in storage to try again (or both), but it cannot remove the work from storage and then fail to complete it. Usually, the easiest way to satisfy this is to make all the work items idempoent and use a “lease” when reading from storage. Most reliable queues have built-in support for leases, but it’s up to you to make the background work idempotent.

4. The last thing in this kind of architecture is some kind of “poison message” recovery. That is, if there is some background work that gets into the system, and that work cannot complete successfully for whatever reason, there has to be some procedure for removing that piece of background work and setting it aside so that the system as a whole can continue processing.

5. Distributed architecture is complex. It is the most reliable and resilient option available.