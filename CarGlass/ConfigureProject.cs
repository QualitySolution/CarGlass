using System;
using QSOrmProject;
using QSProjectsLib;
using CarGlass.Domain;

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

			OrmMain.ConfigureOrm(db, new System.Reflection.Assembly[] {
				System.Reflection.Assembly.GetAssembly (typeof(MainClass)),
			});

            OrmMain.AddObjectDescription<CarBrand>().DefaultTableView().SearchColumn("Наименование", i => i.Name).OrderAsc(i => i.Name).End();
			OrmMain.AddObjectDescription<CarModel>().Dialog<CarModelDlg>();
		}
	}
}
