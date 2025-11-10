using System;
using System.Data.Common;
using System.IO;
using Autofac;
using CarGlass.Dialogs;
using CarGlass.Domain;
using CarGlass.Journal;
using CarGlass.Models.SMS;
using CarGlass.Repository;
using CarGlass.ViewModels.SMS;
using CarGlass.Views.SMS;
using NHibernate.Driver.MySqlConnector;
using QS.BaseParameters;
using QS.Configuration;
using QS.Deletion;
using QS.Deletion.Views;
using QS.Dialog;
using QS.Dialog.GtkUI;
using QS.Dialog.ViewModels;
using QS.Dialog.Views;
using QS.DomainModel.UoW;
using QS.ErrorReporting;
using QS.ErrorReporting.Handlers;
using QS.Extensions.Observable.Collections.List;
using QS.Journal.GtkUI;
using QS.Navigation;
using QS.Permissions;
using QS.Project.DB;
using QS.Project.Dialogs.GtkUI.ServiceDlg;
using QS.Project.Domain;
using QS.Project.Journal;
using QS.Project.Search;
using QS.Project.Search.GtkUI;
using QS.Project.Services;
using QS.Project.Services.GtkUI;
using QS.Project.Versioning;
using QS.Project.ViewModels;
using QS.Project.Views;
using QS.Services;
using QS.Updater;
using QS.Updater.App;
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
				.Dialect<MySQL57ExtendedDialect>()
				.Driver<MySqlConnectorDriver>()
				.ConnectionString(QSMain.ConnectionString)
				.ShowSql()
				.FormatSql();

			OrmConfig.Conventions = new[] { new ObservableListConvention() };
			OrmConfig.ConfigureOrm(db, new System.Reflection.Assembly[] {
				System.Reflection.Assembly.GetAssembly (typeof(MainClass)),
				System.Reflection.Assembly.GetAssembly (typeof(UserBase)),
			});

            OrmMain.AddObjectDescription<CarBrand>().DefaultTableView().SearchColumn("Наименование", i => i.Name).OrderAsc(i => i.Name).End();
			OrmMain.AddObjectDescription<CarModel>().Dialog<CarModelDlg>();
			OrmMain.AddObjectDescription<StoreItem>().Dialog<StoreItemDlg>();

			JournalsColumnsConfigs.RegisterColumns();
		}

		public static ILifetimeScope AppDIContainer;
		public static IContainer StartupContainer;
		
		static void AutofacStartupConfig(ContainerBuilder containerBuilder)
		{
			#region Настройка
			containerBuilder.Register(c => new IniFileConfiguration(
					Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarGlass.ini")))
				.As<IChangeableConfiguration>().AsSelf().SingleInstance();
			#endregion
			
			#region GtkUI
			containerBuilder.RegisterType<GtkMessageDialogsInteractive>().As<IInteractiveMessage>();
			containerBuilder.RegisterType<GtkQuestionDialogsInteractive>().As<IInteractiveQuestion>();
			containerBuilder.RegisterType<GtkInteractiveService>().As<IInteractiveService>();
			containerBuilder.RegisterType<GtkGuiDispatcher>().As<IGuiDispatcher>();
			containerBuilder.RegisterType<GtkRunOperationService>().As<IRunOperationService>();
			#endregion GtkUI

			#region View
			containerBuilder.RegisterType<GtkViewFactory>().As<IGtkViewFactory>();
			#endregion

			#region Versioning
			containerBuilder.RegisterType<ApplicationVersionInfo>().As<IApplicationInfo>();
			#endregion

			#region ErrorReporting
			containerBuilder.RegisterType<DesktopErrorReporter>().As<IErrorReporter>();
			containerBuilder.RegisterType<LogService>().As<ILogService>();
			#if DEBUG
			containerBuilder.Register(c => new ErrorReportingSettings(false, true, false, 300)).As<IErrorReportingSettings>();
			#else
			containerBuilder.Register(c => new  ErrorReportingSettings(true, false, true, 300)).As<IErrorReportingSettings>();
			#endif

			containerBuilder.RegisterType<MySqlExceptionErrorNumberLogger>().As<IErrorHandler>();
			containerBuilder.RegisterType<MySqlException1055OnlyFullGroupBy>().As<IErrorHandler>();
			containerBuilder.RegisterType<MySqlException1366IncorrectStringValue>().As<IErrorHandler>();
			containerBuilder.RegisterType<MySqlExceptionAccessDenied>().As<IErrorHandler>();
			containerBuilder.RegisterType<NHibernateFlushAfterException>().As<IErrorHandler>();
			containerBuilder.RegisterType<NHibernateStaleObjectStateException>().As<IErrorHandler>();
			containerBuilder.RegisterType<ConnectionIsLost>().As<IErrorHandler>();
			#endregion
			
			#region Обновления и версии
			containerBuilder.RegisterModule(new UpdaterDesktopAutofacModule());
			containerBuilder.RegisterModule(new UpdaterAppAutofacModule());
			containerBuilder.RegisterModule(new UpdaterDBAutofacModule());
			containerBuilder.Register(c => MakeUpdateConfiguration()).AsSelf();
			#endregion

			#region Временные будут переопределены
			containerBuilder.RegisterType<ProgressWindowViewModel>().AsSelf();
			containerBuilder.RegisterType<GtkWindowsNavigationManager>().AsSelf().As<INavigationManager>().SingleInstance();
			containerBuilder.Register((ctx) => new AutofacViewModelsGtkPageFactory(StartupContainer)).As<IViewModelsPageFactory>();
			containerBuilder.Register(cc => new ClassNamesBaseGtkViewResolver(cc.Resolve<IGtkViewFactory>(),
				typeof(UpdateProcessView),
				typeof(ProgressWindowView)
			)).As<IGtkViewResolver>();
			#endregion
		}

		public static void AutofacClassConfig(ContainerBuilder builder)
		{
			#region База
			builder.RegisterType<DefaultUnitOfWorkFactory>().As<IUnitOfWorkFactory>();
			builder.RegisterType<DefaultSessionProvider>().As<ISessionProvider>();
			builder.Register(c => new MySqlConnectionFactory(QS.Project.DB.Connection.ConnectionString)).As<IConnectionFactory>();
			builder.Register<DbConnection>(c => c.Resolve<IConnectionFactory>().OpenConnection()).AsSelf().InstancePerLifetimeScope();
			builder.RegisterType<ParametersService>().UsingConstructor(typeof(Func<DbConnection>)).AsSelf().SingleInstance();
			builder.Register(c => QSProjectsLib.QSMain.ConnectionStringBuilder).AsSelf();
			builder.RegisterType<NhDataBaseInfo>().As<IDataBaseInfo>();
			builder.RegisterType<MySQLProvider>().As<IMySQLProvider>();
			#endregion
			
			#region Пользователь
			using (var uow = UnitOfWorkFactory.CreateWithoutRoot()) {
				var user = uow.GetById<UserBase>(QSMain.User.Id);
				if(user != null) {
					builder.Register(c => user).As<IUserInfo>();
					builder.Register(c => new UserService(user)).As<IUserService>();
					//FIXME Временно для старых диалогов
					ServicesConfig.UserService = new UserService(user);
				}
			}
			#endregion

			#region Сервисы
			#region GtkUI
			builder.RegisterType<GtkValidationViewFactory>().As<IValidationViewFactory>();
			#endregion GtkUI
			#region Удаление
			builder.RegisterModule(new DeletionAutofacModule());
			builder.RegisterType<DeleteEntityGUIService>().As<IDeleteEntityService>();
			builder.Register(x => DeleteConfig.Main).AsSelf();
			#endregion
			//FIXME Нужно в конечном итоге попытаться избавится от CommonService вообще.
			builder.RegisterType<CommonServices>().As<ICommonServices>();
			builder.RegisterType<ObjectValidator>().As<IValidator>();
			//FIXME Реализовать везде возможность отсутствия сервиса прав, чтобы не приходилось создавать то что не используется
			builder.RegisterType<DefaultAllowedPermissionService>().As<IPermissionService>();
			builder.RegisterType<CommonMessages>().AsSelf();
			#endregion

			#region Навигация
			builder.Register(ctx => new ClassNamesHashGenerator(null)).As<IPageHashGenerator>();
			//builder.Register(ctx => new ClassNamesHashGenerator(new[] { new RDLReportsHashGenerator() })).As<IPageHashGenerator>();
			//builder.Register((ctx) => new AutofacTdiPageFactory(AppDIContainer)).As<ITdiPageFactory>();
			builder.Register(cc => new ClassNamesBaseGtkViewResolver(cc.Resolve<IGtkViewFactory>(), 
				typeof(SendMessageView),
				typeof(DeletionView),
				typeof(UpdateProcessView)
			)).As<IGtkViewResolver>();
			builder.RegisterDecorator<IGtkViewResolver>((c, p, i) => 
				new RegisteredGtkViewResolver(c.Resolve<IGtkViewFactory>(), i)
					.RegisterView<JournalViewModelBase, JournalView>()
					.RegisterView<SearchViewModel, OneEntrySearchView>());
			builder.RegisterType<GtkApplicationQuitService>().As<IApplicationQuitService>();
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
		}
	}
}
