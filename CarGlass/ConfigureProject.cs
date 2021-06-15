using QSOrmProject;
using QSProjectsLib;
using CarGlass.Domain;
using CarGlass.Dialogs;
using QS.Project.DB;
using Autofac;
using QS.Project.Versioning;
using QS.Updater;
using CarGlass.Repository;
using QS.ViewModels.Resolve;
using QS.ViewModels;
using QS.Project.Search.GtkUI;
using QS.Project.Views;
using QS.Project.ViewModels;
using QS.Navigation;
using QS.Deletion.Views;
using QS.Updater.DB.Views;
using QS.Views.Resolve;
using QS.Dialog.GtkUI;
using QS.Project.Services.GtkUI;
using QS.Validation;
using QS.Dialog;
using QS.Project.Dialogs.GtkUI.ServiceDlg;
using QS.Services;
using QS.Project.Services;
using QS.Deletion;
using QS.Permissions;
using QS.DomainModel.UoW;
using System.Data.Common;
using QS.BaseParameters;

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

		public static Autofac.IContainer AppDIContainer;

		static void AutofacClassConfig()
		{
			var builder = new ContainerBuilder();

			#region База
			builder.RegisterType<DefaultUnitOfWorkFactory>().As<IUnitOfWorkFactory>();
			builder.RegisterType<DefaultSessionProvider>().As<ISessionProvider>();
			builder.Register(c => new MySqlConnectionFactory(QS.Project.DB.Connection.ConnectionString)).As<IConnectionFactory>();
			builder.Register<DbConnection>(c => c.Resolve<IConnectionFactory>().OpenConnection()).AsSelf().InstancePerLifetimeScope();
			builder.RegisterType<ParametersService>().AsSelf();
			builder.Register(c => QSProjectsLib.QSMain.ConnectionStringBuilder).AsSelf();
			builder.RegisterType<NhDataBaseInfo>().As<IDataBaseInfo>();
			builder.RegisterType<MySQLProvider>().As<IMySQLProvider>();
			#endregion

			#region Сервисы
			#region GtkUI
			builder.RegisterType<GtkMessageDialogsInteractive>().As<IInteractiveMessage>();
			builder.RegisterType<GtkQuestionDialogsInteractive>().As<IInteractiveQuestion>();
			builder.RegisterType<GtkInteractiveService>().As<IInteractiveService>();
			builder.RegisterType<GtkValidationViewFactory>().As<IValidationViewFactory>();
			builder.RegisterType<GtkGuiDispatcher>().As<IGuiDispatcher>();
			builder.RegisterType<GtkRunOperationService>().As<IRunOperationService>();
			#endregion GtkUI
			#region Удаление
			builder.RegisterModule(new DeletionAutofacModule());
			builder.RegisterType<DeleteEntityGUIService>().As<IDeleteEntityService>();
			builder.Register(x => DeleteConfig.Main).AsSelf();
			#endregion
			//FIXME Нужно в конечнои итоге попытаться избавится от CommonServce вообще.
			builder.RegisterType<CommonServices>().As<ICommonServices>();
			builder.RegisterType<UserService>().As<IUserService>();
			builder.RegisterType<ObjectValidator>().As<IValidator>();
			//FIXME Реализовать везде возможность отсутствия сервиса прав, чтобы не приходилось создавать то что не используется
			builder.RegisterType<DefaultAllowedPermissionService>().As<IPermissionService>();
			builder.RegisterType<CommonMessages>().AsSelf();
			#endregion

			#region Навигация
			builder.Register(ctx => new ClassNamesHashGenerator(null)).As<IPageHashGenerator>();
			//builder.Register(ctx => new ClassNamesHashGenerator(new[] { new RDLReportsHashGenerator() })).As<IPageHashGenerator>();
			builder.Register((ctx) => new AutofacViewModelsTdiPageFactory(AppDIContainer)).As<IViewModelsPageFactory>();
			builder.Register((ctx) => new AutofacTdiPageFactory(AppDIContainer)).As<ITdiPageFactory>();
			builder.Register((ctx) => new AutofacViewModelsGtkPageFactory(AppDIContainer)).AsSelf();
			builder.RegisterType<GtkWindowsNavigationManager>().AsSelf().As<INavigationManager>().SingleInstance();
			builder.Register(cc => new ClassNamesBaseGtkViewResolver(
				//typeof(RdlViewerView),
				//typeof(OrganizationView),
				typeof(DeletionView),
				typeof(UpdateProcessView)
			)).As<IGtkViewResolver>();
			#endregion

			#region Главное окно
			//builder.Register((ctx) => MainWin.ProgressBar).As<IProgressBarDisplayable>();
			#endregion

			#region Старые диалоги
			//builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetAssembly(typeof(IncomeDocDlg)))
				//.Where(t => t.IsAssignableTo<ITdiTab>() && t.Name.EndsWith("Dlg"))
				//.AsSelf();
			#endregion

			#region Старые общие диалоги
			builder.RegisterType<OrmReference>().AsSelf();
			builder.RegisterType<ReferenceRepresentation>().AsSelf();
			#endregion

			#region Отдельные диалоги
			builder.RegisterType<AboutView>().AsSelf();
			builder.RegisterType<AboutViewModel>().AsSelf();
			#endregion

			#region Rdl
			//builder.RegisterType<RdlViewerViewModel>().AsSelf();
			#endregion

			#region Журналы
			builder.RegisterType<OneEntrySearchView>().Named<Gtk.Widget>("GtkJournalSearchView");
			#endregion

			#region ViewModels
			builder.Register(x => new AutofacViewModelResolver(AppDIContainer)).As<IViewModelResolver>();
			//builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetAssembly(typeof(OrganizationViewModel)))
				//.Where(t => t.IsAssignableTo<ViewModelBase>() && t.Name.EndsWith("ViewModel"))
				//.AsSelf();
			#endregion

			#region Repository
			builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetAssembly(typeof(WorkOrderRepository)))
				.Where(t => t.Name.EndsWith("Repository"))
				.AsSelf();
			#endregion

			#region Обновления и версии
			builder.RegisterType<ApplicationVersionInfo>().As<IApplicationInfo>();
			builder.RegisterModule(new UpdaterAutofacModule());
			builder.Register(c => MainClass.MakeUpdateConfiguration()).AsSelf();
			#endregion

			#region Облако
			//builder.Register(c => new CloudClientService(QSSaaS.Session.SessionId)).AsSelf().SingleInstance();
			#endregion

			#region Настройка
			//builder.Register(c => new IniFileConfiguration(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "workwear.ini"))).As<IChangeableConfiguration>().AsSelf();
			#endregion

			AppDIContainer = builder.Build();
		}
	}
}
