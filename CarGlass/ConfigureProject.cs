using System.Data.Common;
using Autofac;
using CarGlass.Dialogs;
using CarGlass.Domain;
using CarGlass.Journal;
using CarGlass.Models.SMS;
using CarGlass.Repository;
using CarGlass.ViewModels.SMS;
using CarGlass.Views.SMS;
using QS.BaseParameters;
using QS.Deletion;
using QS.Deletion.Views;
using QS.Dialog;
using QS.Dialog.GtkUI;
using QS.DomainModel.UoW;
using QS.Journal.GtkUI;
using QS.Navigation;
using QS.Permissions;
using QS.Project.DB;
using QS.Project.Dialogs.GtkUI.ServiceDlg;
using QS.Project.Journal;
using QS.Project.Search.GtkUI;
using QS.Project.Services;
using QS.Project.Services.GtkUI;
using QS.Project.Versioning;
using QS.Project.ViewModels;
using QS.Project.Views;
using QS.Services;
using QS.Updater;
using QS.Updater.DB.Views;
using QS.Validation;
using QS.ViewModels;
using QS.ViewModels.Resolve;
using QS.Views.Resolve;
using QSOrmProject;
using QSProjectsLib;

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

			JournalsColumnsConfigs.RegisterColumns();
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
			builder.Register((ctx) => new AutofacViewModelsGtkPageFactory(AppDIContainer)).As<IViewModelsPageFactory>();
			//builder.Register((ctx) => new AutofacTdiPageFactory(AppDIContainer)).As<ITdiPageFactory>();
			builder.Register((ctx) => new AutofacViewModelsGtkPageFactory(AppDIContainer)).AsSelf();
			builder.RegisterType<GtkWindowsNavigationManager>().AsSelf().As<INavigationManager>().SingleInstance();
			builder.Register(cc => CreateGtkResolver()).As<IGtkViewResolver>();
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
			builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetAssembly(typeof(SendMessageViewModel)))
				.Where(t => t.IsAssignableTo<ViewModelBase>() && t.Name.EndsWith("ViewModel"))
				.AsSelf();
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

			#region СМС
			builder.RegisterType<OrderMessagesModel>().AsSelf();
			builder.RegisterType<ProstorSmsService>().AsSelf();
			#endregion

			AppDIContainer = builder.Build();
		}

		private static IGtkViewResolver CreateGtkResolver()
		{
			var namedResolver = new ClassNamesBaseGtkViewResolver(
				//typeof(RdlViewerView),
				typeof(SendMessageView),
				typeof(DeletionView),
				typeof(UpdateProcessView));

			var resolver = new RegisteredGtkViewResolver(namedResolver);
			resolver.RegisterView<JournalViewModelBase, JournalView>();
			return resolver;
		}
	}
}
