using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace CozaStore.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundle(BundleCollection bundle)
        {

            bundle.Add(new StyleBundle("~/bundle/css").Include(
                "~/vendor/bootstrap/css/bootstrap.min.css",
                "~/fonts/font-awesome-4.7.0/css/font-awesome.min.css",
                "~/fonts/iconic/css/material-design-iconic-font.min.css",
                "~/fonts/linearicons-v1.0.0/icon-font.min.css",
                "~/vendor/animate/animate.css",
                "~/vendor/css-hamburgers/hamburgers.min.css",
                "~/vendor/animsition/css/animsition.min.css",
                "~/vendor/select2/select2.min.css",
                "~/vendor/daterangepicker/daterangepicker.css",
                "~/vendor/slick/slick.css",
                "~/vendor/MagnificPopup/magnific-popup.css",
                "~/vendor/perfect-scrollbar/perfect-scrollbar.css",
                "~/Content/util.css",
                "~/Content/main.css"));
            bundle.Add(new ScriptBundle("~/bundle/scriptHead").Include(
                "~/vendor/jquery/jquery-3.2.1.min.js",
                "~/Scripts/districts.min.js"));
            bundle.Add(new ScriptBundle("~/bundle/script").Include(
                "~/vendor/animsition/js/animsition.min.js",
                "~/vendor/bootstrap/js/popper.js",
                "~/vendor/bootstrap/js/bootstrap.min.js",
                "~/vendor/select2/select2.min.js",
                "~/vendor/daterangepicker/moment.min.js",
                "~/vendor/daterangepicker/daterangepicker.js",
                "~/vendor/slick/slick.min.js",
                "~/Scripts/slick-custom.js",
                "~/vendor/parallax100/parallax100.js",
                "~/vendor/MagnificPopup/jquery.magnific-popup.min.js",
                "~/vendor/isotope/isotope.pkgd.min.js",
                "~/vendor/sweetalert/sweetalert.min.js",
                "~/vendor/perfect-scrollbar/perfect-scrollbar.min.js"));
            bundle.Add(new StyleBundle("~/bundle/cssAdmin").Include(
                "~/assets/css/pace.min.css",
                "~/assets/plugins/vectormap/jquery-jvectormap-2.0.2.css",
                "~/assets/plugins/simplebar/css/simplebar.css",
                "~/assets/css/bootstrap.min.css",
                "~/assets/css/animate.css",
                "~/assets/css/icons.css",
                "~/assets/css/sidebar-menu.css",
                "~/assets/css/app-style.css"));
            bundle.Add(new ScriptBundle("~/bundle/scriptAdmin").Include(
               "~/assets/js/jquery.min.js",
               "~/assets/js/popper.min.js",
               "~/assets/js/bootstrap.min.js",
               "~/assets/plugins/simplebar/js/simplebar.js",
               "~/assets/js/sidebar-menu.js",
                "~/vendor/sweetalert/sweetalert.min.js",
               "~/assets/js/jquery.loading-indicator.js",
               "~/assets/js/app-script.js",
               "~/assets/plugins/Chart.js/Chart.min.js",
               "~/assets/js/index.js"));
            bundle.Add(new ScriptBundle("~/bundle/scriptLogin").Include(
               "~/assets/js/jquery.min.js",
               "~/assets/js/popper.min.js",
               "~/assets/js/bootstrap.min.js",
               "~/assets/js/sidebar-menu.js",
               "~/assets/js/app-script.js"));
        }
    }
}