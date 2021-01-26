using System.Web;
using System.Web.Optimization;

namespace NegociosElectronicosII
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/ChartJS").Include(
                        "~/Scripts/Chart.bundle.js",
                        "~/Scripts/Chart.bundle.min.js",
                        "~/Scripts/Chart.js",
                        "~/Scripts/Chart.min.js",
                        "~/Scripts/chartjs-plugin-datalabels.js",
                        "~/Scripts/chartjs-plugin-datalabels.min.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/loginJS").Include(
                       "~/Content/ContentLogin/Bootstrap/js/bootstrap.min.js",
                       "~/Content/ContentLogin/Bootstrap/js/jquery.min.js"
                       ));
            bundles.Add(new StyleBundle("~/Content/loginCSS").Include(
                      "~/Content/ContentLogin/Bootstrap/css/bootstrap.min.css",
                      "~/Content/ContentLogin/Bootstrap/css/Fontawesome.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/principalCSS").Include(
                      "~/Content/ui-ecommerce/css/bootstrap.css",
                      "~/Content/ui-ecommerce/fonts/fontawesome/css/fontawesome-all.min.css",
                      "~/Content/ui-ecommerce/plugins/owlcarousel/assets/owl.carousel.min.css",
                      "~/Content/ui-ecommerce/plugins/owlcarousel/assets/owl.theme.default.css",
                      "~/Content/ui-ecommerce/css/ui.css",
                      "~/Content/ui-ecommerce/css/responsive.css",
                      "~/Content/ui-ecommerce/plugins/fancybox/fancybox.min.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/principalJS").Include(
                       "~/Content/ui-ecommerce/js/jquery-2.0.0.min.js",
                       "~/Content/ui-ecommerce/js/bootstrap.bundle.min.js",
                       "~/Content/ui-ecommerce/plugins/owlcarousel/owl.carousel.min.js",
                       "~/Content/ui-ecommerce/js/script.js",
                       "~/Content/ui-ecommerce/plugins/fancybox/fancybox.min.js"
                       ));

            /*----------------------plugin: owl carousel -------------------- */

            bundles.Add(new ScriptBundle("~/bundles/CarouselJS").Include(
                       "~/Content/ui-ecommerce/plugins/owlcarousel/owl.carousel.min.js"
                       ));

            bundles.Add(new StyleBundle("~/Content/CarouselCSS").Include(
                      "~/Content/ui-ecommerce/plugins/owlcarousel/assets/owl.carousel.min.css",
                      "~/Content/ui-ecommerce/plugins/owlcarousel/assets/owl.theme.default.css"
                      ));

            /*----------------------custom javascript -------------------- */
            bundles.Add(new ScriptBundle("~/bundles/CustomJs").Include(
                       "~/Content/ui-ecommerce/js/script.js"
                       ));

            #region datatables

            bundles.Add(new StyleBundle("~/Content/DataTablesCss").Include(
                     "~/Content/datatables/css/jquery.dataTables.css"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                      "~/Content/datatables/js/jquery.dataTables.js",
                      "~/Content/datatables/js/jquery.dataTables.min.js"
                      ));

            #endregion

            #region blockui

            bundles.Add(new ScriptBundle("~/bundles/Block").Include(
                     "~/Scripts/jquery.blockUI.js"));

            #endregion
        }
    }
}
