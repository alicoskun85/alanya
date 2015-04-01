using System.Web;
using System.Web.Optimization;

namespace Alanya
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/infobubble.js",
                      //"~/Content/materialize/js/waves.js",
                      "~/Scripts/CustomGoogleMapMarker.js",
                      "~/Scripts/jquery.mmenu.min.all.js",
                      "~/Content/lightSlider/js/jquery.lightSlider.js"
                      ));

            //bundles.Add(new ScriptBundle("~/bundles/material").Include(
            //         "~/Scripts/materialize/js/materialize.min.js"));
            //bundles.Add(new ScriptBundle("~/Content/materialcss").Include(
            //        "~/Scripts/materialize/js/materialize.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(

                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/panel.css",

                      "~/Content/lightSlider/css/lightSlider.css",
                      "~/Content/jquery.mmenu.all.css",
                      "~/Content/rate/jquery.raty.css"
                      ));
            BundleTable.EnableOptimizations = true;
        }
    }
}
