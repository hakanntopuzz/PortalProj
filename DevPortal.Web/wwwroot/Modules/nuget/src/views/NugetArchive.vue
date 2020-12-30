<template>
  <div>
    <loading :active.sync="isLoading" :can-cancel="true" :is-full-page="fullPage" :loader="loaderType"></loading>
    <h1 class="title-primary display-4">NuGet Paketleri Arşiv</h1>
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
      <b-col md="6"><h4> Paket Adı </h4></b-col>
      <b-col md="3"><h4> Versiyon </h4></b-col>
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
        <b-card class="my-1" v-for="paket in nugetPackageArchiveList" :key="paket.name">
          <b-row>
            <b-col md="6">
              <h5 class="package-name">
                <router-link :to="'packages/' + paket.name">{{ paket.name }}</router-link>
              </h5>
            </b-col>
            <b-col md="3">
              <b-input-group size="md">
                <b-form-select :id="paket.name" v-model="paket.selectedSubDirectory" :options="paket.subDirectory"></b-form-select>
              </b-input-group>
            </b-col>
            <b-col md="3" class="text-center">
              <b-button variant="outline" alt="İndir" @click="onDownload(paket)">
                <i class="fa fa-download text-primary" aria-hidden="true"></i>
              </b-button>
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
      nugetPackageArchiveList: [],
      searchText: '',
      filteredPackages: [],
      totalCount: 0,
      isLoading: true,
      fullPage: true,
      loaderType: 'dots',
      timeOut: null,
      dismissSecs: 5,
      dismissCountDown: 0,
      errorMessage: 'İndirme işlemi gerçekleştirilemedi, lütfen dosyanın bulunduğu dizini kontrol ediniz.',
      successMessage: 'İndirme işlemi başarılı',
      Isfavourite: false,
      favouritePageName: 'NuGet Paketleri Arşiv',
      favouriteId: 0
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
      axios.get(`/get-filtered-archive-nuget-packages?searchString=${payload.searchString}`).then(response => {
        this.isLoading = true
        response.data.forEach(element => {
          element.subDirectory.unshift('Seçiniz...')
          element['selectedSubDirectory'] = 'Seçiniz...'
        })
        this.nugetPackageArchiveList = response.data
        this.filteredPackages = this.nugetPackageArchiveList
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
    base64ToArrayBuffer (base64) {
      var binaryString = window.atob(base64)
      var binaryLen = binaryString.length
      var bytes = new Uint8Array(binaryLen)
      for (var i = 0; i < binaryLen; i++) {
        var ascii = binaryString.charCodeAt(i)
        bytes[i] = ascii
      }
      return bytes
    },
    saveByteArray (fileName, byte) {
      var blob = new Blob([byte])
      var link = document.createElement('a')
      link.href = window.URL.createObjectURL(blob)
      link.download = fileName
      link.click()
    },
    onDownload (paket) {
      const packageName = paket.name
      const selectedPackageVersion = document.getElementById(packageName).value
      if (selectedPackageVersion && selectedPackageVersion != null && selectedPackageVersion !== '' && selectedPackageVersion !== 'Seçiniz...') {
        this.isLoading = true
        const filePath = `${paket.filePath}/${selectedPackageVersion}/`
        axios.get(`download-nuget-packages?path=${filePath}&fileName=${packageName + '.' + selectedPackageVersion + '.nupkg'}`).then(response => {
          if (response.data != null) {
            var sampleArr = this.base64ToArrayBuffer(response.data.fileContent)
            this.saveByteArray(response.data.name, sampleArr)
            this.makeToast('BİLGi', 'success', this.successMessage)
          }
        }).catch(error => {
          console.log(error)
          this.makeToast('UYARI', 'danger', this.errorMessage)
          this.isLoading = false
        }).finally(() => {
          this.isLoading = false
        })
      } else {
        this.makeToast('UYARI', 'danger', 'Lütfen versiyon seçiniz!')
      }
    },
    countDownChanged (dismissCountDown) {
      this.dismissCountDown = dismissCountDown
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
