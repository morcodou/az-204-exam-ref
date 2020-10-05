using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationInsightsTelemetry.Controllers
{
    public class HomeController : Controller
    {
        private TelemetryClient telemetry;
        private double indexLoadCounter;
        public HomeController()
        {
            //Create a TelemetryClient that can be used during the life of the
            // Controller.
            telemetry = new TelemetryClient();

            //Initialize some counters for the custom metrics.
            //This is a fake metric just for demo purposes.
            indexLoadCounter = new Random().Next(1000);
        }
        public ActionResult Index()
        {
            //This example is trivial as ApplicationInsights already registered the
            // load of the page.
            //You can use this example for tracking different events in the
            // application.
            telemetry.TrackEvent("Loading the Index page");
            //Before you can submit a custom metric, you need to use the GetMetric
            //method.
            telemetry.GetMetric("CountOfIndexPageLoads").TrackValue(indexLoadCounter);


            //This trivial example shows how to track exceptions using Application
            //Insights.
            //You can also send trace message to Application Insights.
            try
            {
                Trace.TraceInformation("Raising a trivial exception");
                throw new System.Exception(@"Trivial Exception for testing Tracking
                Exception feature in Application Insights");
            }
            catch (System.Exception ex)
            {
                Trace.TraceError("Capturing and managing the trivial exception");
                telemetry.TrackException(ex);
            }


            //You need to instruct the TelemetryClient to send all in-memory data to
            // the ApplicationInsights.
            telemetry.Flush();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";


            //This example is trivial as ApplicationInsights already registers the
            //load of the page.
            //You can use this example for tracking different events in the
            // application.
            telemetry.TrackEvent("Loading the About page");

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            //This example is trivial as ApplicationInsights already registers the load
            //of the page.
            //You can use this example for tracking different events in the
            // application.
            telemetry.TrackEvent("Loading the Contact page");

            return View();
        }
    }
}