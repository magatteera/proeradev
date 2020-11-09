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
                        "~/adminlte/plugins/datatables/jquery.dataTables.min.js",
                        "~/adminlte/plugins/chart.js/Chart.min.js",                       
                        "~/adminlte/plugins/datatables-bs4/js/dataTables.bootstrap4.min.js",
                        "~/adminlte/plugins/datatables-responsive/js/dataTables.responsive.min.js",
                        "~/adminlte/plugins/datatables-responsive/js/responsive.bootstrap4.min.js"));

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
                      "~/adminlte/plugins/fontawesome-free/css/all.min.css",
                      "~/adminlte/css/adminlte.min.css",
                      "~/adminlte/plugins/fontawesome-free/css/all.min.css",
                      "~/adminlte/plugins/datatables-bs4/css/dataTables.bootstrap4.min.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/adminlte/js").Include(
             "~/adminlte/js/adminlte.min.js"));
        }
    }
}
