namespace DevPortal.Framework
{
    public static class CsvColumnNames
    {
        #region svn

        public static string SvnRepositoryName => "Depo Adı";

        public static string LastUpdated => "Son Güncelleme";

        public static string[] SvnRepositoryList => new[] { SvnRepositoryName, LastUpdated };

        #endregion

        #region applications

        public static string Application => "Uygulama Adı";

        public static string ApplicationGroup => "Uygulama Grubu";

        public static string ApplicationTypeName => "Uygulama Tipi";

        public static string Status => "Uygulama Durumu";

        public static string CreateDate => "Oluşturma Tarihi";

        public static string CreatedUserEmail => "Oluşturan Kullanıcı E-posta";

        public static string ModifiedUserEmail => "Güncelleyen Kullanıcı E-posta";

        public static string ModifiedDate => "Güncellenme Tarihi";

        public static string[] ApplicationList => new[] { Application, ApplicationGroup, ApplicationTypeName, Status };

        public static string[] ApplicationDependenciesList => new[] { Application, ApplicationGroup, Description, CreateDate, CreatedUserEmail, ModifiedDate, ModifiedUserEmail };

        #endregion

        #region database

        public static string Database => "Veritabanı Adı";

        public static string DatabaseGroup => "Veritabanı Grubu";

        public static string DatabaseType => "Veritabanı Tipi";

        public static string[] DatabaseList => new[] { Database, DatabaseGroup, DatabaseType };

        public static string[] DatabaseDependenciesList => new[] { Database, DatabaseGroup, Description, CreateDate, CreatedUserEmail, ModifiedDate, ModifiedUserEmail };

        #endregion

        #region external

        public static string ExternalDependency => "Harici bağımlılık Adı";

        public static string[] ExternalDependenciesList => new[] { ExternalDependency, Description, CreateDate, CreatedUserEmail, ModifiedDate, ModifiedUserEmail };

        #endregion

        #region nuget

        public static string NugetPackageName => "Paket Adı";

        public static string Version => "Versiyon";

        public static string PackageUrl => "Paket Url";

        public static string Description => "Açıklama";

        public static string Tags => "Etiketler";

        public static string Published => "Yayınlama Tarihi";

        public static string[] NugetList => new[] { NugetPackageName, Version, Description, Tags, Published };

        public static string[] NugetPackageDependenciesList => new[] { NugetPackageName, PackageUrl, CreateDate, CreatedUserEmail, ModifiedDate, ModifiedUserEmail };

        #endregion
    }
}