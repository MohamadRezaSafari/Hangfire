using Hangfire;
using HangfireDemo.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpPost]
        public ActionResult CreateRecurringJob()
        {
            RecurringJob.AddOrUpdate("RecurringJob1",
                () => Console.WriteLine("Recurring Job Triggered"),
                "* * * * *");

            return Ok();
        }

        [HttpPost]
        public ActionResult CreateScheduledJob()
        {
            var scheduledDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduledDateTime);

            var job1 = BackgroundJob.Schedule(() => Console.WriteLine("Hi 2"), dateTimeOffset);
            var job2 = BackgroundJob.ContinueJobWith(job1, () => Console.WriteLine("Hi - continue 1"));
            var job3 = BackgroundJob.ContinueJobWith(job2, () => Console.WriteLine("Hi - continue 2"));
            BackgroundJob.ContinueJobWith(job3, () => Console.WriteLine("Hi - continue 3"));

            return Ok();
        }

        [HttpPost]
        public ActionResult CreateBackgroundJob()
        {
            //BackgroundJob.Enqueue(() => Console.WriteLine("Hey"));
            BackgroundJob.Enqueue<TestJob>(i => i.WriteLog("Background JOb Triggered"));

            return Ok();
        }
    }
}
