<template>
  <main role="main" class="py-4">
    <loading :active.sync="loader.isLoading" :can-cancel="true" :is-full-page="loader.fullPage" :loader="loader.loaderType"></loading>
    <Error v-if="hasError" :message="errorMessage" />
    <div v-if="!hasError && !loader.isLoading">
      <h1 class="title-primary display-4">Paket Detayı</h1>
         <b-row>
      <b-col md="12"  v-if="Isfavourite">
        <b-button variant="btn btn-simple btn-sm hover-action float-right active"  id="deleteFavourite" @click="DeleteFavourites()">
        <i class="fa fa-fa fa-star" aria-hidden="true"></i> Favorilerimden Çıkar
      </b-button>
      </b-col>
      <b-col md="12" v-else>
      <b-button variant="btn btn-simple btn-sm hover-action float-right" id="addFavourite" @click="AddFavourites()">
        <i class="fa fa-fa fa-star" aria-hidden="true"></i> Favorilerime Ekle
      </b-button>
      </b-col>
    </b-row>
      <b-row class="mt-4">
        <b-col>
          <b-media>
            <template v-slot:aside>
                <b-img v-if="nugetPackage.iconUrl !== null && nugetPackage.iconUrl !== ''" blank-color="#ccc" width="64" alt="placeholder" :src="nugetPackage.iconUrl"></b-img>
                <b-img v-else blank-color="#ccc" width="64" alt="placeholder" src="https://www.nuget.org/Content/gallery/img/default-package-icon.svg"></b-img>
            </template>
            <h2 class="font-weight-light">
              {{ nugetPackage.title }}
              <span class="version">{{ nugetPackage.version }}</span>
              <b-icon-check-circle variant="primary ml-1"></b-icon-check-circle>
            </h2>
            <p class="description">{{ nugetPackage.description }}</p>
          </b-media>
        </b-col>
      </b-row>
      <b-row>
        <b-col md="8">
          <div class="properties-div">
            <h3 v-b-toggle.release-notes-div class="dependencies-title font-weight-light">
              <b-icon-chevron-right class="is-passive"></b-icon-chevron-right>
              <b-icon-chevron-down  class="is-active"></b-icon-chevron-down>
              Versiyon Değişiklikleri
            </h3>
            <b-collapse visible id="release-notes-div" class="mt-2 ml-5">
                <p class="text-muted">{{ nugetPackage.releaseNotes }}</p>
            </b-collapse>
          </div>
          <div class="properties-div">
            <h3 v-b-toggle.dependencies-div class="dependencies-title font-weight-light">
              <b-icon-chevron-right class="is-passive"></b-icon-chevron-right>
              <b-icon-chevron-down class="is-active"></b-icon-chevron-down>
              Bağımlılıklar
            </h3>
            <b-collapse id="dependencies-div" class="mt-2 ml-5">
              <template v-for="(item, index) in nugetPackage.dependencies">
                <div :key="index">
                  <span class="framework-title" :key="item.framework">{{ item.framework }}</span>
                  <p class="dependency" v-for="subitem in item.dependencies" :key="subitem.dependencies">{{ subitem.name }} <span class="subitem-version">(>= {{ subitem.version }})</span></p>
                </div>
              </template>
            </b-collapse>
          </div>
          <div class="properties-div" v-if="nugetPackage.versionHistory.items.length > 0">
            <h3 v-b-toggle.version-history-div class="dependencies-title font-weight-light">
              <b-icon-chevron-right class="is-passive"></b-icon-chevron-right>
              <b-icon-chevron-down  class="is-active"></b-icon-chevron-down>
              Versiyon Geçmişi
            </h3>
            <b-collapse id="version-history-div" class="mt-2">
              <b-table :items="nugetPackage.versionHistory.items" :fields="nugetPackage.versionHistory.fields" class="my-table">
                <template v-slot:cell(version)="data">
                  <a :href="href(data.item.version, nugetPackage.version)" @click="onVersionChange(data.item.version)">{{ data.item.version }}</a>
                </template>
                <template v-slot:cell(archive)="data">
                  <div class="ml-4" v-if="data.item.archive">
                  <span title="Arşivlendi" ><b-icon-check font-scale="2" variant="success" ></b-icon-check></span>
                  </div>
                  <div class="ml-4" v-else>
                    <b-icon-x font-scale="2" variant="danger" ></b-icon-x><a href="#" @click="addArchive(nugetPackage.id,data.item.version)">Arşive Ekle</a>
                  </div>
                </template>
              </b-table>
            </b-collapse>
          </div>
        </b-col>
        <b-col md="4">
          <div class="properties-div">
            <h3 class="dependencies-title font-weight-light">Paket Bilgisi</h3>
            <p class="text-muted"><b-icon-clock ></b-icon-clock> Son Güncellenme Tarihi: {{ nugetPackage.lastUpdateDate }}</p>
            <p><b-icon-arrow-bar-up ></b-icon-arrow-bar-up> <a :href="nugetPackage.projectUrl" target="_">Proje Url</a></p>
          </div>
          <div class="properties-div">
            <h3 class="dependencies-title font-weight-light">Etiketler</h3>
            <p class="text-muted"><b-icon-tag ></b-icon-tag> {{ nugetPackage.tags }}</p>
          </div>
          <div class="properties-div" v-if="IsAdmin">
            <h3 class="dependencies-title font-weight-light">Diğer işlemler</h3>
            <p><b-icon-plug ></b-icon-plug> <a class="text-primary" @click="onMoveNugetPackageToOldPackagesFile(nugetPackage.id)">Emekliye Ayır</a></p>
          </div>
        </b-col>
      </b-row>
    </div>
  </main>
</template>
<script>
import axios from 'axios'
import Loading from 'vue-loading-overlay'
import Error from '../components/shared/Error'
import { utilities } from '../helpers'

export default {
  data () {
    return {
      hasError: false,
      errorMessage: 'Arşivleme işlemi gerçekleştirilemedi.',
      successMessage: 'Arşivleme işlemi başarılı.',
      returnUrl: null,
      loader: {
        isLoading: true,
        fullPage: true,
        loaderType: 'dots'
      },
      nugetPackage: {
        id: '',
        title: '',
        version: '',
        description: '',
        releaseNotes: '',
        lastUpdateDate: null,
        projectUrl: '',
        tags: '',
        archive: false,
        iconUrl: '',
        dependencies: [],
        versionHistory: {
          fields: [
            {
              key: 'version',
              label: 'Versiyon'
            },
            {
              key: 'published',
              label: 'Son Güncellenme Tarihi'
            },
            {
              key: 'archive',
              label: 'Arşiv Durumu'
            }
          ],
          items: []
        }
      },
      nugetPackageList: [],
      favouritePageName: 'NuGet Detail',
      favouriteId: 0,
      Isfavourite: false,
      IsAdmin: false
    }
  },
  computed: {
    id: function () {
      return this.$route.params.id
    },
    version: function () {
      return this.$route.params.version
    }
  },
  components: {
    Loading,
    Error
  },
  methods: {
    favoriteSearch () {
      var pageUrl = window.location.pathname + window.location.search
      axios.get(`check-page-is-favourites?pageUrl=${pageUrl}`).then(response => {
        if (response.data.isSuccess === true) {
          this.favouriteId = response.data.data.id
          this.Isfavourite = true
        } else {
          this.Isfavourite = false
        }
      }).catch(error => {
        this.makeToast('HATA', 'danger', error)
      }).finally(() => {
        this.isLoading = false
      })
    },
    addFavorite (favouritePageName, pageUrl) {
      let addToFavouritesRequest = {
        PageTitle: favouritePageName,
        PageUrl: pageUrl
      }
      axios.post('add-to-favourites', addToFavouritesRequest).then(response => {
        if (response.data.isSuccess === true) {
          this.$swal(' ', response.data.message, 'success')
          this.favoriteSearch()
        } else {
          this.$swal(' ', response.data.message, 'danger')
        }
      }).catch(error => {
        this.makeToast('HATA', 'danger', error)
      }).finally(() => {
        this.isLoading = false
      })
    },
    deleteFavorite (id) {
      let removeFromFavouritesRequest = {
        Id: id
      }
      axios.post('remove-from-favourites', removeFromFavouritesRequest).then(response => {
        if (response.data.isSuccess === true) {
          this.$swal(' ', response.data.message, 'success')
          this.favoriteSearch()
        } else {
          this.$swal(' ', response.data.message, 'danger')
        }
      }).catch(error => {
        this.makeToast('HATA', 'danger', error)
      }).finally(() => {
        this.isLoading = false
      })
    },
    AddFavourites () {
      var pageUrl = window.location.pathname + window.location.search
      this.addFavorite(this.favouritePageName, pageUrl)
    },
    DeleteFavourites () {
      var id = this.favouriteId
      this.deleteFavorite(id)
    },
    getPackageProperties () {
      if (!utilities.isNullOrEmpty(this.id)) {
        axios.get('/get-nuget-packages-by-title?packageTitle=' + this.id + '').then(response => {
          this.loader.isLoading = true
          this.nugetPackageList = response.data.nugetPackages.reverse()
          this.IsAdmin = response.data.isAuthorized
          this.setPackages(this.nugetPackageList)
        }).catch(error => {
          console.log(error)
        }).finally(() => {
          this.loader.isLoading = false
        })
      }
    },
    groupAllDependencies (dependencies) {
      let dependencyArray = []

      dependencies.forEach(element => {
        if (!element.framework) {
          element.framework = 'Unknown'
        }
        var dependency = dependencyArray.filter((item) => { return item.framework === element.framework })

        if (dependency.length === 1) {
          dependency[0].dependencies.push({
            name: element.name,
            version: element.version
          })
        } else {
          dependencyArray.push({
            framework: element.framework,
            dependencies: [{
              name: element.name,
              version: element.version
            }]
          })
        }
      })

      return dependencyArray
    },
    addArchive (title, version) {
      console.log(title)
      console.log(version)
      axios.get(`/add-archive-nupkg-file?title=${title}&versionItem=${version}`).then(response => {
        this.isLoading = true
        if (response.data === true) {
          this.makeToast('BİLGi', 'success', this.successMessage)
        } else {
          this.makeToast('UYARI', 'danger', this.errorMessage)
        }
        this.getPackageProperties()
      }).catch(error => {
        console.log(error)
        this.makeToast('UYARI', 'danger', this.errorMessage)
        this.isLoading = false
      }).finally(() => {
        this.isLoading = false
      })
    },
    onMoveNugetPackageToOldPackagesFile (id) {
      console.log(id)
      this.$swal({
        title: 'Emin misiniz?',
        text: 'Bu paket emekliye ayrılacaktır!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#c70039',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Evet',
        cancelButtonText: 'Hayır'
      }).then((result) => {
        if (result.value) {
          console.log(id)
          axios.get(`/move-nuget-packages-to-old-packages-file?title=${id}`).then(response => {
            this.isLoading = true
            if (response.data === true) {
              this.makeToast('BİLGi', 'success', 'Emekliye alma işlemi başarılı')
              window.location.href = '/nuget/packages'
            } else {
              this.makeToast('UYARI', 'danger', 'Emekliye alma işlemi gerçekleştirilemedi. İlgili dizini kontrol ediniz.')
            }
          }).catch(error => {
            console.log(error)
            this.makeToast('UYARI', 'danger', 'Emekliye alma işlemi gerçekleştirilemedi. İlgili dizini kontrol ediniz.')
            this.isLoading = false
          }).finally(() => {
            this.isLoading = false
          })
          // this.$swal('Deleted', 'You successfully deleted this file', 'success')
        } else {
          // this.$swal('Cancelled', 'Your file is still intact', 'info')
        }
      })
    },
    makeToast (title, variant, message) {
      this.$bvToast.toast(message, {
        title: ` ${title || 'UYARI'}`,
        variant: variant,
        solid: true
      })
    },
    onVersionChange (versionNumber) {
      let version = this.$route.params.version
      if (version !== versionNumber) {
        if (version && version != null && version !== '') {
          this.$router.push(
            {
              name: 'NugetDetail',
              params:
              {
                id: this.id,
                version: versionNumber
              }
            })
        } else {
          this.$router.push(
            {
              name: 'NugetDetail',
              params:
              {
                id: this.id,
                version: versionNumber
              }
            })
        }
      }
    },
    getNugetPackageVersionHistory (nugetPackageList) {
      let versionHistoryItems = []

      nugetPackageList.forEach(element => {
        versionHistoryItems.push({
          version: element.properties.version,
          published: this.$moment(new Date(element.properties.published)).format('DD MMMM YYYY HH:mm'),
          archive: element.properties.archive
        })
      })

      return versionHistoryItems
    },
    setSelectedNugetPackageProperties (nugetPackageList, version) {
      let nugetPackage = nugetPackageList[0].properties

      if (!utilities.isNullOrEmpty(version)) {
        let nugetPackages = nugetPackageList.filter((item) => { return item.properties.version === this.version })

        if (nugetPackages.length === 0) {
          this.hasError = true
          this.errorMessage = 'Paket bulunamadı'
          return
        }

        this.hasError = false
        nugetPackage = nugetPackages[0].properties
      }

      return nugetPackage
    },
    setDependencies (dependencies) {
      let splittedDependencies = dependencies.split('|')
      let allDependencies = []
      splittedDependencies.forEach(element => {
        const splittedDependency = element.split(':')
        allDependencies.push({
          name: splittedDependency[0],
          version: splittedDependency[1],
          framework: splittedDependency[2]
        })
      })

      return this.groupAllDependencies(allDependencies)
    },
    href: function (rowVersion, currentVersion) {
      return rowVersion !== currentVersion ? 'javascript:void(0)' : null
    },
    archive: function (rowVersion, currentVersion) {
      return rowVersion === currentVersion ? 'javascript:void(0)' : null
    },
    setPackages (nugetPackageList) {
      if (nugetPackageList.length !== 0) {
        this.hasError = false
        let selectedNugetPackageProperties = this.setSelectedNugetPackageProperties(nugetPackageList, this.version)
        this.nugetPackage.title = selectedNugetPackageProperties.title
        this.nugetPackage.version = selectedNugetPackageProperties.version
        this.nugetPackage.description = selectedNugetPackageProperties.description
        this.nugetPackage.releaseNotes = selectedNugetPackageProperties.releaseNotes
        this.nugetPackage.tags = selectedNugetPackageProperties.tags
        this.nugetPackage.lastUpdateDate = this.$moment(new Date(selectedNugetPackageProperties.published)).format('DD-MM-YYYY hh:mm')
        this.nugetPackage.projectUrl = selectedNugetPackageProperties.projectUrl
        this.nugetPackage.dependencies = this.setDependencies(selectedNugetPackageProperties.dependencies)
        this.nugetPackage.versionHistory.items = this.getNugetPackageVersionHistory(nugetPackageList)
        this.nugetPackage.archive = this.isArchived(this.nugetPackage.version, this.nugetPackage.versionHistory.items)
        this.nugetPackage.iconUrl = selectedNugetPackageProperties.iconUrl
        this.nugetPackage.id = selectedNugetPackageProperties.id
      } else {
        this.hasError = true
        this.errorMessage = 'Paket bulunamadı.'
      }
    },
    isArchived (version, versiyonHistory) {
      return versiyonHistory.filter((element) => { return element.version === version })[0].archive
    }
  },
  created () {
    this.favoriteSearch()
    this.getPackageProperties()
  },
  watch: {
    '$route.params.version' (version) {
      this.setPackages(this.nugetPackageList)
    }
  }
}
</script>

<style scoped>
.collapsed > .is-active,
 :not(.collapsed) > .is-passive {
    display: none;
    }
.jumbotron {
    padding: 1rem 1rem !important;
}
.dependencies-title {
  cursor: pointer;
  outline: none;
}
.version {
  font-size: 20px;
  font-weight: 400;
  color: grey;
}
.properties-div {
  margin-top: 30px;
}
.description {
  font-weight: 500;
}
.framework-title {
  font-size: 25px;
  font-weight: 300;
  color: grey;
}
#dependencies-div,#release-notes-div div {
  margin: 20px 0px 0px 20px;
}
.subitem-version {
  font-size: 15px;
  font-weight: 400;
  color: grey;
  margin-left: 10px;
}
.dependency {
  margin: 0.7rem;
}
.container {
  margin-bottom: 30px;
}
.return-to-packages {
  margin-top: -20px;
  margin-bottom: 10px;
}
.media h1, .media p {
  word-break: break-all;
}
.table {
  margin-top: 30px;
}
</style>
