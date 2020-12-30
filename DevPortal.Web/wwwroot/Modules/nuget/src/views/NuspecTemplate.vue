<template>
  <main role="main" class="py-4">
    <loading :can-cancel="true" :is-full-page="fullPage" :loader="loaderType"></loading>
<div>
  <b-row>
  <b-col md="12">
      <h1 class="title-primary display-4">Örnek Nuspec Şablonları</h1>
  </b-col>
  </b-row>
  <b-row class="justify-content-end my-2">
    <div  v-if="Isfavourite">
        <b-button variant="btn btn-simple btn-sm hover-action float-right active"  id="deleteFavourite" @click="DeleteFavourites()">
          <i class="fa fa-fa fa-star" aria-hidden="true"></i> Favorilerimden Çıkar
        </b-button>
    </div>
    <div v-else>
        <b-button variant="btn btn-simple btn-sm hover-action float-right" id="addFavourite" @click="AddFavourites()">
          <i class="fa fa-fa fa-star" aria-hidden="true"></i> Favorilerime Ekle
        </b-button>
    </div>
        <b-button variant="btn btn-simple btn-sm hover-primary" alt="İndir" @click="onDownload()">
          <i class="fa fa-download" aria-hidden="true"></i> Şablonu İndir
        </b-button>
  </b-row>
  <b-tabs justified class="nav-pills flex-column flex-sm-row" active-nav-item-class="flex-sm-fill nav-link active">
    <b-tab title=".Net Standard" active @click="getNuspecTemplate(filePath , netStandardNuspecFileName)"><pre>{{ nuspecText }}</pre></b-tab>
    <b-tab title=".Net Framework" @click="getNuspecTemplate(filePath , netFrameworkNuspecFileName)"><pre>{{ nuspecText }}</pre></b-tab>
    <b-tab title=".Net Standard & .Net Framework" @click="getNuspecTemplate(filePath , netStandardAndNetFrameworkNuspecFileName)"><pre>{{ nuspecText }}</pre></b-tab>
  </b-tabs>
</div>
  </main>
</template>
<script>
import axios from 'axios'
import Loading from 'vue-loading-overlay'

export default {
  data () {
    return {
      fullPage: true,
      loaderType: 'dots',
      errorMessage: 'İndirme işlemi gerçekleştirilemedi, Lütfen dosyanın bulunduğu dizini kontrol ediniz.',
      successMessage: 'İndirme işlemi başarılı',
      nuspecText: '',
      netStandardNuspecFileName: 'netStandardNuspecTemplate.nuspec',
      netFrameworkNuspecFileName: 'netFrameworkNuspecTemplate.nuspec',
      netStandardAndNetFrameworkNuspecFileName: 'netStandardAndNetFrameworkNuspecTemplate.nuspec',
      filePath: '/Modules/nuget/public/assets/',
      fileName: '',
      Isfavourite: false,
      favouritePageName: 'Nuspec Template',
      favouriteId: 0
    }
  },
  components: {
    Loading
  },
  methods: {
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
    onDownload () {
      axios.get(`/download-nuspec-template?path=${this.filePath}&fileName=${this.fileName}`).then(response => {
        console.log(response)
        if (response.data != null) {
          var sampleArr = this.base64ToArrayBuffer(response.data.fileContent)
          this.saveByteArray(this.fileName, sampleArr)
          this.makeToast('BİLGi', 'success', this.successMessage)
        }
      }).catch(error => {
        console.log(error)
        this.makeToast('UYARI', 'danger', this.errorMessage)
        this.isLoading = false
      }).finally(() => {
        this.isLoading = false
      })
    },
    getNuspecTemplate (filePath, fileName) {
      this.fileName = fileName
      axios.get(`/get-nuspec-template?path=${filePath + fileName}`).then(response => {
        console.log(response)
        if (response.data != null) {
          this.nuspecText = response.data.text
        }
      }).catch(error => {
        console.log(error)
        this.nuspecText = 'Dosya içeriği okunamadı.'
        this.isLoading = false
      }).finally(() => {
        this.isLoading = false
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
  },
  created () {
    this.favoriteSearch()
    this.getNuspecTemplate(this.filePath, this.netStandardNuspecFileName)
  }
}
</script>
