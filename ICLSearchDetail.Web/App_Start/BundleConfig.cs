using System.Web;
using System.Web.Optimization;

namespace ICLSearchDetail.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Home/Index").Include(
               "~/ScriptsNg/Module/indexApp.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/common").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/font-awesome.min.css",
                "~/Content/loading-bar.css",
                "~/Content/Site.css"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/angular-bootstrap-ui").Include(
                "~/Scripts/angular.min.js",
                "~/Scripts/angular-resource.min.js",
                "~/Scripts/angular-animate.min.js",
                "~/Scripts/angular-touch.min.js",
                "~/Scripts/angular-cookies.min.js",
                "~/Scripts/angular-resource.min.js",
                "~/Scripts/angular-sanitize.min.js",
                "~/Scripts/angular-route.min.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
                "~/Scripts/angular-ui/ui-bootstrap.min.js",
                "~/Scripts/ui-bootstrap-tpls-2.5.0.js",
                "~/Scripts/loading-bar.js",
                "~/Scripts/date.format.js",
                "~/Scripts/angular-file-upload/ng-file-upload-shim.min.js",
                "~/Scripts/angular-file-upload/ng-file-upload.min.js"
                ));
        }
    }
}
