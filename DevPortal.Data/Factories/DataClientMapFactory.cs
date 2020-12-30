using DevPortal.Model;
using System;
using Environment = DevPortal.Model.Environment;

namespace DevPortal.Data.Factories
{
    public static class DataClientMapFactory
    {
        #region application group list item

        public static Func<ApplicationGroup, RecordUpdateInfo, ApplicationGroup> ApplicationGroupsMap
        {
            get
            {
                return (applicationGroup, recordUpdateInfo) =>
                {
                    applicationGroup.RecordUpdateInfo = recordUpdateInfo;

                    return applicationGroup;
                };
            }
        }

        #endregion

        #region application list item

        public static Func<Application, RecordUpdateInfo, Application> ApplicationsMap
        {
            get
            {
                return (application, recordUpdateInfo) =>
                {
                    application.RecordUpdateInfo = recordUpdateInfo;

                    return application;
                };
            }
        }

        #endregion

        #region environment list item

        public static Func<Environment, RecordUpdateInfo, Environment> EnvironmentsMap
        {
            get
            {
                return (environment, recordUpdateInfo) =>
                {
                    environment.RecordUpdateInfo = recordUpdateInfo;

                    return environment;
                };
            }
        }

        #endregion

        #region menu list item

        public static Func<MenuModel, RecordUpdateInfo, MenuModel> MenusMap
        {
            get
            {
                return (menu, recordUpdateInfo) =>
                {
                    menu.RecordUpdateInfo = recordUpdateInfo;

                    return menu;
                };
            }
        }

        #endregion

        #region database type list item

        public static Func<DatabaseType, RecordUpdateInfo, DatabaseType> DatabaseTypesMap
        {
            get
            {
                return (databaseType, recordUpdateInfo) =>
                {
                    databaseType.RecordUpdateInfo = recordUpdateInfo;

                    return databaseType;
                };
            }
        }

        #endregion

        #region user list item

        public static Func<User, RecordUpdateInfo, User> UsersMap
        {
            get
            {
                return (user, recordUpdateInfo) =>
                {
                    user.RecordUpdateInfo = recordUpdateInfo;

                    return user;
                };
            }
        }

        #endregion

        #region database list item

        public static Func<Database, RecordUpdateInfo, Database> DatabasesMap
        {
            get
            {
                return (database, recordUpdateInfo) =>
                {
                    database.RecordUpdateInfo = recordUpdateInfo;

                    return database;
                };
            }
        }

        #endregion

        #region database group list item

        public static Func<DatabaseGroup, RecordUpdateInfo, DatabaseGroup> DatabaseGroupsMap
        {
            get
            {
                return (databaseGroup, recordUpdateInfo) =>
                {
                    databaseGroup.RecordUpdateInfo = recordUpdateInfo;

                    return databaseGroup;
                };
            }
        }

        #endregion
    }
}