<template>
  <div>
    <loading :active.sync="isLoading" :can-cancel="true" :is-full-page="fullPage" :loader="loaderType"></loading>
    <h1 class="title-primary display-4">NuGet Paketi Yükleme Sonucu</h1>
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
    <h4>Paket sunucuya başarıyla yüklendi. Paket bilgileri:</h4>
    </div>
  </div>
</template>
<script>
import axios from 'axios'
import Loading from 'vue-loading-overlay'

export default {
  data () {
    return {
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
