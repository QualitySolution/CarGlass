using QSOrmProject;
using QSProjectsLib;
using CarGlass.Domain;
using CarGlass.Dialogs;
using QS.Project.DB;

namespace CarGlass
{
	partial class MainClass
	{
		static void CreateBaseConfig()
		{
			logger.Info("Настройка параметров базы...");

			// Настройка ORM
			var db = FluentNHibernate.Cfg.Db.MySQLConfiguration.Standard
				.ConnectionString(QSMain.ConnectionString)
				.ShowSql()
				.FormatSql();

			OrmConfig.ConfigureOrm(db, new System.Reflection.Assembly[] {
				System.Reflection.Assembly.GetAssembly (typeof(MainClass)),
			});

            OrmMain.AddObjectDescription<CarBrand>().DefaultTableView().SearchColumn("Наименование", i => i.Name).OrderAsc(i => i.Name).End();
			OrmMain.AddObjectDescription<CarModel>().Dialog<CarModelDlg>();
			OrmMain.AddObjectDescription<StoreItem>().Dialog<StoreItemDlg>();
		}
	}
}
