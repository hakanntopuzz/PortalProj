const applicationId = $("#applicationId").val();
const applicationName = $("#applicationName").val();

$(document).ready(function () {
    $('.link-tab').click(function () {
        window.location.href($(this));
    });

    $('#nav-tab a').on('click', function (e) {
        e.preventDefault();

        var tabId = $(this).attr("aria-controls");
        $(".tab-content .tab-pane ").removeClass("show active");
        var selectedTabContent = $("#" + tabId + "");
        selectedTabContent.addClass("show active");
    });

    $('#dependencies-tab').on('click', function (e) {
        GetApplicationDependenciesByApplicationId(applicationId);
        GetDatabaseDependenciesByApplicationId(applicationId);
        GetExternalDependenciesByApplicationId(applicationId);
        GetNugetPackageDependenciesByApplicationId(applicationId);
    });

    $("#export-dependencies-as-wiki").on("click", () => {
        exportDependenciesAsWikiText();
    });

    $("#export-dependencies-as-wiki-file").on("click", () => {
        exportDependenciesAsWikiFile();
    });

    $('#environment-tab').on('click', function (e) {
        $.ajax({
            dataType: 'json',
            type: 'GET',
            url: Urls.GetApplicationEnvironments,
            data: { applicationId: applicationId },
            success: function (result) {
                if (result.data.length !== 0) {
                    $("#environment-tab-content-table-tbody").empty();

                    $.each(result.data, function (index, value) {
                        var hasLog = value.hasLog === true ? 'Görüntülenebilir' : 'Görüntülenemez';

                        var row = generateEnviromentTableRow(value.environmentName, hasLog, value.url === null ? "" : value.url, value.id);
                        $("#environment-tab-content-table-tbody").append(row);
                    });
                    $("#environment-count").empty();
                    $("#environment-count").append(result.data.length + " ortam");
                }
                else {
                    $("#environment-tab-content-table-tbody").empty();
                    var nullRow = generateNullTableRow(4);
                    $("#environment-tab-content-table-tbody").append(nullRow);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    });

    $('#nuget-tab').on('click', function (e) {
        $.ajax({
            dataType: 'json',
            type: 'GET',
            url: Urls.GetNugetPackages,
            data: { applicationId: applicationId },
            success: function (result) {
                $("#nuget-tab-content-table-tbody").empty();
                if (result.data.length !== 0) {
                    $.each(result.data, function (index, value) {
                        var row = generateNugetTableRow(value.nugetPackageName, value.packageUrl, value.nugetPackageId);
                        $("#nuget-tab-content-table-tbody").append(row);
                    });
                    $("#nuget-count").empty();
                    $("#nuget-count").append(result.data.length + " paket");
                }
                else {
                    var nullRow = generateNullTableRow(3);
                    $("#nuget-tab-content-table-tbody").append(nullRow);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    });

    $('#svn-tab').on('click', function (e) {
        $.ajax({
            dataType: 'json',
            type: 'GET',
            url: Urls.GetSvnRepositories,
            data: { applicationId: applicationId },
            success: function (result) {
                $("#svn-tab-content-table-tbody").empty();
                if (result.data.length !== 0) {
                    $.each(result.data, function (index, value) {
                        var row = generateSvnTableRow(value.svnUrl, value.svnRepositoryTypeName, value.id);
                        $("#svn-tab-content-table-tbody").append(row);
                    });
                    $("#svn-count").empty();
                    $("#svn-count").append(result.data.length + " depo");
                }
                else {
                    var nullRow = generateNullTableRow(3);
                    $("#svn-tab-content-table-tbody").append(nullRow);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    });

    $('#jenkins-tab').on('click', function (e) {
        $.ajax({
            dataType: 'json',
            type: 'GET',
            url: Urls.GetJenkinsJobs,
            data: { applicationId: applicationId },
            success: function (result) {
                $("#jenkins-tab-content-table-tbody").empty();
                if (result.data.length !== 0) {
                    $.each(result.data, function (index, value) {
                        var row = generateJenkinsTableRow(value.jobUrl, value.jenkinsJobTypeName, value.jenkinsJobId);
                        $("#jenkins-tab-content-table-tbody").append(row);
                    });
                    $("#jenkins-count").empty();
                    $("#jenkins-count").append(result.data.length + " görev");
                }
                else {
                    var nullRow = generateNullTableRow(3);
                    $("#jenkins-tab-content-table-tbody").append(nullRow);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    });

    $('#sonarqube-tab').on('click', function (e) {
        $.ajax({
            dataType: 'json',
            type: 'GET',
            url: Urls.GetSonarqubeProjects,
            data: { applicationId: applicationId },
            success: function (result) {
                $("#sonarqube-tab-content-table-tbody").empty();
                if (result.data.length !== 0) {
                    $.each(result.data, function (index, value) {
                        var row = generateSonarqubeTableRow(value.projectUrl, value.sonarqubeProjectTypeName, value.sonarqubeProjectId);
                        $("#sonarqube-tab-content-table-tbody").append(row);
                    });
                    $("#sonarqube-count").empty();
                    $("#sonarqube-count").append(result.data.length + " proje");
                }
                else {
                    var nullRow = generateNullTableRow(3);
                    $("#sonarqube-tab-content-table-tbody").append(nullRow);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    });

    $("#deleteApplication").on("click", () => {
        deleteApplication();
    });

    $('#build-settings-tab').on('click', function (e) {
        getBuildSettings(applicationId);
    });
});

function deleteApplication() {
    Swal.fire({
        title: 'Emin misiniz?',
        text: "Bu kayıt kalıcı olarak silinecektir!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#c70039',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Sil',
        cancelButtonText: 'Vazgeç'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                dataType: 'json',
                type: 'POST',
                url: "/Application/Delete",
                data: { id: applicationId },
                success: function (result) {
                    if (result.isSuccess === true) {
                        redirectToWindowLocation(result.redirectUrl);
                    }
                    else {
                        if (result.message !== null) {
                            SweetAlertError(result.message);
                        }
                        else {
                            SweetAlertError();
                        }
                    }
                },
                error: function (err) {
                    console.log(err);
                    SweetAlertError();
                }
            });
        }
    });
}

function SweetAlertError(message) {
    if (message === "") {
        Swal.fire('İşlem başarısız!', 'Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.', 'warning');
    }
    else {
        Swal.fire('İşlem başarısız!', message, 'warning');
    }
}

function redirectToWindowLocation(redirectUrl) {
    window.location.href = redirectUrl;
}

function exportDependenciesAsWikiFile() {
    var applicationId = $("#applicationId").val();
    var url = Urls.ExportDependenciesAsWikiFile + "?applicationId=" + applicationId;
    window.location.href = url;
}

function exportDependenciesAsWikiText() {
    var applicationId = $("#applicationId").val();

    $.ajax({
        url: Urls.ExportDependenciesAsWiki,
        type: 'get',
        data: { applicationId: applicationId },
        success: function (response) {
            $('#dependencies-wiki-text').html(response);
            $('#dependenciesModal').modal('show');
        }
    });
}

function GetApplicationDependenciesByApplicationId(applicationId) {
    $.ajax({
        dataType: 'json',
        type: 'GET',
        url: Urls.GetApplicationDependenciesByApplicationId,
        data: { applicationId: applicationId },
        success: function (result) {
            $("#application-tab-content-table-tbody").empty();
            if (result.data.length !== 0) {
                $.each(result.data, function (index, value) {
                    var row = generateApplicationDependenciesTableRow(value.name, value.applicationGroupName, value.id, value.dependedApplicationId);
                    $("#application-tab-content-table-tbody").append(row);
                });
                $("#application-count").empty();
                $("#application-count").append(result.data.length + " uygulama");
            }
            else {
                var nullRow = generateNullTableRow(3);
                $("#application-tab-content-table-tbody").append(nullRow);
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function generateApplicationDependenciesTableRow(dependedApplicationName, applicationGroupName, dependencyId, dependedApplicationId) {
    return "<tr class='d-flex'>" +
        "<td class='col-md-4'><a href=/application/detail/" + dependedApplicationId + " target='_blank'>" + dependedApplicationName + "</a></td>" +
        "<td class='col-md-7'>" + applicationGroupName + "</td>" +
        "<td class='col-md-1 btnActions text-center'>" +
        "<a class='btn btn-link' type='button' data-toggle='tooltip' data-placement='top' title='Detay' href=/applicationdependency/detail/" + dependencyId + "><i class='far fa-eye' aria-hidden='true'></i></button>" + "</td>" +
        "</tr>";
}

function GetDatabaseDependenciesByApplicationId(applicationId) {
    $.ajax({
        dataType: 'json',
        type: 'GET',
        url: Urls.GetDatabaseDependenciesByApplicationId,
        data: { applicationId: applicationId },
        success: function (result) {
            $("#database-tab-content-table-tbody").empty();
            if (result.data.length !== 0) {
                $.each(result.data, function (index, value) {
                    var row = generateDatabaseDependenciesTableRow(value.name, value.databaseGroupName, value.id, value.databaseId);
                    $("#database-tab-content-table-tbody").append(row);
                });
                $("#database-count").empty();
                $("#database-count").append(result.data.length + " veritabanı");
            }
            else {
                var nullRow = generateNullTableRow(3);
                $("#database-tab-content-table-tbody").append(nullRow);
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function generateDatabaseDependenciesTableRow(databaseName, databaseGroupName, databaseDependencyId, databaseId) {
    return "<tr class='d-flex'>" +
        "<td class='col-md-4'><a href=/database/detail/" + databaseId + " target='_blank'>" + databaseName + "</a></td>" +
        "<td class='col-md-7'>" + databaseGroupName + "</td>" +
        "<td class='col-md-1 btnActions text-center'>" +
        "<a class='btn btn-link' type='button' data-toggle='tooltip' data-placement='top' title='Detay' href=/databasedependency/detail/" + databaseDependencyId + "><i class='far fa-eye' aria-hidden='true'></i></button>" + "</td>" +
        "</tr>";
}

function GetExternalDependenciesByApplicationId(applicationId) {
    $.ajax({
        dataType: 'json',
        type: 'GET',
        url: Urls.GetExternalDependenciesByApplicationId,
        data: { applicationId: applicationId },
        success: function (result) {
            $("#external-tab-content-table-tbody").empty();
            if (result.data.length !== 0) {
                $.each(result.data, function (index, value) {
                    var row = generateExternalDependenciesTableRow(value.name, value.description, value.id);
                    $("#external-tab-content-table-tbody").append(row);
                });
                $("#external-count").empty();
                $("#external-count").append(result.data.length + " bağımlılık");
            }
            else {
                var nullRow = generateNullTableRow(3);
                $("#external-tab-content-table-tbody").append(nullRow);
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function generateExternalDependenciesTableRow(name, description, externalDependencyId) {
    return "<tr class='d-flex'>" +
        "<td class='col-md-4'>" + name + "</td>" +
        "<td class='col-md-7'>" + description + "</td>" +
        "<td class='col-md-1 btnActions text-center'>" +
        "<a class='btn btn-link' type='button' data-toggle='tooltip' data-placement='top' title='Detay' href=/externaldependency/detail/" + externalDependencyId + "><i class='far fa-eye' aria-hidden='true'></i></button>" + "</td>" +
        "</tr>";
}

function GetNugetPackageDependenciesByApplicationId(applicationId) {
    $.ajax({
        dataType: 'json',
        type: 'GET',
        url: Urls.GetNugetPackageDependenciesByApplicationId,
        data: { applicationId: applicationId },
        success: function (result) {
            $("#nugetpackage-tab-content-table-tbody").empty();
            if (result.data.length !== 0) {
                $.each(result.data, function (index, value) {
                    var row = generateNugetPackageDependenciesTableRow(value.nugetPackageName, value.packageUrl, value.id);
                    $("#nugetpackage-tab-content-table-tbody").append(row);
                });
                $("#nugetpackage-count").empty();
                $("#nugetpackage-count").append(result.data.length + " bağımlılık");
            }
            else {
                var nullRow = generateNullTableRow(3);
                $("#nugetpackage-tab-content-table-tbody").append(nullRow);
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function generateNugetPackageDependenciesTableRow(nugetPackageName, packageUrl, nugetPackageDependencyId) {
    return "<tr class='d-flex'>" +
        "<td class='col-md-4'>" + nugetPackageName + "</td>" +
        "<td class='col-md-7'><a href='" + packageUrl + "' target='_blank'>" + packageUrl + "</a></td>" +
        "<td class='col-md-1 btnActions text-center'>" +
        "<a class='btn btn-link' type='button' data-toggle='tooltip' data-placement='top' title='Detay' href=/nugetpackagedependency/detail/" + nugetPackageDependencyId + "><i class='far fa-eye' aria-hidden='true'></i></button>" + "</td>" +
        "</tr>";
}

function generateSvnTableRow(svnUrl, svnRepositoryTypeName, svnRepositoryId) {
    return "<tr class='d-flex'>" +
        "<td class='col-md-7'>" + svnUrl + "</td>" +
        "<td class='col-md-4'>" + svnRepositoryTypeName + "</td>" +
        "<td class='col-md-1 btnActions text-center'>" +
        "<a class='btn btn-link' type='button' data-toggle='tooltip' data-placement='top' title='Detay' href=/applicationsvn/detail/" + svnRepositoryId + "><i class='far fa-eye' aria-hidden='true'></i></button>" + "</td>" +
        "</tr>";
}

function generateJenkinsTableRow(jobUrl, jenkinsJobTypeName, jenkinsJobId) {
    return "<tr class='d-flex'>" +
        "<td class='col-md-7'><a href=" + jobUrl + " target='_blank' >" + jobUrl + "</a></td>" +
        "<td class='col-md-4'>" + jenkinsJobTypeName + "</td>" +
        "<td class='col-md-1 btnActions text-center'>" +
        "<a class='btn btn-link' type='button' data-toggle='tooltip' data-placement='top' title='Detay' href=/applicationjenkinsjob/detail/" + jenkinsJobId + "><i class='far fa-eye' aria-hidden='true'></i></button>" + "</td>" +
        "</tr>";
}

function generateSonarqubeTableRow(projectUrl, sonarqubeProjectTypeName, sonarqubeProjectId) {
    return "<tr class='d-flex'>" +
        "<td class='col-md-7'><a href=" + projectUrl + " target='_blank' >" + projectUrl + "</a></td>" +
        "<td class='col-md-4'>" + sonarqubeProjectTypeName + "</td>" +
        "<td class='col-md-1 btnActions text-center'>" +
        "<a class='btn btn-link' type='button' data-toggle='tooltip' data-placement='top' title='Detay' href=/applicationsonarqubeproject/detail/" + sonarqubeProjectId + "><i class='far fa-eye' aria-hidden='true'></i></button>" + "</td>" +
        "</tr>";
}

function generateNugetTableRow(nugetPackageName, packageUrl, nugetPackageId) {
    return "<tr class='d-flex'>" +
        "<td class='col-md-4'>" + nugetPackageName + "</td>" +
        "<td class='col-md-7'><a href=" + packageUrl + " target='_blank' >" + packageUrl + "</a></td>" +
        "<td class='col-md-1 btnActions text-center'>" +
        "<a class='btn btn-link' type='button' data-toggle='tooltip' data-placement='top' title='Detay' href=/applicationnugetpackage/detail/" + nugetPackageId + "><i class='far fa-eye' aria-hidden='true'></i></button>" + "</td>" +
        "</tr>";
}

function generateNullTableRow(colspan) {
    return "<tr><td colspan=" + colspan + ">Kayıt Bulunamadı</td></tr>";
}

function generateEnviromentTableRow(environmentName, hasLog, url, id) {
    return "<tr class='d-flex'>" +
        "<td class='col-md-1'>" + environmentName + "</td>" +
        "<td class='col-md-2 col-sm-3'>" + hasLog + "</td>" +
        "<td class='col-md-8 col-sm-6'><a href=" + url + " target='_blank' >" + url + "</a></td>" +
        "<td class='col-md-1 btnActions text-center'>" +
        "<a class='btn btn-link' type='button' data-toggle='tooltip' data-placement='top' title='Detay' href=/applicationenvironment/detail/" + id + "><i class='far fa-eye' aria-hidden='true'></i></button>" + "</td>" +
        "</tr>";
}

function getBuildSettings(applicationId) {
    $.ajax({
        dataType: 'json',
        type: 'GET',
        url: Urls.GetApplicationBuildSettings,
        data: { applicationId: applicationId },
        success: function (result) {
            if (result) {
                $("#workspace").text(result.workspace);
                $("#solution-name").text(result.solutionName);
                $("#project-name").text(result.projectName);
                $("#deploy-path").text(result.deployPath);

                $("#dev-publish-profile").text(result.devPublishProfileName);
                $("#test-publish-profile").text(result.testPublishProfileName);
                $("#pre-prod-publish-profile").text(result.preProdPublishProfileName);
                $("#prod-publish-profile").text(result.prodPublishProfileName);

                $("#dev-remote-address").text(result.devRemoteAddress);
                $("#test-remote-address").text(result.testRemoteAddress);
                $("#pre-prod-remote-address").text(result.preProdRemoteAddress);
                $("#prod-remote-address").text(result.prodRemoteAddress);
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}