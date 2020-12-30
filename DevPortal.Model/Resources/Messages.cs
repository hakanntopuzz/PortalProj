namespace DevPortal.Model
{
    public static class Messages
    {
        public static string GeneralError => "Bir hata oluştu.";

        public static string InvalidRequest => "Geçersiz istek...";

        public static string NullParameterError => "Geçersiz parametre. Parametre null.";

        public static string AddingSucceeds => "Ekleme işlemi başarılı";

        public static string AddingFails => "Ekleme işlemi başarısız";

        public static string UpdateSucceeds => "Güncelleme işlemi başarılı";

        public static string UpdateFails => "Güncelleme işlemi başarısız";

        public static string DeleteFails => "Silme işlemi başarısız";

        public static string SortFails => "Sıralama işlemi başarısız";

        public static string ApplicationGroupNameExists => "Bu ada sahip uygulama grubu kayıtlıdır. Başka bir uygulama grubu adı belirleyin.";

        public static string ApplicationEnvironmentNotFound => "Uygulama ortamı bulunamadı.";

        public static string ApplicationEnvironmentCreated => "Yeni uygulama ortamı oluşturuldu.";

        public static string ApplicationEnvironmentFound => "Bu ortama sahip uygulama ortamı kayıtlıdır. Başka bir ortam seçiniz.";

        public static string ApplicationSvnRepositoryCreated => "Yeni uygulama svn deposu oluşturuldu.";

        public static string ApplicationSvnRepositoryUpdated => "Uygulama svn deposu güncellendi.";

        public static string ApplicationSvnRepositoryDeleted => "Uygulama svn deposu silindi.";

        public static string ApplicationSvnRepositoryNotFound => "Uygulama svn deposu bulunamadı.";

        public static string ApplicationEnvironmentUpdated => "Uygulama ortamı güncellendi.";

        public static string ApplicationEnvironmentDeleted => "Uygulama ortamı silindi.";

        public static string ApplicationGroupNotFound => "Uygulama grubu bulunamadı.";

        public static string MenuNotFound => "Menü bulunamadı.";

        public static string ApplicationGroupUpdated => "Uygulama grubu bilgileri güncellendi.";

        public static string ApplicationGroupDeleted => "Uygulama grubu silindi.";

        public static string ApplicationNotFound => "Uygulama bulunamadı.";

        public static string ApplicationCreated => "Yeni uygulama oluşturuldu.";

        public static string MenuCreated => "Yeni menü oluşturuldu.";

        public static string ApplicationUpdated => "Uygulama bilgileri güncellendi.";

        public static string ApplicationDeleted => "Uygulama silindi.";

        public static string MenuDeleted => "Menü silindi.";

        public static string MenuUpdated => "Menü bilgisi güncellendi.";

        public static string GeneralSettingsUpdated => "Genel ayarlar güncellendi.";

        public static string GeneralSettingsUpdateErrorOccured => "Genel ayarlar güncellenirken bir hata oluştu.";

        public static string InvalidParameterError => "Geçersiz parametre hatası";

        public static string InvalidFormatError => "Geçersiz format hatası";

        public static string ApplicationJenkinsJobCreated => "Yeni uygulama jenkins görevi oluşturuldu.";

        public static string ApplicationJenkinsJobNotFound => "Jenkins görevi bulunamadı.";

        public static string ApplicationJenkinsJobUpdated => "Jenkins görevi güncellendi.";

        public static string ApplicationJenkinsJobDeleted => "Jenkins görevi silindi.";

        public static string RelatedApplicationsExists => "Uygulama grubuna bağlı uygulama varken uygulama grubu silinemez. Önce uygulamaları silmeyi deneyin.";

        public static string ApplicationNameExists => "Bu ada sahip uygulama kayıtlıdır. Başka bir uygulama adı belirleyin.";

        public static string SonarQubeProjectNotFound => "SonarQube projesi bulunamadı.";

        public static string ApplicationSonarQubeProjectCreated => "Yeni uygulama sonarQube projesi oluşturuldu.";

        public static string ApplicationSonarQubeProjectUpdated => "SonarQube projesi güncellendi.";

        public static string ApplicationNugetPackageUpdated => "Uygulama nuget paketi bilgisi güncellendi.";

        public static string ApplicationSonarQubeProjectDeleted => "Uygulama sonarqube projesi silindi.";

        public static string GeneratePasswordRequireCharacterType => "İçerilecek karakter tiplerinden en az birini seçin.";

        public static string UserNotFound => "Kullanıcı bulunamadı.";

        public static string AddingNewUserSucceed => "Yeni kullanıcı oluşturuldu.";

        public static string InvalidUserInformation => "Kullanıcı bilgileri geçersiz.";

        public static string UserUpdated => "Kullanıcı bilgisi güncellendi.";

        public static string UserUpdateSucceeds => "Kullanıcı bilgileriniz başarıyla güncellendi.";

        public static string UserEmailAddressExists => "Bu e-posta adresine sahip kullanıcı kayıtlıdır. Başka bir kullanıcı e-posta adresi belirleyin.";

        public static string SvnUserNameExists => "Bu svn kullanıcı adına sahip kullanıcı kayıtlıdır. Başka bir svn kullanıcı adı belirleyin.";

        public static string UserDeleted => "Kullanıcı silindi.";

        public static string PasswordChangeSucceeds => "Parolanız başarıyla değiştirildi.";

        public static string PasswordResetRequestSucceeds => "Yeni parola oluşturma bilgileri e-posta adresinize gönderilmiştir.";

        public static string UserNotFoundByEmailAddress => "Sağlanan e-postaya bağlı bir kullanıcı bulunamadı.";

        public static string InvalidPasswordRequestToken => "Token değeri geçersiz.";

        public static string ResetPasswordSucceed => "Yeni parolanız başarıyla kaydedildi.";

        #region Identity Errors

        public static string PasswordRequiresNonAlphanumeric => "Parola en az bir alfanümerik olmayan karakter içermelidir.";

        public static string PasswordRequiresDigit => "Parola en az bir rakam içermelidir. ('0'-'9').";

        public static string PasswordRequiresLower => "Parola en az bir küçük harf içermelidir. ('a'-'z').";

        public static string PasswordRequiresUpper => "Parola en az bir büyük harf içermelidir. ('A'-'Z').";

        public static string DuplicateEmail => "{0} e-posta adresine sahip kullanıcı kaydı bulunmaktadır.";

        public static string DuplicateUserName => "{0} kullanıcı adına sahip kullanıcı kaydı bulunmaktadır.";

        public static string NotAuthorizedTransaction => "Bu işlem için yetkiniz yoktur.";

        public static string PasswordMismatch => "Hatalı parola.";

        #endregion

        #region Mail Subjects

        public static string ForgotPasswordMailSubject => "Parola Yenileme Talebi - DevPortal";

        #endregion

        public static string ApplicationNugetPackageNotFound => "Nuget paketi bulunamadı.";

        public static string ApplicationNugetPackageCreated => "Yeni uygulama nuget paketi oluşturuldu.";

        public static string ApplicationNugetPackageNameExists => "Bu nuget paketi başka bir uygulamaya ait olacak şekilde tanımlanmıştır. Başka bir uygulama nuget paketi seçin.";

        public static string ApplicationNugetPackageDeleted => "Uygulama nuget paketi silindi.";

        public static string EnvironmentNotFound => "Ortam bulunamadı.";

        public static string EnvironmentFound => "Bu ada sahip ortam kayıtlıdır.Başka bir ortam adı belirleyin.";

        public static string EnvironmentUpdated => "Ortam bilgileri güncellendi.";

        public static string EnvironmentDeleted => "Ortam silindi.";

        public static string RelatedApplicationEnvironmentsExists => "Ortama bağlı uygulama ortamı varken ortam silinemez. Önce uygulama ortamını silmeyi deneyin.";

        public static string DatabaseNotFound => "Veritabanı bulunamadı.";

        public static string DatabaseTypeNotFound => "Veritabanı tipi bulunamadı.";

        public static string DatabaseTypeAdded => "Yeni veritabanı tipi oluşturuldu.";

        public static string DatabaseTypeFound => "Bu ada sahip veritabanı tipi kayıtlıdır. Başka bir veritabanı tipi adı belirleyin.";

        public static string DatabaseTypeUpdated => "Veritabanı tipi bilgileri güncellendi.";

        public static string RelatedDatabaseTypeDatabaseExists => "Veritabanı tipine bağlı veritabanı varken veritabanı tipi silinemez. Önce veritabanlarının tiplerini değiştirmeyi deneyin.";

        public static string DatabaseTypeDeleted => "Veritabanı tipi silindi.";

        public static string ExternalDependencyNotFound => "Harici bağımlılık bulunamadı.";

        public static string ExternalDependencyUpdated => "Harici bağımlılık bilgileri güncellendi.";

        public static string ExternalDependencyDeleted => "Harici bağımlılık silindi.";

        public static string DatabaseGroupAdded => "Yeni veritabanı grubu oluşturuldu.";

        public static string DatabaseGroupFound => "Bu ada sahip veritabanı grubu kayıtlıdır. Başka bir veritabanı grubu adı belirleyin.";

        public static string DatabaseGroupNotFound => "Veritabanı grubu bulunamadı.";

        public static string DatabaseGroupUpdated => "Veritabanı grubu bilgileri güncellendi.";

        public static string DatabaseGroupDeleted => "Veritabanı grubu silindi.";

        public static string RelatedDatabaseGroupDatabaseExists => "Veritabanı grubuna bağlı veritabanı varken veritabanı grubu silinemez. Önce veritabanlarının gruplarını değiştirmeyi deneyin.";

        public static string DatabaseNameExists => "Bu ada sahip veritabanı kayıtlıdır. Başka bir veritabanı adı belirleyin.";

        public static string DatabaseUpdated => "Veritabanı bilgileri güncellendi.";

        public static string DatabaseFound => "Bu ada sahip veritabanı kayıtlıdır. Başka bir veritabanı adı belirleyin.";

        public static string DatabaseAdded => "Yeni veritabanı oluşturuldu.";

        public static string DatabaseDeleted => "Veritabanı silindi.";

        public static string JenkinsServerError => "Jenkins sunucusundan veri alınırken beklenmeyen bir hata oluştu.";

        public static string DatabaseDependencyNotFound => "Veritabanı bağımlılığı bulunamadı.";

        public static string NugetPackageDependencyNotFound => "Nuget Paketi bağımlılığı bulunamadı.";

        public static string DatabaseDependencyCreated => "Yeni veritabanı bağımlılığı oluşturuldu.";

        public static string NugetPackageDependencyCreated => "Yeni nuget paketi bağımlılığı oluşturuldu.";

        public static string DatabaseDependencyUpdated => "Veritabanı bağımlılık bilgileri güncellendi.";

        public static string ApplicationDependencyNotFound => "Uygulama bağımlılığı bulunamadı.";

        public static string ApplicationDependencyCreated => "Yeni uygulama bağımlılığı oluşturuldu.";

        public static string DatabaseDependencyDeleted => "Veritabanı bağımlılığı silindi.";

        public static string NugetPackageDependencyDeleted => "Nuget Paketi bağımlılığı silindi.";

        public static string ApplicationDependencyDeleted => "Uygulama bağımlılığı silindi.";

        public static string ApplicationDependencyUpdated => "Uygulama bağımlılık bilgileri güncellendi.";

        public static string BuildScriptCreationFails => "Derleme scripti oluşturulurken bir hata oluştu!";

        public static string FavoriteAdded => "Sayfa favorilere eklendi.";

        public static string FavoriteExists => "Bu sayfa zaten favorilerinizde kayıtlıdır.";

        public static string FavoriteDeleted => "Sayfa favorilerden çıkarıldı.";

        public static string FavoritePageSorted => "Sıralama işlemi başarılı.";

        #region svn admin

        public static string SvnRepositoryFolderCreated => "Svn Depo Klasörü oluşturuldu.";

        public static string SvnRepositoryFolderAlreadyExists => "Bu ada sahip bir Svn Depo Klasörü zaten mevcut.";

        #endregion

        public static string ApplicationBuildSettingsUpdated => "Uygulama derleme ayarları güncellendi.";
    }
}