<template>
  <div>
    <loading :active.sync="isLoading" :can-cancel="true" :is-full-page="fullPage" :loader="loaderType"></loading>
    <h1 class="title-primary display-4">Emekli NuGet Paketleri</h1>
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
    <br>
    <b-row>
      <b-col md="9"><h4> Paket Adı </h4></b-col>
      <b-col md="3">
        <b-input-group size="md">
          <b-input-group-prepend is-text>
            <b-icon icon="search"></b-icon>
          </b-input-group-prepend>
          <b-form-input type="search" placeholder="Paket ara" v-model="searchText" @input="onSearch"></b-form-input>
        </b-input-group>
        </b-col>
    </b-row>
    <br>
    <b-row>
      <b-col md="12">
        <b-card class="my-1" v-for="paket in oldNugetPackageList" :key="paket.name">
          <b-row >
            <b-col md="8">
              <h5 class="package-name">
                {{ paket.name }}
              </h5>
            </b-col>
            <b-col md="4" class="text-center" v-if="IsAdmin">
              <b-button variant="outline-primary" alt="Tekrar Yayına Al" @click="onMoveOldPackageToNugetPackageFile(paket.name)"><b-icon-arrow-clockwise></b-icon-arrow-clockwise>Tekrar Yayına Al</b-button>
            </b-col>
          </b-row>
        </b-card>
      </b-col>
    </b-row>
    <br>
  </div>
</template>
<script>
import axios from 'axios'
import Loading from 'vue-loading-overlay'

export default {
  data () {
    return {
      oldNugetPackageList: [],
      searchText: '',
      filteredPackages: [],
      totalCount: 0,
      isLoading: true,
      fullPage: true,
      loaderType: 'dots',
      timeOut: null,
      dismissSecs: 5,
      dismissCountDown: 0,
      errorMessage: 'Yayına alma işlemi gerçekleştirilemedi. İlgili dizini kontrol ediniz.',
      successMessage: 'Yayına alma işlemi başarılı',
      Isfavourite: false,
      favouritePageName: 'Emekli NuGet Paketleri',
      favouriteId: 0,
      IsAdmin: false
    }
  },
  components: {
    Loading
  },
  created () {
    this.getPackages()
    this.favoriteSearch()
  },
  methods: {
    getPackages () {
      let payload = {
        searchString: this.searchText
      }
      axios.get(`/get-filtered-old-nuget-packages?searchString=${payload.searchString}`).then(response => {
        this.isLoading = true
        this.IsAdmin = response.data.isAuthorized
        response.data.nugetPackageFolders.forEach(element => {
          element.subDirectory.unshift('Seçiniz...')
          element['selectedSubDirectory'] = 'Seçiniz...'
        })
        this.oldNugetPackageList = response.data.nugetPackageFolders
        this.filteredPackages = this.oldNugetPackageList
      }).catch(error => {
        console.log(error)
        this.isLoading = false
      }).finally(() => {
        this.isLoading = false
      })
    },
    onSearch (val) {
      if (this.timeOut) {
        clearTimeout(this.timeOut)
      }
      this.timeOut = setTimeout(() => {
        this.getPackages()
      }, 500)
    },
    onMoveOldPackageToNugetPackageFile (id) {
      this.$swal({
        title: 'Emin misiniz?',
        text: 'Bu paket tekrar yayına alınacaktır!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#c70039',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Evet',
        cancelButtonText: 'Hayır'
      }).then((result) => {
        if (result.value) {
          console.log(id)
          axios.get(`/move-old-package-to-nuget-packages-file?title=${id}`).then(response => {
            this.isLoading = true
            if (response.data === true) {
              this.makeToast('BİLGi', 'success', this.successMessage)
            } else {
              this.makeToast('UYARI', 'danger', this.errorMessage)
            }
            this.getPackages()
          }).catch(error => {
            console.log(error)
            this.makeToast('UYARI', 'danger', this.errorMessage)
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
    }
  }
}
</script>
