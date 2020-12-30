<template>
  <div>
    <loading :active.sync="isLoading" :can-cancel="true" :is-full-page="fullPage" :loader="loaderType"></loading>
    <h1 class="title-primary display-4">NuGet Paketi Yükleme</h1>
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
    <div>
    <h4>Paket yüklemek için nuspec dosyanızı seçin.</h4>
    <div class="mt-3">Dosya: {{ file ? file.name : '' }}</div>
    <b-form-file v-model="file" class="mt-3" accept=".nuspec" plain></b-form-file>
    <br>
    <b-button variant="primary" @click="uploadNewNugetPackage()">Yayına Al</b-button>
    </div>
  </div>
</template>
<script>
import axios from 'axios'
import Loading from 'vue-loading-overlay'

export default {
  data () {
    return {
      file: null,
      Isfavourite: false,
      favouritePageName: 'NuGet Paketleri Arşiv',
      favouriteId: 0
    }
  },
  components: {
    Loading
  },
  created () {
    this.favoriteSearch()
  },
  methods: {
    makeToast (title, variant, message) {
      this.$bvToast.toast(message, {
        title: ` ${title || 'UYARI'}`,
        variant: variant,
        solid: true
      })
    },
    uploadNewNugetPackage () {
      this.$swal({
        title: 'Emin misiniz?',
        text: 'Paket Nuget sunucusuna yüklenecek. Bu işlem geri alınamaz.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#c70039',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Evet',
        cancelButtonText: 'İptal'
      }).then((result) => {
        if (result.value) {
          axios.get(`/upload-new-nuget-package?fileName=${this.file.name}`).then(response => {
            this.isLoading = true
            if (response.data === true) {
              this.makeToast('BİLGi', 'success', 'Paketi sunucuya yükleme işlemi başarılı')
              window.location.href = '/nuget/uploadNewNugetPackageResult'
            } else {
              this.makeToast('UYARI', 'danger', 'Paketi sunucuya yükleme işlemi gerçekleştirilemedi.')
            }
          }).catch(error => {
            console.log(error)
            this.makeToast('UYARI', 'danger', 'Paketi sunucuya yükleme işlemi gerçekleştirilemedi.')
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
