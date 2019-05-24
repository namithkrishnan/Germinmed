using System.Web;
using System.Web.Optimization;

namespace Germinmed
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
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
                 "~/Scripts/bootstrap3-wysihtml5.all.js",

                 "~/Scripts/js/flaunt.js",
                 "~/Scripts/js/owl.carousel.js",
                  "~/Scripts/js/main.js",
                   "~/Scripts/js/bootsnav.js",
                    "~/Scripts/js//slider/jquery.themepunch.tools.min.js",
                     "~/Scripts/js/slider/jquery.themepunch.revolution.min.js",
                      "~/Scripts/js/slider/custom.js",
                     

                         "~/Scripts/js/slider/revolution.extension.slideanims.min.js",
                            "~/Scripts/js/slider/revolution.extension.layeranimation.min.js",
                               "~/Scripts/js/slider/revolution.extension.navigation.min.js",
                                  "~/Scripts/js/slider/revolution.extension.parallax.min.js"
                     ));


            bundles.Add(new StyleBundle("~/Content/admin-style").Include(
   "~/Content/bootstrap.css",
     "~/Content/css/font-awesome.min.css",
     "~/Contentionicons.min.css",
     "~/Content/site.css",
   "~/Content/_all-skins.min.css",
   "~/Content/AdminLTE.min.css",
    "~/Content/bootstrap3-wysihtml5.css",
     "~/Content/blue.css"));

      bundles.Add(new StyleBundle("~/Content/css-style").Include(
             "~/Content/bootstrap.css",
     "~/Content/css/font-awesome.min.css",
      "~/Content/css/owl.carousel.min.css",
         "~/Content/css/owl.theme.default.min.css",
            "~/Content/css/slider/settings.css",
           "~/Content/css/slider/ALightBox.css",
            "~/Content/css/slider/lightslider.css",
                  "~/Content/css/mega-menu.css",
                     "~/Content/css/bootsnav.css",
                     "~/Content/css/style.css",
                        "~/Content/css/responsive.css"  ));


        }
    }
}
