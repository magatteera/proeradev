using System.Web;
using System.Web.Optimization;

namespace proera
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/datatables/jquery.dataTables.min.js",
                        "~/Scripts/chart.js/Chart.min.js",
                        "~/Scripts/datatables-bs4/js/dataTables.bootstrap4.min.js",
                        "~/Scripts/datatables-responsive/js/dataTables.responsive.min.js",
                        "~/Scripts/datatables-responsive/js/responsive.bootstrap4.min.js",
                        "~/Scripts/js1/adminlte.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilisez la version de développement de Modernizr pour le développement et l'apprentissage. Puis, une fois
            // prêt pour la production, utilisez l'outil de génération à l'adresse https://modernizr.com pour sélectionner uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            "~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/adminlte/css/adminlte.min.css",
                      "~/Content/datatables-bs4/css/dataTables.bootstrap4.min.css",
                      "~/Content/site.css"));

        }
    }
}
